using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRO.Data.Context;
using Pro.Shared.Dtos;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Pro.Server.Controllers;

[ApiController]
[Route("api/tools")]
public class ToolsController : ControllerBase
{
    private readonly ToolLendingContext _db;

    public ToolsController(ToolLendingContext db) => _db = db;
    
    private static string Img(string? fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return "/images/placeholder.jpg";

        fileName = Path.GetFileName(fileName);
        return $"/images/{fileName}";
    }
    
    private Guid CurrentUserId()
        => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    private bool IsAdmin()
        => User.IsInRole("Admin");
    
    private static ToolDetailsDto ToDetailsDto(PRO.Models.Tool tool)
    {
        var reviews = tool.Reviews
            .OrderByDescending(r => r.Date)
            .Select(r => new ReviewDto(
                r.Id,
                r.Rating,
                r.Description,
                r.Date,
                new UserDtos(r.User.Id, r.User.Username, r.User.Email, r.User.Role)
            ))
            .ToList();

        return new ToolDetailsDto(
            tool.Id,
            tool.Name,
            tool.Description,
            tool.Price,
            tool.Quantity,
            Img(tool.ImagePath),
            tool.Category.Name,
            tool.User.Username,
            reviews
        );
    }

    private static string NormalizeImage(string? fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return "placeholder.jpg";

        return Path.GetFileName(fileName);
    }

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
                t.Id,
                t.Name,
                t.Price,
                Img(t.ImagePath),
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
            .FirstOrDefaultAsync(t => t.Id == toolId);

        if (tool is null) return NotFound();

        var reviews = tool.Reviews
            .OrderByDescending(r => r.Date)
            .Select(r => new ReviewDto(
                r.Id,
                r.Rating,
                r.Description,
                r.Date,
                new UserDtos(r.User.Id, r.User.Username, r.User.Email, r.User.Role)
            ))
            .ToList();

        var dto = new ToolDetailsDto(
            tool.Id,
            tool.Name,
            tool.Description,
            tool.Price,
            tool.Quantity,
            Img(tool.ImagePath),
            tool.Category.Name,
            tool.User.Username,
            reviews
        );

        return Ok(dto);
    }
    
    [Authorize(Roles = "Seller,Admin")]
    [HttpPost]
    public async Task<ActionResult<ToolDetailsDto>> CreateTool([FromBody] CreateToolRequestDto req)
    {
        if (string.IsNullOrWhiteSpace(req.Name))
            return BadRequest("Name is required.");

        if (req.Price < 0) return BadRequest("Price must be >= 0.");
        if (req.Quantity < 0) return BadRequest("Quantity must be >= 0.");

        var categoryExists = await _db.Categories.AnyAsync(c => c.Id == req.CategoryId);
        if (!categoryExists)
            return BadRequest("CategoryId not found.");

        var userId = CurrentUserId();

        var tool = new PRO.Models.Tool
        {
            Id = Guid.NewGuid(),
            Name = req.Name.Trim(),
            Description = req.Description?.Trim() ?? "",
            Price = req.Price,
            Quantity = req.Quantity,
            CategoryId = req.CategoryId,
            UsersId = userId,
            ImagePath = NormalizeImage(req.ImageFileName)
        };

        _db.Tools.Add(tool);
        await _db.SaveChangesAsync();
        
        var created = await _db.Tools
            .Include(t => t.Category)
            .Include(t => t.User)
            .Include(t => t.Reviews).ThenInclude(r => r.User)
            .FirstAsync(t => t.Id == tool.Id);

        // return CreatedAtAction(nameof(GetTool), new { toolId = tool.Id }, await GetTool(tool.Id));
        return CreatedAtAction(nameof(GetTool), new { toolId = tool.Id }, ToDetailsDto(created));
    }
    
    [Authorize(Roles = "Seller,Admin")]
    [HttpPut("{toolId:guid}")]
    public async Task<IActionResult> UpdateTool(Guid toolId, [FromBody] UpdateToolRequestDto req)
    {
        var tool = await _db.Tools.FirstOrDefaultAsync(t => t.Id == toolId);
        if (tool is null) return NotFound();

        var userId = CurrentUserId();

        if (!IsAdmin() && tool.UsersId != userId)
            return Forbid();

        var categoryExists = await _db.Categories.AnyAsync(c => c.Id == req.CategoryId);
        if (!categoryExists)
            return BadRequest("CategoryId not found.");

        tool.Name = req.Name.Trim();
        tool.Description = req.Description?.Trim() ?? "";
        tool.Price = req.Price;
        tool.Quantity = req.Quantity;
        tool.CategoryId = req.CategoryId;
        tool.ImagePath = NormalizeImage(req.ImageFileName);

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [Authorize(Roles = "Seller,Admin")]
    [HttpGet("mine")]
    public async Task<ActionResult<IReadOnlyList<ToolListItemDto>>> GetMyTools()
    {
        var userId = CurrentUserId();

        var list = await _db.Tools
            .Include(t => t.Category)
            .Include(t => t.User)
            .Where(t => t.UsersId == userId)
            .OrderBy(t => t.Name)
            .Select(t => new ToolListItemDto(
                t.Id,
                t.Name,
                t.Price,
                Img(t.ImagePath),
                t.Category.Name,
                t.User.Username
            ))
            .ToListAsync();

        return Ok(list);
    }

}
