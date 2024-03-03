using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Domain
{
    public static class HashHelper
    {
        /// <summary>
        /// to hash str
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static string ToHashString(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("x2"));
            }
            return sb.ToString();
        }

        public static string ComputeSha256Hash(Stream stream)
        {
            using SHA256 hash = SHA256.Create();
            byte[] bytes = hash.ComputeHash(stream);
            return ToHashString(bytes);
        }

        public static string  ComputeSha256Hash(string input)
        {
            using SHA256 hash = SHA256.Create();
            byte[] bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            return ToHashString(bytes);
        }

        public static string ComputeMd5Hash(string input)
        {
            using MD5 md5Hash = MD5.Create();
            byte[] bytes = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            return ToHashString(bytes);
        }
    }
}
