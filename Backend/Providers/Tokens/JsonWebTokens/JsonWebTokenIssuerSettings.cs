using System;

namespace SocialNet.Backend.Tokens.JsonWebTokens
{
    public class JsonWebTokenIssuerSettings
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public TimeSpan LifeTime { get; set; }

        public string AudiencePublicKey { get; set; }
    }
}
