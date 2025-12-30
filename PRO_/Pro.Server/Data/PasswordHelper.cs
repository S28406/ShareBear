using System.Security.Cryptography;

namespace ToolRent.Security;

public static class PasswordHelper
{
    private const int SaltSize = 16;   // 128-bit
    private const int KeySize  = 32;   // 256-bit
    private const int Iterations = 100_000;

    public static (byte[] hash, byte[] salt) Hash(string password)
    {
        using var rng = RandomNumberGenerator.Create();
        var salt = new byte[SaltSize];
        rng.GetBytes(salt);

        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
        var hash = pbkdf2.GetBytes(KeySize);
        return (hash, salt);
    }

    public static bool Verify(string password, byte[] salt, byte[] expectedHash)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
        var hash = pbkdf2.GetBytes(KeySize);
        return CryptographicOperations.FixedTimeEquals(hash, expectedHash);
    }
}