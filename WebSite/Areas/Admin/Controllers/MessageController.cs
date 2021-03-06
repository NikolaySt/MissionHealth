using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

using SocialNet.Backend.Catalogs;
using SocialNet.Backend.DataFilters;

namespace SocialNet.WebSite.Areas.Admin.Controllers
{

    public class MessageController : AdminSuperController
    {
		public async Task<ActionResult> Index(int page = 0)
        {
            var messageCatalog = new MessageCatalog();

            var result = await messageCatalog.GetPageAsync(new MessageListFilter(), page, 20, "Created DESC");

			var messages = await messageCatalog.GetCountAsync(new MessageListFilter() { Seen = false });

			ViewBag.Message = messages;

			if (Request.IsAjaxRequest())
			{
				return PartialView("_messages", result.Items);
			}

			return View(result.Items);
        }

        [HttpGet]
        public async Task<ActionResult> Read(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) RedirectToAction("Index");

			var messageCatalog = new MessageCatalog();

			var message = await messageCatalog.GetAsync(id);

			message.Seen = true;

			await messageCatalog.StoreAsync(message);

			if (Request.IsAjaxRequest())
			{
				return PartialView("_message", message);
			}

			return new JsonResult() { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
		}

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) RedirectToAction("Index");

			var catalog = new MessageCatalog();

			await catalog.DeleteAsync(id);

            return new HttpStatusCodeResult(HttpStatusCode.OK, "Съобщението е изтрита успешно");
        }
    }
}