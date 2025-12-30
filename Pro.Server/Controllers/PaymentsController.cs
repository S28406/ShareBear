using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRO.Data.Context;
using PRO.Models;
using Pro.Shared.Dtos;

namespace Pro.Server.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly ToolLendingContext _db;

    public PaymentsController(ToolLendingContext db) => _db = db;

    [HttpPost("confirm")]
    public async Task<IActionResult> Confirm([FromBody] PaymentConfirmRequestDto req)
    {
        var borrow = await _db.Borrows.FirstOrDefaultAsync(b => b.ID == req.BorrowId);
        if (borrow is null) return NotFound("Borrow not found");

        var payment = new Payment
        {
            ID = Guid.NewGuid(),
            Orders_ID = borrow.ID,
            Date = DateTime.UtcNow,
            Ammount = (float)req.Amount,
            Method = req.Method,
            Status = req.Status
        };

        _db.Payments.Add(payment);

        // Optional: mark borrow paid
        if (req.Status.Equals("Paid", StringComparison.OrdinalIgnoreCase))
            borrow.Status = "Paid";

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("history")]
    public async Task<ActionResult<IReadOnlyList<PaymentHistoryItemDto>>> GetHistory([FromQuery] DateTime? fromUtc, [FromQuery] DateTime? toUtc)
    {
        var q = _db.Payments.AsQueryable();

        if (fromUtc is not null) q = q.Where(p => p.Date >= fromUtc.Value);
        if (toUtc is not null) q = q.Where(p => p.Date <= toUtc.Value);

        var list = await q
            .OrderByDescending(p => p.Date)
            .Select(p => new PaymentHistoryItemDto(
                p.ID,
                p.Date,
                (decimal)p.Ammount,
                p.Status,
                p.Method,
                p.Orders_ID
            ))
            .ToListAsync();

        return Ok(list);
    }
}