using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRO.Data.Context;
using Pro.Shared.Dtos;

namespace Pro.Server.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly ToolLendingContext _db;
    public AdminController(ToolLendingContext db) => _db = db;

    [HttpGet("users")]
    public async Task<ActionResult<IReadOnlyList<AdminUserRowDto>>> GetUsers([FromQuery] string? search)
    {
        var q = _db.Users.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLowerInvariant();
            q = q.Where(u =>
                u.Username.ToLower().Contains(s) ||
                u.Email.ToLower().Contains(s));
        }

        var users = await q
            .OrderBy(u => u.Username)
            .Select(u => new AdminUserRowDto(u.Id, u.Username, u.Email, u.Role))
            .ToListAsync();

        return Ok(users);
    }

    [HttpPut("users/{id:guid}/role")]
    public async Task<IActionResult> UpdateRole(Guid id, [FromBody] UpdateUserRoleRequestDto req)
    {
        var roleRaw = (req.Role ?? "").Trim();

        bool ok =
            roleRaw.Equals("Customer", StringComparison.OrdinalIgnoreCase) ||
            roleRaw.Equals("Seller", StringComparison.OrdinalIgnoreCase) ||
            roleRaw.Equals("Admin", StringComparison.OrdinalIgnoreCase);

        if (!ok)
            return BadRequest("Invalid role. Allowed: Customer, Seller, Admin.");

        var role = char.ToUpperInvariant(roleRaw[0]) + roleRaw[1..].ToLowerInvariant();

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user is null) return NotFound("User not found");

        user.Role = role;
        await _db.SaveChangesAsync();

        return NoContent();
    }
}