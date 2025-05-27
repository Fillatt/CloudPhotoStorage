using System.Text;

namespace CloudPhotoStorage.API.Services;

public static class PasswordHasher
{
    /// <summary>
    /// Хеширование пароля
    /// </summary>
    /// <param name="password"></param>
    /// <param name="passwordHash"></param>
    /// <param name="passwordSalt"></param>
    public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    /// <summary>
    /// Проверка пароля
    /// </summary>
    /// <param name="password"></param>
    /// <param name="storedHash"></param>
    /// <param name="storedSalt"></param>
    /// <returns></returns>
    public static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            var result = computedHash.SequenceEqual(storedHash);

            return result;
        }
    }
}
