using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRO.Data.Context;
using PRO.Models;
using Pro.Shared.Dtos;

namespace Pro.Server.Controllers;

[ApiController]
[Route("api/borrows")]
public class BorrowsController : ControllerBase
{
    private readonly ToolLendingContext _db;
    public BorrowsController(ToolLendingContext db) => _db = db;

    private Guid CurrentUserId()
        => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    private static readonly string[] ActiveStatuses = { "Pending", "Paid", "Confirmed" };
    private bool IsAdmin() => User.IsInRole("Admin");

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<CreateBorrowResponseDto>> CreateBorrow([FromBody] CreateBorrowRequestDto req)
    {
        if (req.Quantity <= 0) return BadRequest("Quantity must be > 0");

        var tool = await _db.Tools.FirstOrDefaultAsync(t => t.Id == req.ToolId);
        if (tool is null) return NotFound("Tool not found");

        if (tool.Quantity < req.Quantity) return Conflict("Not enough quantity available");

        var days = 1;

        var today = DateTime.UtcNow.Date;
        var horizonDays = 365;

        var hasSchedules = await _db.Schedules.AnyAsync(s => s.ToolsId == tool.Id);

        DateTime? chosenStart = null;
        DateTime? chosenEnd = null;

        for (int i = 0; i < horizonDays; i++)
        {
            var start = today.AddDays(i);
            var end = start.AddDays(days);

            if (hasSchedules)
            {
                var fitsSchedule = await _db.Schedules.AnyAsync(s =>
                    s.ToolsId == tool.Id &&
                    s.AvailableFrom.Date <= start &&
                    s.AvailableTo.Date >= end
                );

                if (!fitsSchedule) continue;
            }

            var reserved = await _db.ProductBorrows
                .Where(pb => pb.ToolId == tool.Id)
                .Join(_db.Borrows,
                    pb => pb.BorrowId,
                    b => b.Id,
                    (pb, b) => new { pb.Quantity, b.StartDate, b.EndDate, b.Status })
                .Where(x =>
                    ActiveStatuses.Contains(x.Status) &&
                    x.StartDate.Date < end &&
                    x.EndDate.Date > start
                )
                .SumAsync(x => (int?)x.Quantity) ?? 0;

            if (reserved + req.Quantity <= tool.Quantity)
            {
                chosenStart = start;
                chosenEnd = end;
                break;
            }
        }

        if (chosenStart is null || chosenEnd is null)
            return Conflict("No availability found in the next 12 months.");

        var userId = CurrentUserId();

        var borrow = new Borrow
        {
            Id = Guid.NewGuid(),
            UsersId = userId,
            Status = "Pending",
            Date = DateTime.UtcNow,
            StartDate = chosenStart.Value,
            EndDate = chosenEnd.Value,
            Price = tool.Price * req.Quantity * days
        };

        _db.Borrows.Add(borrow);

        _db.ProductBorrows.Add(new ProductBorrow
        {
            Id = Guid.NewGuid(),
            BorrowId = borrow.Id,
            ToolId = tool.Id,
            Quantity = req.Quantity
        });

        await _db.SaveChangesAsync();

        return Ok(new CreateBorrowResponseDto(
            borrow.Id,
            (decimal)borrow.Price,
            borrow.StartDate,
            borrow.EndDate
        ));
    }

    [Authorize]
    [HttpGet("{borrowId:guid}/items")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBorrowItemNames(Guid borrowId)
    {
        var borrow = await _db.Borrows.AsNoTracking().FirstOrDefaultAsync(b => b.Id == borrowId);
        if (borrow is null) return NotFound("Borrow not found");

        var userId = CurrentUserId();
        var isAdmin = User.IsInRole("Admin");

        if (!isAdmin && borrow.UsersId != userId)
            return Forbid();

        var names = await _db.ProductBorrows
            .Where(pb => pb.BorrowId == borrowId)
            .Include(pb => pb.Tool)
            .Select(pb => pb.Tool.Name)
            .ToListAsync();

        return Ok(names);
    }
    
    [Authorize]
[HttpPost("{borrowId:guid}/return")]
public async Task<ActionResult<ReturnDto>> CreateReturn(Guid borrowId, [FromBody] CreateReturnRequestDto req)
{
    if (string.IsNullOrWhiteSpace(req.Condition)) return BadRequest("Condition is required.");
    if (string.IsNullOrWhiteSpace(req.Damage)) return BadRequest("Damage is required.");

    var borrow = await _db.Borrows.FirstOrDefaultAsync(b => b.Id == borrowId);
    if (borrow is null) return NotFound("Borrow not found");

    var userId = CurrentUserId();
    if (!IsAdmin() && borrow.UsersId != userId) return Forbid();

    if (borrow.Status != BorrowStatuses.Paid && borrow.Status != BorrowStatuses.Confirmed)
        return Conflict($"Cannot return borrow in status '{borrow.Status}'.");


    var already = await _db.Returns.AnyAsync(r => r.BorrowsId == borrowId);
    if (already) return Conflict("Return already exists for this borrow.");

    var now = DateTime.UtcNow;

    var ret = new Return
    {
        Id = Guid.NewGuid(),
        Date = now,
        Condition = req.Condition.Trim(),
        Damage = req.Damage.Trim(),
        BorrowsId = borrowId
    };

    _db.Returns.Add(ret);

    borrow.Status = BorrowStatuses.Returned;

    await _db.SaveChangesAsync();

    return Ok(new ReturnDto(ret.Id, ret.Date, ret.Condition, ret.Damage, ret.BorrowsId));
}

[Authorize]
[HttpGet("{borrowId:guid}/return")]
public async Task<ActionResult<ReturnDto>> GetReturn(Guid borrowId)
{
    var borrow = await _db.Borrows.AsNoTracking().FirstOrDefaultAsync(b => b.Id == borrowId);
    if (borrow is null) return NotFound("Borrow not found");

    var userId = CurrentUserId();
    if (!IsAdmin() && borrow.UsersId != userId) return Forbid();

    var ret = await _db.Returns.AsNoTracking().FirstOrDefaultAsync(r => r.BorrowsId == borrowId);
    if (ret is null) return NotFound("Return not found");

    return Ok(new ReturnDto(ret.Id, ret.Date, ret.Condition, ret.Damage, ret.BorrowsId));
}

[Authorize(Roles = "Admin")]
[HttpPost("{borrowId:guid}/return/finalize")]
public async Task<IActionResult> FinalizeReturn(Guid borrowId, [FromBody] FinalizeReturnRequestDto req)
{
    var borrow = await _db.Borrows.FirstOrDefaultAsync(b => b.Id == borrowId);
    if (borrow is null) return NotFound("Borrow not found");

    var ret = await _db.Returns.FirstOrDefaultAsync(r => r.BorrowsId == borrowId);
    if (ret is null) return NotFound("Return not found");

    if (borrow.Status != BorrowStatuses.Returned)
        return Conflict($"Borrow must be '{BorrowStatuses.Returned}' to finalize.");

    var pbToolIds = await _db.ProductBorrows
        .Where(pb => pb.BorrowId == borrowId)
        .Select(pb => pb.ToolId)
        .ToListAsync();

    var deposits = await _db.SecurityDeposits
        .Where(sd => sd.UsersId == borrow.UsersId && pbToolIds.Contains(sd.ToolsId))
        .OrderByDescending(sd => sd.RefundDate)
        .ToListAsync();

    var newStatus = (req.Status ?? "").Trim();
    if (newStatus != DepositStatuses.Refunded && newStatus != DepositStatuses.Withheld)
        return BadRequest("Invalid deposit status. Allowed: Refunded, Withheld.");

    var now = DateTime.UtcNow;
    foreach (var d in deposits)
    {
        d.Status = newStatus;
        d.RefundDate = now;
    }

    borrow.Status = BorrowStatuses.Completed;

    await _db.SaveChangesAsync();
    return NoContent();
}

}