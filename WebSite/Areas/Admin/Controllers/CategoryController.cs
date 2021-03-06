using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

using SocialNet.Backend.Catalogs;
using SocialNet.Backend.DataFilters;
using SocialNet.Backend.DataObjects;
using System.Net;

namespace SocialNet.WebSite.Areas.Admin.Controllers
{

    public class CategoryController : AdminSuperController
    {
        public async Task<ActionResult> Index()
        {
            var postCategoryCatalog = new ArticleCategoryCatalog();

            var categories = await postCategoryCatalog.GetAsync(new PostCategoryListFilter(), null);

            ViewBag.Areas = categories.GroupBy(i => new { i.AreaValue, i.AreaName })
                          .Select(i => new Tuple<string, string>(i.Key.AreaValue, i.Key.AreaName)).ToList();

            return View(categories);
        }


        public ActionResult Create()
        {
            return View(new ArticleCategory());
        }

        [HttpPost]
        public async Task<ActionResult> Create(ArticleCategory model)
        {
            var item = new ArticleCategory();
            if (!TryUpdateModel(item))
            {
                return View(model);
            }

            var catalog = new ArticleCategoryCatalog();

            item.AreaValue = item.AreaValue?.ToLower();
            item.CategoryValue = item.CategoryValue?.ToLower();
            item.SubCategoryValue = item.SubCategoryValue?.ToLower();

            item = await catalog.StoreAsync(item);

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) RedirectToAction("Index");

            var catalog = new ArticleCategoryCatalog();

            var model = await catalog.GetAsync(id);

            return PartialView("_edit", model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(string id, ArticleCategory model)
        {
            if (string.IsNullOrWhiteSpace(id)) RedirectToAction("Index");

            var catalog = new ArticleCategoryCatalog();

            var item = await catalog.GetAsync(id);

            if (!TryUpdateModel(item))
            {
                return View(model);
            }

            item = await catalog.StoreAsync(item);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) RedirectToAction("Index");

            var catalog = new ArticleCategoryCatalog();

            await catalog.DeleteAsync(id);

            return new HttpStatusCodeResult(HttpStatusCode.OK, "Категория е изтрита успешно");
        }

        public async Task<ActionResult> GetCategories(string area)
        {
            if (string.IsNullOrWhiteSpace(area))
            {
                return PartialView("_listOptionsCategory", new List<Tuple<string, string>>());
            }

            var catalog = new ArticleCategoryCatalog();

            var categories = await catalog.GetAsync(new PostCategoryListFilter() { AreaValue = area }, null);

            var result = categories
                    .GroupBy(i => new { i.AreaValue, i.CategoryValue, i.SubCategoryValue })
                    .Select(i => new SelectListItem
                    {
                        Value = i.Key.AreaValue + "." + i.Key.CategoryValue + "." + i.Key.SubCategoryValue,
                        Text = i.Key.AreaValue + "." + i.Key.CategoryValue + "." + i.Key.SubCategoryValue
                    }).ToList();

            result.Insert(0, new SelectListItem { Value = "", Text = "[избор]" });

            return PartialView("_listOptionsCategory", result);
        }

    }
}