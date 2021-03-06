namespace SocialNet.Backend.Configuration
{
	public partial class SystemSettings
	{
		private static string _tokenIssuer;

		private static string _tokenAudience;

		private static string _tokenAudiencePublicKey;

		private static string _tokenAudiencePrivateKey;

		public static string TokenIssuer
		{
			get { return _tokenIssuer; }
		}

		public static string TokenAudience
		{
			get { return _tokenAudience; }
		}

		public static string TokenAudiencePublicKey
		{
			get { return _tokenAudiencePublicKey; }
		}

		public static string TokenAudiencePrivateKey
		{
			get { return _tokenAudiencePrivateKey; }
		}
	}
}
