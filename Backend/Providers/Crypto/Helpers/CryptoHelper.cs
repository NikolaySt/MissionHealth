using System;
using System.Security.Cryptography;
using System.Text;

namespace SocialNet.Backend.Providers.Crypto
{
    public class CryptoHelper
    {        
        public static string GenerateRandomKey(int lenght = 32)
        {
            var data = new byte[lenght];
            
            using (var provider = new RNGCryptoServiceProvider())
            {
                provider.GetNonZeroBytes(data);
            }

            return Convert.ToBase64String(data); 
        }

		public static string GenerateRandomAlphabetKey(int lenght = 32)
		{
			var data = new byte[lenght];

			using (var provider = new RNGCryptoServiceProvider())
			{
				provider.GetNonZeroBytes(data);
			}
			return Convert.ToBase64String(data).TrimEnd('=').Replace("+", "").Replace("/", "");
		}
	}
}


