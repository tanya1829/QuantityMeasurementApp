using System.Security.Cryptography;

namespace QuantityMeasurementApp.BusinessLayer.Services
{
    public static class PasswordHasher
    {
        private const int SaltSize    = 16;
        private const int HashSize    = 32;
        private const int Iterations  = 100_000;

        private static readonly HashAlgorithmName Algorithm =
            HashAlgorithmName.SHA256;

        /// <summary>
        /// Hash a plain-text password using PBKDF2 + random salt.
        /// Returns "iterations.saltBase64.hashBase64"
        /// </summary>
        public static string Hash(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password, salt, Iterations, Algorithm, HashSize);

            return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        /// <summary>
        /// Verify a plain-text password against a stored hash string.
        /// </summary>
        public static bool Verify(string password, string storedHash)
        {
            string[] parts = storedHash.Split('.');
            if (parts.Length != 3) return false;

            int    iterations = int.Parse(parts[0]);
            byte[] salt       = Convert.FromBase64String(parts[1]);
            byte[] hash       = Convert.FromBase64String(parts[2]);

            byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(
                password, salt, iterations, Algorithm, hash.Length);

            return CryptographicOperations.FixedTimeEquals(hash, inputHash);
        }
    }
}