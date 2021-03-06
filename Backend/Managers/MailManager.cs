using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using SocialNet.Backend.Configuration;
using SocialNet.Backend.Helpers;

namespace SocialNet.Backend.Managers
{
    public class MailManager
    {
        public async Task SendBookAsync(
            string email,
            string userName,
            string bookId,
            string userId,
            string bookName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("email");
            if (string.IsNullOrWhiteSpace(bookId)) throw new ArgumentException("bookId");
            if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentException("name");

            var builder = new StringBuilder(SystemSettings.DownloadBookFeature.MessageBody);

            builder.Replace("[NAME]", userName.Trim());

            builder.Replace("[BOOKID]", bookId);

            builder.Replace("[USERID]", userId);

            builder.Replace("[BOOKTITLE]", bookName);

            var message = builder.ToString();

            var shrink = new MemoryStream();
            
            var streamProcess = MinifyResponseHelper.Process(shrink);

            var buffer = Encoding.UTF8.GetBytes(message);

            streamProcess.Write(buffer, 0, buffer.Length);

            message = Encoding.UTF8.GetString(shrink.ToArray());

            var mailClient = MailClientFactory.GetDownloadBookInstance();

            await mailClient.SendAsync(
                SystemSettings.DownloadBookFeature.MessageFrom,                
                $"{userName}<{email}>",
                SystemSettings.DownloadBookFeature.MessageSubject + " - " + bookName,
                message,
                true,
                Encoding.UTF8);
        }
    }
}
