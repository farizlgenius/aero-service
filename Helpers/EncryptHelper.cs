using System.Security.Cryptography;
using System.Text;

namespace HIDAeroService.Helpers
{
    public sealed class EncryptHelper
    {
        public static string Hash(string raw)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(raw));
            return Convert.ToHexString(bytes).ToLowerInvariant();
        }

        public static bool CompareHash(string rawInput, string storedHash)
        {
            var hashBytes1 = Convert.FromHexString(Hash(rawInput));
            var hashBytes2 = Convert.FromHexString(storedHash);

            return CryptographicOperations.FixedTimeEquals(hashBytes1, hashBytes2);
        }
        public static string HashPassword(string password)
        {
            // generate a random salt
            byte[] salt = RandomNumberGenerator.GetBytes(16);

            // derive a 32-byte hash with PBKDF2 (100,000 iterations recommended)
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);

            // combine salt + hash into one string (Base64)
            byte[] result = new byte[salt.Length + hash.Length];
            Buffer.BlockCopy(salt, 0, result, 0, salt.Length);
            Buffer.BlockCopy(hash, 0, result, salt.Length, hash.Length);

            return Convert.ToBase64String(result);
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            byte[] storedBytes = Convert.FromBase64String(storedHash);

            byte[] salt = new byte[16];
            Buffer.BlockCopy(storedBytes, 0, salt, 0, salt.Length);

            byte[] storedHashBytes = new byte[32];
            Buffer.BlockCopy(storedBytes, salt.Length, storedHashBytes, 0, 32);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
            byte[] computedHash = pbkdf2.GetBytes(32);

            // constant-time comparison to prevent timing attacks
            return CryptographicOperations.FixedTimeEquals(computedHash, storedHashBytes);
        }
    }
}
