using System.Threading.Tasks;
using System.Web.Mvc;
using System.Collections.Generic;
using System;
using SocialNet.WebSite.Areas.Book.Models;
using SocialNet.Backend.Managers;
using SocialNet.Backend.Catalogs;
using SocialNet.Backend.DataObjects;
using SocialNet.Backend.DataFilters;
using System.Linq.Expressions;

namespace SocialNet.WebSite.Areas.Book.Controllers
{
    public class ReviewController : Controller
    {
        public ActionResult Index(string id)
        {
            var model = new BookRequestViewModel()
            {
                BookId = id
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Request(BookRequestViewModel model)
        {
            if (!TryValidateModel(model))
            {
                return View("Index", model);
            }

            var catalog = new UserCatalog(new Application());

            var filter = new UserItemFilter() {
                Email = model.Email.ToLower()
            };

            var user = await catalog.GetAsync(filter);
            if (user == null)
            {
                user = new User()
                {
                    Email = model.Email.ToLower(),
                    EmailStatus = UserEmailStatus.Pending,
                    FirstName = model.Name,
                };
                user = await catalog.StoreAsync(user);
            }

			Task.Factory.StartNew(async () =>
            {
                try
                {
					
					var manager = new MailManager();

                    await manager.SendBookAsync(model.Email, model.Name, model.BookId, user.Id, "Наръчник за балансиране на хормоните");

					var incValues = new Dictionary<Expression<Func<User, long>>, long> { { it => it.BookSent, 1 } };

					var userCatalog = new UserCatalog(new Application());

					await userCatalog.IncrementAsync(user.Id, incValues);
				}
                catch (Exception exception)
                {
                    var logger = LoggerFactory.GetInstance("WebSite");
                    await logger.LogErrorAsync(exception, "ReviewController", "Request", new Dictionary<string, object> { { "model", model } });
                }
            });

			ViewBag.Url = $"https://misiazdrave.bg/book/download/index?id={user.Id}&bookid={model.BookId}";

			return View();
        }
    }
}