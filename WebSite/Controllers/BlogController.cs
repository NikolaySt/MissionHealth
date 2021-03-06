using System.Threading.Tasks;
using System.Web.Mvc;
using SocialNet.Backend.Catalogs;
using SocialNet.Backend.DataFilters;
using SocialNet.WebSite.Controllers;

namespace SocialNet.WebSite.Areas.Health.Controllers
{
    public class BlogController : SuperController
    {
        protected string Area { get; set; }

        protected string Category { get; set; }

        protected string SubCategory { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Area = (string)filterContext.RouteData.Values["main"];
            Category = (string)filterContext.RouteData.Values["category"];
            SubCategory = (string)filterContext.RouteData.Values["subcategory"];

            base.OnActionExecuting(filterContext);
        }

        public async Task<ActionResult> Index(string id, int page = 0)
        {
            var catalog = new ArticleCatalog();

			if (SubCategory?.ToLower() == "post") SubCategory = null;

			var filter = new ArticleListFilter()
            {
                AreaValue = Area?.ToLower(),
                CategoryValue = Category?.ToLower(),
                SubCategoryValue = SubCategory?.ToLower(),
                Interesting = false,
                Published = true,
            };

            if (!string.IsNullOrWhiteSpace(id))
            {
                var article = await catalog.GetAsync(id);

                if (article == null || (!article.Published && !Request.IsAuthenticated))
                {
                    return RedirectToAction("Index");
                }            

                filter.Interesting = true;

                var more = await catalog.GetPageAsync(filter, page, 10, "Created DESC");

                ViewBag.More = new MoreViewModel() { Title = "Интересно", Articles = more.Items };

                return View("Article", article);
            }


            var result = await catalog.GetPageAsync(filter, page, 10, "Created DESC");

            if (Request.IsAjaxRequest())
            {
                return PartialView("_articlesFeed", result.Items);
            }

            filter.Interesting = true;

            var interesting = await catalog.GetPageAsync(filter, page, 10, "Created DESC");

            ViewBag.More = new MoreViewModel()
            {
                Title = "Интересно",
                Articles = interesting.Items
            };

            ViewBag.Articles = result.Items;

            return View("Index");
        }
    }
}