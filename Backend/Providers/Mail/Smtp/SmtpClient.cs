using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using SystemSmtpClient = System.Net.Mail.SmtpClient;

namespace SocialNet.Backend.Mail
{
    public class SmtpClient : IMailClient
    {
        private readonly SmtpClientSettings  _settings;

        public SmtpClient(SmtpClientSettings settings)
        {
            _settings = settings;
        }

        public async Task SendAsync(
            string from, 
            string to, 
            string subject, 
            string body, 
            bool isHtml = false,
            Encoding encoding = null)
        {
            if (_settings == null || string.IsNullOrWhiteSpace(_settings.Host) ||
                _settings.Port <= 0 ||
                string.IsNullOrWhiteSpace(_settings.Account) ||
                string.IsNullOrWhiteSpace(_settings.Password))
            {
                return;
            }

            await Task.Run(() =>
            {
                var message = new MailMessage(from, to)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = isHtml,
                    BodyEncoding = encoding ?? Encoding.UTF8
                };

				var client = new SystemSmtpClient
				{
					Host = _settings.Host,
					Port = _settings.Port,
					Credentials = new NetworkCredential(_settings.Account, _settings.Password),
					EnableSsl = _settings.EnableSsl,
                };

                client.Send(message);
            });
        }
    }
}
