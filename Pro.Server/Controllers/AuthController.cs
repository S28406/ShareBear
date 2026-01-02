using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRO.Data.Context;
using PRO.Models;
using Pro.Shared.Dtos;
using ToolRent.Security;

namespace Pro.Server.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ToolLendingContext _db;

    public AuthController(ToolLendingContext db) => _db = db;

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

        // DEV TOKEN (replace with JWT later)
        var token = user.Id.ToString();

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
}