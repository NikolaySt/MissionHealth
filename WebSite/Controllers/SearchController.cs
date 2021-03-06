using System.Threading.Tasks;
using System.Web.Mvc;
using SocialNet.Backend.Catalogs;
using SocialNet.Backend.DataFilters;
using SocialNet.Backend.DataObjects;

namespace SocialNet.WebSite.Controllers
{
    public class SearchController : SuperController
	{
        [Route("{criteria}")]
		[ValidateInput(false)]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Index(string criteria, int page = 0)
        {
            ViewBag.Criteria = criteria;

            if (string.IsNullOrWhiteSpace(criteria)) return View(new PagedResult<Article>());

            var catalog = new ArticleCatalog(new Application());

            var filter = new ArticleListFilter() { };
            
            filter.Criteria = criteria;

            filter.CriteriaType = CriteriaType.Keywords;

            filter.Published = true;

            var result = await catalog.GetPageAsync(filter, page, 20, "Created DESC");

            return View(result);
        }
    }
}