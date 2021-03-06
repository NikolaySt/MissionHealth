using SocialNet.Backend.DataObjects;

namespace SocialNet.Backend.DataFilters
{
	public class UserListFilter : SuperListFilter
	{
		public string Email { get; set; }
		public UserEmailStatus? EmailStatus { get; set; }
		public long? BookDownloads { get; set; }
    }
}
