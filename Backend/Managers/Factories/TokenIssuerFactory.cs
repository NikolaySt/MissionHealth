using System;
using System.Threading.Tasks;

using SocialNet.Backend.Configuration;
using SocialNet.Backend.Tokens;
using SocialNet.Backend.Tokens.JsonWebTokens;

namespace SocialNet.Backend.Managers
{
	public static class TokenIssuerFactory
	{
		public static async Task<ITokenIssuer> GetInstance()
		{
			var settings = new JsonWebTokenIssuerSettings
			{
				Issuer = SystemSettings.TokenIssuer,
				LifeTime = TimeSpan.FromSeconds(SystemSettings.TokenLifeTime),
				Audience = SystemSettings.TokenAudience,
				AudiencePublicKey = SystemSettings.TokenAudiencePublicKey
			};

			return new JsonWebTokenIssuer(settings);
		}

		public static async Task<ITokenIssuer> GetInstance(TimeSpan lifeTime)
		{
			var settings = new JsonWebTokenIssuerSettings
			{
				Issuer = SystemSettings.TokenIssuer,
				LifeTime = lifeTime,
				Audience = SystemSettings.TokenAudience,
				AudiencePublicKey = SystemSettings.TokenAudiencePublicKey
			};

			return new JsonWebTokenIssuer(settings);
		}
	}
}
