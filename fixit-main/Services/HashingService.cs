using fixit_main.Services.Templates;
using System.Security.Cryptography;
using System.Text;

namespace fixit_main.Services
{
    public class HashingService : IHashingService
    {
        private const int KEY_SIZE = 64;
        private const int ITERATIONS = 350000;

        public string HashPassword(string password, out string salt)
        {
            byte[] saltBytes = RandomNumberGenerator.GetBytes(KEY_SIZE);
            salt = Convert.ToHexString(saltBytes);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), saltBytes, ITERATIONS, HashAlgorithmName.SHA512, KEY_SIZE);
            return Convert.ToHexString(hash);
        }

        public string HashPasswordWithSalt(string password, string salt)
        {
            byte[] saltBytes = Convert.FromHexString(salt);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), saltBytes, ITERATIONS, HashAlgorithmName.SHA512, KEY_SIZE);
            return Convert.ToHexString(hash);
        }
    }
}
