using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRO.Data.Context;
using Pro.Shared.Dtos;

namespace Pro.Server.Controllers;

[ApiController]
[Route("api/tools")]
public class ToolsController : ControllerBase
{
    private readonly ToolLendingContext _db;

    public ToolsController(ToolLendingContext db) => _db = db;

    [HttpGet("filters")]
    public async Task<ActionResult<ToolFiltersDto>> GetFilters()
    {
        var categories = await _db.Categories
            .Select(c => c.Name)
            .Distinct()
            .OrderBy(x => x)
            .ToListAsync();

        var owners = await _db.Users
            .Select(u => u.Username)
            .Distinct()
            .OrderBy(x => x)
            .ToListAsync();

        return Ok(new ToolFiltersDto(categories, owners));
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ToolListItemDto>>> GetTools([FromQuery] string? category, [FromQuery] string? owner)
    {
        var q = _db.Tools
            .Include(t => t.Category)
            .Include(t => t.User)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(category))
            q = q.Where(t => t.Category.Name == category);

        if (!string.IsNullOrWhiteSpace(owner))
            q = q.Where(t => t.User.Username == owner);

        var list = await q
            .OrderBy(t => t.Name)
            .Select(t => new ToolListItemDto(
                t.ID,
                t.Name,
                t.Price,
                t.ImagePath,
                t.Category.Name,
                t.User.Username
            ))
            .ToListAsync();

        return Ok(list);
    }

    [HttpGet("{toolId:guid}")]
    public async Task<ActionResult<ToolDetailsDto>> GetTool(Guid toolId)
    {
        var tool = await _db.Tools
            .Include(t => t.Category)
            .Include(t => t.User)
            .Include(t => t.Reviews)
                .ThenInclude(r => r.User)
            .FirstOrDefaultAsync(t => t.ID == toolId);

        if (tool is null) return NotFound();

        var reviews = tool.Reviews
            .OrderByDescending(r => r.Date)
            .Select(r => new ReviewDto(
                r.ID,
                r.Rating,
                r.Description,
                r.Date,
                new UserDtos(r.User.ID, r.User.Username, r.User.Email, r.User.Role)
            ))
            .ToList();

        var dto = new ToolDetailsDto(
            tool.ID,
            tool.Name,
            tool.Description,
            tool.Price,
            tool.Quantity,
            tool.ImagePath,
            tool.Category.Name,
            tool.User.Username,
            reviews
        );

        return Ok(dto);
    }
}
