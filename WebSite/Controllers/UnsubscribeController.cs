using SocialNet.Backend.Catalogs;
using SocialNet.Backend.DataObjects;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SocialNet.WebSite.Controllers
{
    public class UnsubscribeController : SuperController
    {
        public async Task<ActionResult> Index(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                var catalog = new UserCatalog(new Application());

                var user = await catalog.GetAsync(id);

                return View(user);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(string id, FormCollection model)
        {
            var catalog = new UserCatalog(new Application());

            User user = null;

            if (!string.IsNullOrWhiteSpace(id))
            {
                user = await catalog.GetAsync(id);
            }

            if (user == null)
            {
                ViewBag.Success = false;

                ViewBag.Message = "Възникна грешка, моля свържете с мен чрез формата за контакти.";

                return View();
            }

            await catalog.UpdateAsync(user.Id, new Dictionary<string, object> { { "Unsubscribe", true } });

            ViewBag.Success = true;

            ViewBag.Message = "Успешно бяхте отписaн от списъка за получаване на известия.";

            return View(user);
        }
    }
}