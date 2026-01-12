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
[Route("api/tools/{toolId:guid}/reviews")]
public class ToolsReviewsController : ControllerBase
{
    private readonly ToolLendingContext _db;
    public ToolsReviewsController(ToolLendingContext db) => _db = db;

    private Guid CurrentUserId()
        => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost]
    public async Task<ActionResult<CreateReviewResponseDto>> Create(Guid toolId, [FromBody] CreateReviewRequestDto req)
    {
        if (req.Rating < 1 || req.Rating > 5)
            return BadRequest("Rating must be between 1 and 5.");

        var desc = (req.Description ?? "").Trim();
        if (string.IsNullOrWhiteSpace(desc))
            return BadRequest("Description is required.");

        var toolExists = await _db.Tools.AnyAsync(t => t.Id == toolId);
        if (!toolExists) return NotFound("Tool not found.");

        var userId = CurrentUserId();
        var today = DateTime.UtcNow.Date;

        var eligibleBorrow = await _db.Borrows
            .Include(b => b.ProductBorrows)
            .Include(b => b.Reviews)
            .Where(b => b.UsersId == userId)
            .Where(b => b.EndDate.Date <= today)
            .Where(b => b.Status == "Paid" || b.Status == "Confirmed")
            .Where(b => b.ProductBorrows.Any(pb => pb.ToolId == toolId))
            .Where(b => !b.Reviews.Any(r => r.ToolId == toolId && r.UserId == userId))
            .OrderByDescending(b => b.EndDate)
            .FirstOrDefaultAsync();

        if (eligibleBorrow is null)
            return Conflict("You can review only after a completed paid/confirmed booking for this tool.");

        var review = new Review
        {
            Id = Guid.NewGuid(),
            ToolId = toolId,
            UserId = userId,
            BorrowId = eligibleBorrow.Id,
            Rating = req.Rating,
            Description = desc,
            Date = DateTime.UtcNow,
            Status = "Approved"
        };

        _db.Reviews.Add(review);
        await _db.SaveChangesAsync();

        return Ok(new CreateReviewResponseDto(review.Id, review.Status));
    }
    
    [Authorize(Roles = "Admin")]
    [HttpDelete("{reviewId:guid}")]
    public async Task<IActionResult> Delete(Guid reviewId)
    {
        var review = await _db.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
        if (review is null) return NotFound();

        _db.Reviews.Remove(review);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
