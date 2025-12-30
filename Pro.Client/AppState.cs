using Pro.Shared.Dtos;

namespace Pro.Client;

public static class AppState
{
    public static UserDto? CurrentUser { get; set; }
    public static string? Token { get; set; }
}