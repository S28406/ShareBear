namespace Pro.Shared.Dtos;

public record AdminUserRowDto(
    Guid Id,
    string Username,
    string Email,
    string Role
);

public record UpdateUserRoleRequestDto(string Role);