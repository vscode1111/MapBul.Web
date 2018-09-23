using System;
using System.Security.Cryptography;
using System.Text;

namespace MapBul.SharedClasses
{
    public static class StringTransformationProvider
    {
        public static string Md5(string input)
        {
            var md5 = MD5.Create();
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hash = md5.ComputeHash(inputBytes);
            var sBuilder = new StringBuilder();
            foreach (var t in hash)
            {
                sBuilder.Append(t.ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public static string TransformEmail(string input)
        {
            return input.Trim().ToLower();
        }

        public static string GeneratePassword()
        {
            var key = "";
            var r = new Random(Guid.NewGuid().GetHashCode());
            while (key.Length < 7)
            {
                var c = (char)r.Next(33, 125);
                if (Char.IsLetterOrDigit(c))
                    key += c;
            }
            return key;
        }
    }
}
