using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Sny.Kernel.Extensions
{
    public static class Hashing
    {
        public static string HashPassword(string password, string salt)
        {
            var md5 = new HMACMD5(Encoding.UTF8.GetBytes(salt + password));
            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            return System.Convert.ToBase64String(data);
        }
    }
}
