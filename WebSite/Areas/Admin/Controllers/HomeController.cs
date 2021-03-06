using SocialNet.Backend.Catalogs;
using SocialNet.Backend.DataFilters;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SocialNet.WebSite.Areas.Admin.Controllers
{
    
    public class HomeController : AdminSuperController
	{
        // GET: home/index
        public async Task<ActionResult> Index()
        {
			var messageCatalog = new MessageCatalog();

			var messages = await messageCatalog.GetCountAsync(new MessageListFilter() { Seen = false });

			ViewBag.Message = messages;

			return View();
        }
    }
}