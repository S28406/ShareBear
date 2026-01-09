using System.IdentityModel.Tokens.Jwt;

namespace Pro.Client.Helpers;

public static class JwtHelper
{
    public static string? TryGetRole(string? jwt)
    {
        if (string.IsNullOrWhiteSpace(jwt)) return null;

        var token = new JwtSecurityTokenHandler().ReadJwtToken(jwt);

        // ASP.NET can emit either "role" or the long ClaimTypes.Role URI
        var role =
            token.Claims.FirstOrDefault(c => c.Type == "role")?.Value
            ?? token.Claims.FirstOrDefault(c => c.Type.EndsWith("/role"))?.Value;

        return role?.Trim();
    }
}