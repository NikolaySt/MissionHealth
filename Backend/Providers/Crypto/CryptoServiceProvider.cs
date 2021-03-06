using System;
using System.Text;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Collections.Generic;
using Newtonsoft.Json;
using Security.Cryptography;

namespace SocialNet.Backend.Providers.Crypto
{
    public class CryptoServiceProvider : IDecryption, IEncryption
    {
        protected const string TypeHeader = "typ";
        protected const string TypeHeaderValue = "CSP"; //Crypto Service Provider
        protected const string EncryptionMethodHeader = "enc";
        protected const string EncryptionMethodValue = "RSA_OAEP";

        protected DecryptionSettings DecryptionSettings;
        protected EncryptionSettings EncryptionSettings;

        public byte[] GetAuthenticatedData(
                  string header,
                  byte[] encryptedMasterKey,
                  byte[] initializationVector)
        {
            var temp = string.Format("{0}.{1}.{2}",
                header.ToBase64String(),
                encryptedMasterKey.ToBase64String(),
                initializationVector.ToBase64String());

            return Encoding.UTF8.GetBytes(temp);
        }

        public static IDecryption Create(DecryptionSettings settings)
        {
            if (settings == null) throw new ArgumentException("settings");

            var provider = new CryptoServiceProvider();

            provider.DecryptionSettings = settings;

            return provider as IDecryption;
        }

        public static IEncryption Create(EncryptionSettings settings)
        {
            if (settings == null) throw new ArgumentException("settings");

            var provider = new CryptoServiceProvider();

            provider.EncryptionSettings = settings;

            return provider as IEncryption;
        }

        public string Encrypt(string data)
        {
            var headerData = GetHeaderData();

            var encryptionData = Encoding.UTF8.GetBytes(data);

            var masterKey = GenerateMasterKeyData();

            var encryptedMasterKeyData = GetEncryptedMasterKeyData(masterKey);

            var initializationVektorData = GenerateVectorData();

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
                            cryptoStream.Write(encryptionData, 0, encryptionData.Length);

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

        public string Decrypt(string data)
        {
            if (string.IsNullOrWhiteSpace(data)) throw new ArgumentException("data");

            var values = data.Split('.');

            if (values.Length != 5) throw new SecurityException("Invalid data");

            if (string.IsNullOrWhiteSpace(values[0])) throw new SecurityException("Missing data header");
            var headerData = Encoding.UTF8.GetString(values[0].ToByteArray());

            if (string.IsNullOrWhiteSpace(values[1])) throw new SecurityException("Missing data master key");
            var encryptedMasterKeyData = values[1].ToByteArray();
            var masterKeyData = GetDecryptedMasterKey(encryptedMasterKeyData);

            if (string.IsNullOrWhiteSpace(values[2])) throw new SecurityException("Missing data initialization vektor");
            var initializationVektorData = values[2].ToByteArray();

            if (string.IsNullOrWhiteSpace(values[3])) throw new SecurityException("Missing data claims");
            var encryptedClaimsData = values[3].ToByteArray();

            if (string.IsNullOrWhiteSpace(values[4])) throw new SecurityException("Missing data tag");
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

            return Encoding.UTF8.GetString(claimsData);
        }

        protected CryptoServiceProvider() { }

        private byte[] GetDecryptedMasterKey(byte[] encryptedMasterKey)
        {
            byte[] masterKey;

            using (var provider = new RSACryptoServiceProvider())
            {
                provider.FromXmlString(DecryptionSettings.PrivateKey);
                masterKey = provider.Decrypt(encryptedMasterKey, true);
            }

            return masterKey;
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

        private byte[] GetEncryptedMasterKeyData(byte[] masterKey)
        {
            byte[] encryptedMasterKey;

            using (var provider = new RSACryptoServiceProvider())
            {
                provider.FromXmlString(EncryptionSettings.PublicKey);
                encryptedMasterKey = provider.Encrypt(masterKey, true);
            }

            return encryptedMasterKey;
        }

        private byte[] GenerateMasterKeyData()
        {
            var data = new byte[32];

            using (var provider = new RNGCryptoServiceProvider())
            {
                provider.GetBytes(data);
            }

            return data;
        }

        private byte[] GenerateVectorData()
        {
            var data = new byte[12];

            using (var provider = new RNGCryptoServiceProvider())
            {
                provider.GetBytes(data);
            }

            return data;
        }
    }
}


