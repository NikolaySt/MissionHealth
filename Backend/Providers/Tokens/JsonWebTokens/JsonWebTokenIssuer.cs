using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using SocialNet.Backend.Security;
using Security.Cryptography;

namespace SocialNet.Backend.Tokens.JsonWebTokens
{
    public class JsonWebTokenIssuer : JsonWebTokenProcessor, ITokenIssuer
    {
        private readonly JsonWebTokenIssuerSettings _settings;

        public JsonWebTokenIssuer(JsonWebTokenIssuerSettings settings)
        {
            _settings = settings;
        }

        public string GetToken(IEnumerable<Claim> claims)
        {
            var headerData = GetHeaderData();

            var claimsData = GetClaimsData(claims);

            var masterKey = GetMasterKeyData();

            var encryptedMasterKeyData = GetEncryptedMasterKeyData(masterKey);

            var initializationVektorData = GetInitializationVectorData();

            var authenticatedData = GetAuthenticatedData(
                headerData,
                encryptedMasterKeyData,
                initializationVektorData);

            byte[] tag;
            byte[] cipherText;

            using (var aes = new AuthenticatedAesCng())
            {
                aes.CngMode = CngChainingMode.Gcm;
                aes.Key = masterKey;
                aes.IV = initializationVektorData;
                aes.AuthenticatedData = authenticatedData;

                using (var memoryStream = new MemoryStream())
                {
                    using (var encryptor = aes.CreateAuthenticatedEncryptor())
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(claimsData, 0, claimsData.Length);

                            cryptoStream.FlushFinalBlock();

                            tag = encryptor.GetTag();
                            cipherText = memoryStream.ToArray();
                        }
                    }
                }
            }

            return string.Format("{0}.{1}.{2}.{3}.{4}",
                headerData.ToBase64String(),
                encryptedMasterKeyData.ToBase64String(),
                initializationVektorData.ToBase64String(),
                cipherText.ToBase64String(),
                tag.ToBase64String());
        }

        private string GetHeaderData()
        {
            var header = new Dictionary<string, object>
            {
                { TypeHeader, TypeHeaderValue },
                { EncryptionMethodHeader, EncryptionMethodValue }
            };

            return JsonConvert.SerializeObject(header);
        }

        private byte[] GetClaimsData(IEnumerable<Claim> claims)
        {
            var dictionary = new Dictionary<string, string>
            {
                { Claims.Issuer, _settings.Issuer },
                { Claims.Audience, _settings.Audience },
                { Claims.ExpirationTime, GetExpirationIntervalSeconds().ToString(CultureInfo.InvariantCulture) }
            };

            foreach (var claim in claims)
            {
                dictionary.Add(claim.Type, claim.Value);
            }

            var claimsJson = JsonConvert.SerializeObject(dictionary);

            return Encoding.UTF8.GetBytes(claimsJson);
        }

        private byte[] GetMasterKeyData()
        {
            var data = new byte[32];

            using (var provider = new RNGCryptoServiceProvider())
            {
                provider.GetBytes(data);
            }

            return data;
        }

        private byte[] GetEncryptedMasterKeyData(byte[] masterKey)
        {
            byte[] encryptedMasterKey;

            using (var provider = new RSACryptoServiceProvider())
            {
                provider.FromXmlString(_settings.AudiencePublicKey);
                encryptedMasterKey = provider.Encrypt(masterKey, true);
            }

            return encryptedMasterKey;
        }

        private byte[] GetInitializationVectorData()
        {
            var data = new byte[12];

            using (var provider = new RNGCryptoServiceProvider())
            {
                provider.GetBytes(data);
            }

            return data;
        }

        private long GetExpirationIntervalSeconds()
        {
            var interval = DateTime.UtcNow - EpochStart + _settings.LifeTime;
            return (long) interval.TotalSeconds;
        }
    }
}
