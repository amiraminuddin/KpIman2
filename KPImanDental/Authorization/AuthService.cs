using KPImanDental.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace KPImanDental.Authorization
{
    public class AuthService
    {
        public PasswordModel GetPasswordHasher(string password)
        {
            var hmac = new HMACSHA512();
            var passwordDetail = new PasswordModel
            {
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                PasswordSalt = hmac.Key
            };
            return passwordDetail;
        }

        public bool LoginAuthorization(KpImanUser user, string loginUserPassword)
        {
            bool result = true;
            var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginUserPassword));

            // Check if the lengths match before proceeding
            if (computedHash.Length != user.PasswordHash.Length)
            {
                return false; // Lengths are different, hashes do not match
            }

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                {
                    result = false;
                    break; //exit the loop
                }
            }
            return result;
        }

    }

    public class PasswordModel
    {
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
    }

}
