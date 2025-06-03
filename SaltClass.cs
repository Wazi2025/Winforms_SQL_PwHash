using System;
using System.Security.Cryptography;
using System.Text;

public static class SaltClass
{
    // 🍟 Generate a new salt
    public static string GenerateSalt(int size = 32)
    {
        byte[] saltBytes = new byte[size];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }

        return Convert.ToBase64String(saltBytes);
    }

    // 🧂 Hash a password WITH salt
    public static string HashWithSalt(string password, string salt)
    {
        string saltedPassword = salt + password; // Or password + salt – just be consistent
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(saltedPassword);
            byte[] hashBytes = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}