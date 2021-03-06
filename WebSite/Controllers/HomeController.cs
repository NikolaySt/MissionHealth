using System.Net;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Web.Mvc;
using System.Threading.Tasks;

using SocialNet.Backend.Catalogs;
using SocialNet.Backend.DataObjects;
using SocialNet.Backend.DataFilters;

namespace SocialNet.WebSite.Controllers
{
    public class HomeController : SuperController
    {
        public async Task<ActionResult> Index()
        {
            var catalog = new ArticleCatalog(new Application());

            var filter = new ArticleListFilter()
            {
                Published = true,
                Top = false,
                Interesting = false,
            };

            var result = await catalog.GetPageAsync(filter, 0, 5, "Created DESC");

            return View(result.Items);
        }

        public async Task<ActionResult> Interesting()
        {
            var catalog = new ArticleCatalog(new Application());

            var filter = new ArticleListFilter()
            {
                Published = true,
                Top = false,
                Interesting = true
            };

            var result = await catalog.GetPageAsync(filter, 0, 5, "Created ASC");

            return PartialView("_articlesInteresting", result.Items);
        }

        public async Task<ActionResult> Articles(int page = 0)
        {
            var catalog = new ArticleCatalog(new Application());

            var filter = new ArticleListFilter()
            {
                Published = true,
                Top = false,
                Interesting = false
            };

            var result = await catalog.GetPageAsync(filter, page, 5, "Created DESC");

            return PartialView("_articlesFeed", result.Items);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult TermsAndPolicy()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Comment(string id)
        {
            var catalog = new ArticleCatalog(new Application());
            if (!string.IsNullOrWhiteSpace(id))
            {
                var item = await catalog.GetAsync(id);
                if (item != null)
                {
                    await catalog.IncrementAsync(id, new Dictionary<Expression<Func<Article, long>>, long> { { it => it.SocialNetwork.CommentsCount, 1 } });
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}