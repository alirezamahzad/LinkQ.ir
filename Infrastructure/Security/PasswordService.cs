using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Security
{
    public static class PasswordService
    {
        public static string HashPasswordWithSalt(string password, string salt)
        {
            var hashedPassword = Hash(password);
            var hashedSalt = Hash(salt);
            return string.Concat(hashedPassword, hashedSalt);
        }

        public static string HashPasswordWithoutSalt(string password)
        {
            return Hash(password);
        }

        public static bool VerifyPasswordWithSalt(string password, string salt, string hashedPassword)
        {
            var hashedInput = HashPasswordWithSalt(password, salt);
            return hashedInput == hashedPassword;
        }

        public static bool VerifyPasswordWithoutSalt(string password, string hashedPassword)
        {
            var hashedInput = HashPasswordWithoutSalt(password);
            return hashedInput == hashedPassword;
        }

        private static string Hash(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }

}
