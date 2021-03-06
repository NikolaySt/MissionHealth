using System;
using System.Threading.Tasks;
using SocialNet.Backend.Mail;
using SocialNet.Backend.Configuration;

namespace SocialNet.Backend.Managers
{
    public static class MailClientFactory
    {        
        public static IMailClient GetConfrimEmailInstance()
        {
            var mailClientSettings = SystemSettings.ConfirmEmailFeature.MailAccountSettings;
            if (mailClientSettings == null) throw new ApplicationException("Email client not configured");

            var smtpSettings = new SmtpClientSettings
            {
                Host = mailClientSettings.Host,
                Port = mailClientSettings.Port,
                Account = mailClientSettings.Account,                   
				Password = mailClientSettings.Password,
				EnableSsl = mailClientSettings.Ssl,
			};
            return new MimeKitSmtpClient(smtpSettings);
        }

        public static IMailClient GetResetPasswordInstance()
		{
			var mailClientSettings = SystemSettings.ResetPasswordFeature.MailAccountSettings; 
			if (mailClientSettings == null) throw new ApplicationException("Email client not configured");

            var smtpSettings = new SmtpClientSettings
            {
                Host = mailClientSettings.Host,
                Port = mailClientSettings.Port,
                Account = mailClientSettings.Account,
                Password = mailClientSettings.Password,
                EnableSsl = mailClientSettings.Ssl,
            };
            return new MimeKitSmtpClient(smtpSettings);
        }

		public static IMailClient GetDownloadBookInstance()
		{
			var mailClientSettings = SystemSettings.DownloadBookFeature.MailAccountSettings;
			if (mailClientSettings == null) throw new ApplicationException("Email client not configured");

            var smtpSettings = new SmtpClientSettings
            {
                Host = mailClientSettings.Host,
                Port = mailClientSettings.Port,
                Account = mailClientSettings.Account,
                Password = mailClientSettings.Password,
                EnableSsl = mailClientSettings.Ssl,
            };

			//return new MimeKitSmtpClient(smtpSettings);
			return new SmtpClient(smtpSettings);
		}
	}
}
