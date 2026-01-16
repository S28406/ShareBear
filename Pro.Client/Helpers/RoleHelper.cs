using Pro.Shared.Dtos;

namespace Pro.Client.Helpers;

public static class RoleHelper
{
    public const string Admin = "Admin";
    public const string Seller = "Seller";
    public const string Customer = "Customer";

    private static string? Norm(string? role) => role?.Trim();

    public static bool IsSellerOrAdmin(UserDtos? u)
    {
        var role = Norm(u?.Role);
        return role != null &&
               (role.Equals(Seller, StringComparison.OrdinalIgnoreCase) ||
                role.Equals(Admin, StringComparison.OrdinalIgnoreCase));
    }

}