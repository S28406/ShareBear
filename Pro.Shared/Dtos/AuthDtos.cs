namespace Pro.Shared.Dtos;

public record LoginRequestDto(string UsernameOrEmail, string Password);
public record RegisterRequestDto(string Username, string Email, string Password, string? Role);

public record AuthResponseDto(string Token, UserDtos User);