using System;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    public static class Helpers
    {
        public static string CreateMD5Hash(string source)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(source));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    builder.Append(data[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static bool VerifyMD5Hash(string input, string hash)
        {
            return StringComparer.OrdinalIgnoreCase.Compare(Helpers.CreateMD5Hash(input), hash) == 0;
        }
    }
}