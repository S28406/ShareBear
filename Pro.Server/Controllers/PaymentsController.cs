using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRO.Data.Context;
using PRO.Models;
using Pro.Shared.Dtos;

namespace Pro.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly ToolLendingContext _db;
    private Guid CurrentUserId()
        => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    private bool IsAdmin()
        => User.IsInRole("Admin");
    public PaymentsController(ToolLendingContext db) => _db = db;

    [HttpPost("confirm")]
    [HttpPost("confirm")]
    public async Task<IActionResult> Confirm([FromBody] PaymentConfirmRequestDto req)
    {
        var borrow = await _db.Borrows.FirstOrDefaultAsync(b => b.Id == req.BorrowId);
        if (borrow is null) return NotFound("Borrow not found");

        var userId = CurrentUserId();
        if (!IsAdmin() && borrow.UsersId != userId)
            return Forbid();

        var alreadyPaid = await _db.Payments.AnyAsync(p =>
            p.OrdersId == borrow.Id && p.Status == "Paid");
        if (alreadyPaid) return Conflict("This borrow is already paid.");

        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            OrdersId = borrow.Id,
            Date = DateTime.UtcNow,
            Ammount = borrow.Price,
            Method = string.IsNullOrWhiteSpace(req.Method) ? "Fake" : req.Method.Trim(),
            Status = "Paid"
        };

        _db.Payments.Add(payment);
        borrow.Status = "Paid";

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("history")]
    public async Task<ActionResult<IReadOnlyList<PaymentHistoryItemDto>>> GetHistory(
        [FromQuery] DateTime? fromUtc,
        [FromQuery] DateTime? toUtc)
    {
        var q = _db.Payments.AsQueryable();

        if (!IsAdmin())
        {
            var userId = CurrentUserId();
            q = q.Where(p => p.Borrow.UsersId == userId);
        }

        if (fromUtc is not null) q = q.Where(p => p.Date >= fromUtc.Value);
        if (toUtc is not null) q = q.Where(p => p.Date <= toUtc.Value);

        var list = await q
            .OrderByDescending(p => p.Date)
            .Select(p => new PaymentHistoryItemDto(
                p.Id,
                p.Date,
                (decimal)p.Ammount,
                p.Status,
                p.Method,
                p.OrdersId
            ))
            .ToListAsync();

        return Ok(list);
    }

}