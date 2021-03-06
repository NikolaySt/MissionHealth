using System;
using System.Text;

namespace SocialNet.Backend.Tokens.JsonWebTokens
{
    internal static class EncodingHelper
    {
        public static string ToBase64String(this byte[] input)
        {
            if (input == null) throw new ArgumentException("input");

            return Convert.ToBase64String(input).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }

        public static string ToBase64String(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) throw new ArgumentException("input");

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(input)).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }

        public static byte[] ToByteArray(this string input)
        {
            input = input.Replace('-', '+').Replace('_', '/');

            int pad = 4 - (input.Length % 4);
            pad = pad > 2 ? 0 : pad;

            input = input.PadRight(input.Length + pad, '=');

            return Convert.FromBase64String(input);
        }
    }
}
