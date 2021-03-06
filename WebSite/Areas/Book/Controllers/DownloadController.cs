using SocialNet.Backend.Catalogs;
using SocialNet.Backend.DataObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SocialNet.WebSite.Areas.Book.Controllers
{
    public class DownloadController : Controller
    {
        public async Task<FileResult> Index(string id, string bookId)
        {
            var folder = AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "\\books";

            var files = Directory.GetFiles(folder, $"{bookId}*.pdf");

            var file = files.FirstOrDefault();

            var catalog = new UserCatalog(new Application());

            var user = await catalog.GetAsync(id);

            if (file == null || user == null)
            {
                using (var memory = new MemoryStream())
                {
                    return new FileStreamResult(memory, "application/pdf");
                }
            }
            
            var incValues = new Dictionary<Expression<Func<User, long>>, long> { { it => it.BookDownloads, 1 } };

            catalog.IncrementAsync(id, incValues);

            catalog.UpdateAsync(id, new Dictionary<string, object> { { "EmailStatus", UserEmailStatus.Confirmed} });

            var url = Url.Content(file);

            var fileName = Path.GetFileName(file).Split('_')[1];

            var fileStream = new FileStreamResult(new FileStream(file, FileMode.Open), "application/pdf");

            fileStream.FileDownloadName = fileName;

            return fileStream;
        }
    }
}