using System;
using System.Text;

namespace SocialNet.Backend.Tokens.JsonWebTokens
{
    public abstract class JsonWebTokenProcessor
    {
        protected const string TypeHeader = "typ";
        protected const string TypeHeaderValue = "JWT";
        protected const string EncryptionMethodHeader = "enc";
        protected const string EncryptionMethodValue = "RSA_OAEP";

        protected static readonly DateTime EpochStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public static byte[] GetAuthenticatedData(
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
    }
}
