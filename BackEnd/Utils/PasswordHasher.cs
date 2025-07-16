using System.Security.Cryptography;
using System.Text;

public static class PasswordHasher
{
    private static readonly byte[] Salt = Encoding.UTF8.GetBytes("FixedSalt123456");
    private const int Iterations = 10000;

    public static string HashPassword(string password)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(password, Salt, Iterations, HashAlgorithmName.SHA256);
        byte[] hashBytes = pbkdf2.GetBytes(15); // 15×Ö½Ú ¡Ö 20¸öbase64×Ö·û
        return Convert.ToBase64String(hashBytes).Substring(0, 20); // ½Ø¶Ï
    }

    public static bool VerifyPassword(string password, string hashed)
    {
        return HashPassword(password) == hashed;
    }
}
