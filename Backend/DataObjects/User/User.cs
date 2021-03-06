

namespace SocialNet.Backend.DataObjects
{
    public class User : IdDataObject
    {
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Email { get; set; }

		public UserEmailStatus EmailStatus { get; set; }

        public bool Unsubscribe { get; set; }

		public long BookSent { get; set; }

		public long BookDownloads { get; set; }		
	}
}
