using SocialNet.Backend.DataObjects;

namespace SocialNet.Backend.DataFilters
{
	public class UserCredentialItemFilter : SuperFilter
	{
        public string UserId { get; set; }
		public UserCredentialType? Type { get; set; }
		public UserCredentialStatus? Status { get; set; }
	}
}
