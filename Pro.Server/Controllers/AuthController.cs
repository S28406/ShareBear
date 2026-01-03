using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRO.Data.Context;
using PRO.Models;
using Pro.Shared.Dtos;
using ToolRent.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.IdentityModel.Tokens;

namespace Pro.Server.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ToolLendingContext _db;

    public AuthController(ToolLendingContext db) => _db = db;
    private string CreateJwt(User user)
    {
        var jwt = HttpContext.RequestServices
            .GetRequiredService<IConfiguration>()
            .GetSection("Jwt");

        var issuer = jwt["Issuer"];
        var audience = jwt["Audience"];
        var key = jwt["Key"] ?? throw new InvalidOperationException("Jwt:Key missing");

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, user.Role),
        };

        var creds = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto req)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u =>
                u.Username == req.UsernameOrEmail || u.Email == req.UsernameOrEmail);

        if (user is null)
            return Unauthorized("Invalid credentials");

        if (!PasswordHelper.Verify(req.Password, user.PasswordSalt, user.PasswordHash))
            return Unauthorized("Invalid credentials");

        var dtoUser = new UserDtos(user.Id, user.Username, user.Email, user.Role);

        var token = CreateJwt(user);

        return Ok(new AuthResponseDto(token, dtoUser));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto req)
    {
        var exists = await _db.Users.AnyAsync(u => u.Username == req.Username || u.Email == req.Email);
        if (exists)
            return Conflict("Username or email already exists");

        var (hash, salt) = PasswordHelper.Hash(req.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = req.Username,
            Email = req.Email,
            PasswordHash = hash,
            PasswordSalt = salt,
            Role = "Customer"
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return NoContent();
    }
    
    
    // TODO: Remove this endpoint its only for testing 
    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        return Ok(new
        {
            userId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            username = User.Identity?.Name,
            role = User.FindFirstValue(ClaimTypes.Role)
        });
    }
}