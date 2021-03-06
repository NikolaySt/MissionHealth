using SocialNet.Backend.Configuration;
using SocialNet.Backend.Tokens;
using SocialNet.Backend.Tokens.JsonWebTokens;

namespace SocialNet.Backend.Managers
{
    public static class TokenParserFactory
    {
        public static ITokenParser GetInstance()
        {
            var settings = new JsonWebTokenParserSettings
            {
                AudiencePrivateKey = SystemSettings.TokenAudiencePrivateKey
            };

            return new JsonWebTokenParser(settings);
        }
    }
}
