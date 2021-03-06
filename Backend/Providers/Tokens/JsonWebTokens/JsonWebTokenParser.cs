using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using SocialNet.Backend.Security;
using Security.Cryptography;

namespace SocialNet.Backend.Tokens.JsonWebTokens
{
    public class JsonWebTokenParser : JsonWebTokenProcessor, ITokenParser
    {
        private readonly JsonWebTokenParserSettings _settings;

        public JsonWebTokenParser(JsonWebTokenParserSettings settings)
        {
            if (settings == null) throw new ArgumentException("settings");

            _settings = settings;
        }

        public IEnumerable<Claim> GetClaims(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) throw new ArgumentException("token");

            var values = token.Split('.');

            if (values.Length != 5) throw new SecurityException("Invalid token");

            if (string.IsNullOrWhiteSpace(values[0])) throw new SecurityException("Missing token header");
            var headerData = Encoding.UTF8.GetString(values[0].ToByteArray());

            if (string.IsNullOrWhiteSpace(values[1])) throw new SecurityException("Missing token master key");
            var encryptedMasterKeyData = values[1].ToByteArray();
            var masterKeyData = GetDecryptedMasterKey(encryptedMasterKeyData);

            if(string.IsNullOrWhiteSpace(values[2])) throw new SecurityException("Missing token initialization vektor");
            var initializationVektorData = values[2].ToByteArray();

            if(string.IsNullOrWhiteSpace(values[3])) throw new SecurityException("Missing token claims");
            var encryptedClaimsData = values[3].ToByteArray();

            if(string.IsNullOrWhiteSpace(values[4])) throw new SecurityException("Missing token tag");
            var tagData = values[4].ToByteArray();

            byte[] additionalAuthenticatedData = GetAuthenticatedData(
                headerData,
                encryptedMasterKeyData,
                initializationVektorData);

            byte[] claimsData;
            using (var aes = new AuthenticatedAesCng())
            {
                aes.CngMode = CngChainingMode.Gcm;
                aes.Key = masterKeyData;
                aes.IV = initializationVektorData;
                aes.AuthenticatedData = additionalAuthenticatedData;
                aes.Tag = tagData;

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(encryptedClaimsData, 0, encryptedClaimsData.Length);
                        cryptoStream.FlushFinalBlock();
                        claimsData = memoryStream.ToArray();
                    }
                }
            }

            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Encoding.UTF8.GetString(claimsData));

            var claims = dictionary.Select(item => new Claim(item.Key, item.Value)).ToList();

            return claims;
        }

        private byte[] GetDecryptedMasterKey(byte[] encryptedMasterKey)
        {
            byte[] masterKey;

            using (var provider = new RSACryptoServiceProvider())
            {
                provider.FromXmlString(_settings.AudiencePrivateKey);
                masterKey = provider.Decrypt(encryptedMasterKey, true);
            }

            return masterKey;
        }
    }
}


