
namespace SocialNet.Backend.DataObjects
{
    public class MailAccountSettings : DataObject
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public string Account { get; set; }

        public string Password { get; set; }

		public bool Ssl { get; set; }
    }
}
