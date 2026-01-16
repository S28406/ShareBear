using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRO.Data.Context;
using Pro.Shared.Dtos;

namespace Pro.Server.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly ToolLendingContext _db;
    public CategoriesController(ToolLendingContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CategoryDto>>> Get()
    {
        var list = await _db.Categories
            .OrderBy(c => c.Name)
            .Select(c => new CategoryDto(c.Id, c.Name))
            .ToListAsync();

        return Ok(list);
    }
}