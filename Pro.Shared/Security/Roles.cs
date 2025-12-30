using Pro.Shared.Dtos;

namespace Pro.Shared.Security;

public static class Roles
{
    public const string Admin = "Admin";
    public const string Customer = "Customer";
    public const string Seller = "Seller";

    public static bool IsSeller(UserDtos? user)
        => string.Equals(user?.Role, Seller, StringComparison.OrdinalIgnoreCase)
           || IsAdmin(user);

    public static bool IsAdmin(UserDtos? user)
        => string.Equals(user?.Role, Admin, StringComparison.OrdinalIgnoreCase);
}