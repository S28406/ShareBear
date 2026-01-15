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

    public PaymentsController(ToolLendingContext db) => _db = db;

    private Guid CurrentUserId()
        => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    private bool IsAdmin()
        => User.IsInRole("Admin");

    private static string NormalizeMethod(string? method)
        => string.IsNullOrWhiteSpace(method) ? "Fake" : method.Trim();

    private static string MakeReceiptNumber(Guid paymentId, DateTime utcNow)
    {
        // Thesis-friendly receipt number
        // Example: TR-20260112-AB12CD
        var shortId = paymentId.ToString("N")[..6].ToUpperInvariant();
        return $"TR-{utcNow:yyyyMMdd}-{shortId}";
    }

    [HttpPost("initiate")]
    public async Task<ActionResult<PaymentInitiateResponseDto>> Initiate([FromBody] PaymentInitiateRequestDto req)
    {
        var borrow = await _db.Borrows.FirstOrDefaultAsync(b => b.Id == req.BorrowId);
        if (borrow is null) return NotFound("Borrow not found");

        var userId = CurrentUserId();
        if (!IsAdmin() && borrow.UsersId != userId)
            return Forbid();

        // If there is already an Initiated/Confirmed payment for this borrow, return it (idempotent)
        var existing = await _db.Payments
            .Where(p => p.OrdersId == borrow.Id &&
                        (p.Status == PaymentStatuses.Initiated || p.Status == PaymentStatuses.Confirmed))
            .OrderByDescending(p => p.Date)
            .FirstOrDefaultAsync();

        if (existing is not null)
        {
            return Ok(new PaymentInitiateResponseDto(
                existing.Id,
                borrow.Id,
                existing.Date,
                (decimal)existing.Ammount,
                existing.Status,
                existing.Method
            ));
        }

        var now = DateTime.UtcNow;

        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            OrdersId = borrow.Id,
            Date = now,
            Ammount = (float)borrow.Price, // borrow.Price is float in your model
            Method = NormalizeMethod(req.Method),
            Status = PaymentStatuses.Initiated
        };

        _db.Payments.Add(payment);
        await _db.SaveChangesAsync();

        return Ok(new PaymentInitiateResponseDto(
            payment.Id,
            borrow.Id,
            payment.Date,
            (decimal)payment.Ammount,
            payment.Status,
            payment.Method
        ));
    }

    [HttpPost("confirm")]
    public async Task<ActionResult<PaymentConfirmResponseDto>> Confirm([FromBody] Pro.Shared.Dtos.PaymentConfirmRequestDto req)
    {
        var payment = await _db.Payments
            .Include(p => p.Borrow)
            .FirstOrDefaultAsync(p => p.Id == req.PaymentId);

        if (payment is null) return NotFound("Payment not found");

        var borrow = payment.Borrow;
        if (borrow is null) return Problem("Payment has no borrow linked (data integrity issue).");

        var userId = CurrentUserId();
        if (!IsAdmin() && borrow.UsersId != userId)
            return Forbid();

        if (payment.Status == PaymentStatuses.Confirmed)
        {
            var receiptNumberExisting = MakeReceiptNumber(payment.Id, payment.Date);
            return Ok(new PaymentConfirmResponseDto(payment.Id, payment.Status, payment.Date, receiptNumberExisting));
        }

        if (payment.Status != PaymentStatuses.Initiated)
            return Conflict($"Cannot confirm payment in status '{payment.Status}'.");

        payment.Method = NormalizeMethod(req.Method);
        payment.Status = PaymentStatuses.Confirmed;

        borrow.Status = BorrowStatuses.Paid;
        var toolIds = await _db.ProductBorrows
            .Where(pb => pb.BorrowId == borrow.Id)
            .Select(pb => pb.ToolId)
            .Distinct()
            .ToListAsync();

        foreach (var toolId in toolIds)
        {
            var deposit = new SecurityDeposit
            {
                Id = Guid.NewGuid(),
                ToolsId = toolId,
                UsersId = borrow.UsersId,
                Ammount = 50f,
                Status = DepositStatuses.Held,
                RefundDate = DateTime.UtcNow
            };
            _db.SecurityDeposits.Add(deposit);
        }

        await _db.SaveChangesAsync();

        var receiptNumber = MakeReceiptNumber(payment.Id, DateTime.UtcNow);
        return Ok(new PaymentConfirmResponseDto(payment.Id, payment.Status, payment.Date, receiptNumber));
    }

    [HttpGet("{paymentId:guid}/receipt")]
    public async Task<ActionResult<ReceiptDto>> GetReceipt([FromRoute] Guid paymentId)
    {
        var payment = await _db.Payments
            .Include(p => p.Borrow)
                .ThenInclude(b => b.User)
            .Include(p => p.Borrow)
                .ThenInclude(b => b.ProductBorrows)
                    .ThenInclude(pb => pb.Tool)
            .FirstOrDefaultAsync(p => p.Id == paymentId);

        if (payment is null) return NotFound("Payment not found");
        if (payment.Borrow is null) return Problem("Payment has no borrow linked (data integrity issue).");

        var borrow = payment.Borrow;
        var userId = CurrentUserId();
        if (!IsAdmin() && borrow.UsersId != userId)
            return Forbid();

        if (payment.Status != PaymentStatuses.Confirmed)
            return Conflict("Receipt is available only for confirmed payments.");

        var items = borrow.ProductBorrows
            .Select(pb =>
            {
                var toolName = pb.Tool?.Name ?? "Unknown tool";
                var qty = pb.Quantity;
                var unit = (decimal)(pb.Tool?.Price ?? 0f);
                return new ReceiptItemDto(toolName, qty, unit, unit * qty);
            })
            .ToList();

        var total = (decimal)borrow.Price;

        var receiptNumber = MakeReceiptNumber(payment.Id, payment.Date);

        var dto = new ReceiptDto(
            receiptNumber,
            payment.Date,
            payment.Id,
            borrow.Id,
            borrow.User?.Username ?? "Unknown",
            borrow.User?.Email ?? "Unknown",
            borrow.StartDate,
            borrow.EndDate,
            items,
            total
        );

        return Ok(dto);
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
