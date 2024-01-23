using System.Security.Cryptography;
using System.Text;

namespace MyWebApp.Helpers
{
    public class PasswordHelper
    {
        private static PasswordHelper _single = null;
        
        private PasswordHelper() { }

        public static PasswordHelper Initialize()
        {
            if (_single == null)
                _single = new PasswordHelper();

            return _single;
        }
        
        public string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                string hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                return hash;
            }
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                string hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                return hash == hashedPassword;
            }
        }
    }
}
