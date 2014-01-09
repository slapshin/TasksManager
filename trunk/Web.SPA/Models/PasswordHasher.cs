using Common;
using Microsoft.AspNet.Identity;

namespace Web.SPA.Models
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            return Helpers.CreateMD5Hash(password);
        }

        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            return hashedPassword == HashPassword(providedPassword) ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }
    }
}