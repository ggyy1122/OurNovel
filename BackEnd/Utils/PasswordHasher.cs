using System.Security.Cryptography;
using System.Text;

public static class PasswordHasher
{
    private static readonly byte[] Salt = Encoding.UTF8.GetBytes("FixedSalt123456");
    private const int Iterations = 10000;

    public static string HashPassword(string password)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(password, Salt, Iterations, HashAlgorithmName.SHA256);
        byte[] hashBytes = pbkdf2.GetBytes(15); // 15�ֽ� �� 20��base64�ַ�
        return Convert.ToBase64String(hashBytes).Substring(0, 20); // �ض�
    }

    public static bool VerifyPassword(string password, string hashed)
    {
        return HashPassword(password) == hashed;
    }
}
