using System;
using System.Security.Cryptography;
using System.Text;

namespace AISOS.Utils
{
    public static class PasswordHasher
    {
        private static MD5 md5;

        public static string Hash(string password)
        {
            byte[] hash = (md5 ?? (md5 = MD5.Create())).ComputeHash(Encoding.ASCII.GetBytes(password));
            password = Convert.ToBase64String(hash);
            return password;
        }
    }
}
