using System.Security.Cryptography;
using System.Text;
using ManageEase.Services.Interfaces;

namespace ManageEase.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        // Method to hash a password using a salt and the PBKDF2 algorithm with SHA-512
        public string HashPassword(string password, string salt)
        {
            // Generate a hashed password using PBKDF2 with the specified parameters:
            // - password: the input password to be hashed
            // - salt: the salt value to add randomness to the hashing process
            // - 400000: the number of iterations to perform (increases security by making brute-force attacks harder)
            // - HashAlgorithmName.SHA512: the hashing algorithm to use (SHA-512)
            // - 64: the size of the resulting hash in bytes
            return Convert.ToBase64String(Rfc2898DeriveBytes.Pbkdf2(
                password, 
                Encoding.Unicode.GetBytes(salt), 
                400000, 
                HashAlgorithmName.SHA512, 
                64));
        }
    }
}