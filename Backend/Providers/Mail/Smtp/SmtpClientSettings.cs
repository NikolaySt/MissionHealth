using System.Security.Authentication;

namespace SocialNet.Backend.Mail
{
    public class SmtpClientSettings
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public string Account { get; set; }

        public string Password { get; set; }

		public bool EnableSsl { get; set; }

		public SslProtocols SslProtocol { get; set; }

        public SmtpClientSettings()
        {
            SslProtocol = SslProtocols.Tls12;
        }
    }
}
