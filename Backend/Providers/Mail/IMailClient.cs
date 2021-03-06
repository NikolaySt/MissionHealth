using System.Text;
using System.Threading.Tasks;

namespace SocialNet.Backend.Mail
{
    public interface IMailClient
    {
        Task SendAsync(
            string from,
            string to,
            string subject,
            string body,
            bool isHtml = false,
            Encoding encoding = null);
    }
}
