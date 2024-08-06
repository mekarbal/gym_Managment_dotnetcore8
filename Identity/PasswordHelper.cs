using Microsoft.AspNetCore.Identity;

namespace GymManagment.Identity
{
    public class PasswordHelper
    {
        private readonly PasswordHasher<string> _passwordHasher;
        private static readonly char[] CharacterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_-+=<>?".ToCharArray();

        public PasswordHelper()
        {
            _passwordHasher = new PasswordHasher<string>();
        }

        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(null, password);
        }

        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var verificationResult = _passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
            return verificationResult == PasswordVerificationResult.Success;
        }

        public string GenerateRandomPassword()
        {
            int length = 20;
            var password = new char[length];
            var random = new Random();

            for (int i = 0; i < length; i++)
            {
                var index = random.Next(CharacterSet.Length);
                password[i] = CharacterSet[index];
            }

            return new string(password);
        }
    }
}
