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

    // TEMP: until JWT exists, we “pretend” the borrower is user "john"
    private async Task<User> GetCurrentUserAsync()
    {
        var user = await _db.Users.FirstAsync(u => u.Username == "john");
        return user;
    }

    [HttpPost]
    public async Task<ActionResult<CreateBorrowResponseDto>> CreateBorrow([FromBody] CreateBorrowRequestDto req)
    {
        var user = await GetCurrentUserAsync();

        var tool = await _db.Tools.FirstOrDefaultAsync(t => t.Id == req.ToolId);
        if (tool is null) return NotFound("Tool not found");
        if (req.Quantity <= 0) return BadRequest("Quantity must be > 0");
        if (tool.Quantity < req.Quantity) return Conflict("Not enough quantity available");

        var borrow = new Borrow
        {
            Id = Guid.NewGuid(),
            UsersId = user.Id,
            Status = "Pending",
            Date = DateTime.UtcNow,
            Price = tool.Price * req.Quantity
        };

        _db.Borrows.Add(borrow);

        var pb = new ProductBorrow
        {
            Id = Guid.NewGuid(),
            BorrowId = borrow.Id,
            ToolId  = tool.Id,
            Quantity  = req.Quantity
        };

        _db.ProductBorrows.Add(pb);

        // Reserve stock immediately (simple)
        tool.Quantity -= req.Quantity;

        await _db.SaveChangesAsync();

        return Ok(new CreateBorrowResponseDto(borrow.Id, (decimal)borrow.Price));
    }

    [HttpGet("{borrowId:guid}/items")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBorrowItemNames(Guid borrowId)
    {
        var names = await _db.ProductBorrows
            .Where(pb => pb.BorrowId == borrowId)
            .Include(pb => pb.Tool)
            .Select(pb => pb.Tool.Name)
            .ToListAsync();

        return Ok(names);
    }
}
