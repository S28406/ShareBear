namespace Pro.Shared.Dtos;

public record UserDtos(
    Guid Id,
    string Username,
    string Email,
    string Role
);