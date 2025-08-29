using PRO.Models;

namespace PRO.Security
{
    public static class Roles
    {
        public const string Admin    = "Admin";
        public const string Seller   = "Seller";
        public const string Customer = "Customer";

        public static bool IsSeller(User? u) =>
            u != null && (u.Role == Seller || u.Role == Admin);
    }
}