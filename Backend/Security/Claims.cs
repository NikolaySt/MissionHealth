namespace SocialNet.Backend.Security
{
	public class Claims
	{
		// Those are reserved as per the JWT specs 
		public const string TokenId = "jti";
		public const string Issuer = "iss";
		public const string Audience = "aud";
		public const string ExpirationTime = "exp";

		public const string AppRealmId = "http://orionscloud.com/claims/appDomainId";
		public const string AppId = "http://orionscloud.com/claims/appId";
		public const string UserId = "http://orionscloud.com/claims/userId";
		public const string UserName = "http://orionscloud.com/claims/userName";
		public const string SessionId = "http://orionscloud.com/claims/SessionId";
		public const string CustomData = "http://orionscloud.com/claims/user/CustomData";
		public const string Role = "http://orionscloud.com/claims/user/role";
	}
}
