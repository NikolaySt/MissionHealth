using MimeKit;
using MimeKit.Text;
using System.Text;
using System.Threading.Tasks;
using System;

namespace SocialNet.Backend.Mail
{
    public class MimeKitSmtpClient : IMailClient
    {
        private readonly SmtpClientSettings _settings;

        public MimeKitSmtpClient(SmtpClientSettings settings)
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
                var message = new MimeMessage();

                var mail = new System.Net.Mail.MailAddress(from);
                message.From.Add(new MailboxAddress(mail.DisplayName, mail.Address));

                mail = new System.Net.Mail.MailAddress(to);
                message.To.Add(new MailboxAddress(mail.DisplayName, mail.Address));
                message.Subject = subject;

                if (isHtml)
                {
                    message.Body = new TextPart(TextFormat.Html)
                    {
                        Text = body,
                    };
                }
                else
                {
                    message.Body = new TextPart(TextFormat.Plain)
                    {
                        Text = body,
                    };
                }

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    if (_settings.EnableSsl)
                    {
                        client.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                    }

                    client.Connect(_settings.Host, _settings.Port, _settings.EnableSsl);

                    client.AuthenticationMechanisms.Remove("XOAUTH2");

                    client.Authenticate(_settings.Account, _settings.Password);

                    client.Send(message);

                    client.Disconnect(true);
                }
            });
        }
    }
}
