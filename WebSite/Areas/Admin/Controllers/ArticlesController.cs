using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections.Generic;

using SocialNet.Backend.Catalogs;
using SocialNet.Backend.DataFilters;
using SocialNet.Backend.DataObjects;
using SocialNet.Backend.Configuration;

using SocialNet.WebSite.Areas.Admin.Models;

namespace SocialNet.WebSite.Areas.Admin.Controllers
{
    public class ArticlesController : AdminSuperController
    {
        public async Task<ActionResult> Index(int page = 0, string areaValue = "")
        {
            var catalog = new ArticleCatalog();
            var filter = new ArticleListFilter();
            if (!string.IsNullOrWhiteSpace(areaValue))
             {
                filter.AreaValue = areaValue;
            };

            ViewBag.AreaValue = areaValue;

            var result = await catalog.GetPageAsync(filter, page, 5, "Created DESC");

            if (Request.IsAjaxRequest())
            {
                return PartialView("_articles", result.Items);
            }

            return View(result.Items);
        }

        public async Task<ActionResult> Create()
        {
            var model = new ArticleViewModel()
            {
                Areas = await BuildAreas(),
                Categories = new List<SelectListItem>(),
            };

            return View("Edit", model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(ArticleViewModel model)
        {
			if (!TryValidateModel(model))
			{
				return View("Edit", model);
			}

			return await UpdateOrCreate(model);
		}

		public async Task<ActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return RedirectToAction("Index");

            var categoryCatalog = new ArticleCategoryCatalog();
            var categories = await categoryCatalog.GetAsync(new PostCategoryListFilter(), null);

            var areas = await BuildAreas();

            var postCatalog = new ArticleCatalog();

            var post = await postCatalog.GetAsync(id);

            var model = new ArticleViewModel()
            {
                Id = post.Id,
                AreaValue = post.Category?.AreaValue,
                CategoryValue = post.Category?.AreaValue + "." + post.Category?.CategoryValue + "." + post.Category?.SubCategoryValue,
                HtmlRaw = post.Content?.Html,
                Text = post.Content?.Text,
                Title = post.Title?.Text,
                TitleType = post.Title.Type,
                ImageUrl = post.Title.ImageUrl,
                VideoUrl = post.Title.VideoUrl,
                ImageThumbnailUrl = post.Title.ImageThumbnailUrl,
                ImageSmallUrl = post.Title.ImageSmallUrl,
                Top = post.Top,
                Interesting = post.Interesting,
				Review = post.Review,
				Keywords = string.Join(",", post.Keywords ?? new List<string>())
			};

            string areaValue = post.Category?.AreaValue;

            model.Areas = await BuildAreas();

            model.Categories = await BuildCategories(areaValue);

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(string id, ArticleViewModel model)
        {
			if (!TryValidateModel(model))
			{
				return View(model);
			}
			return await UpdateOrCreate(model);
		}

		private async Task<ActionResult> UpdateOrCreate(ArticleViewModel model)
		{
			var postCategoryCatalog = new ArticleCategoryCatalog();
			var categories = await postCategoryCatalog.GetAsync(new PostCategoryListFilter(), null);

			model.Areas = await BuildAreas();
			model.Categories = await BuildCategories(model.AreaValue);

			var catalog = new ArticleCatalog();

			Article item = null;

			if (string.IsNullOrWhiteSpace(model.Id))
			{
				item = new Article();
			}
			else
			{
				item = await catalog.GetAsync(model.Id);
			}

			item.Title = new Content();
			item.Title.Text = model.Title;
			item.Title.Type = model.TitleType;
			item.Title.ImageUrl = model.ImageUrl;
			item.Title.VideoUrl = model.VideoUrl;
			item.Title.ImageThumbnailUrl = model.ImageThumbnailUrl;
			item.Title.ImageSmallUrl = model.ImageSmallUrl;
			item.TitleId = Regex.Replace(model.Title, @"\W+", "-").ToLower();

			item.Review = !string.IsNullOrWhiteSpace(model.Review) ? model.Review : model.Text.TruncateAtWord(200);

			item.Keywords = !string.IsNullOrWhiteSpace(model.Keywords) ? Keywords(model.Keywords) : Keywords(model.Text);

			item.Content = new Content();
			item.Content.Text = model.Text;
			item.Content.Html = model.HtmlRaw;
			item.Content.Type = ContentType.Text;
			item.Top = model.Top;
			item.Interesting = model.Interesting;

			var categoryValue = model.CategoryValue.Split('.')[1];
			var subCategoryValue = model.CategoryValue.Split('.')[2];

			categories = await postCategoryCatalog.GetAsync(new PostCategoryListFilter()
			{
				AreaValue = model.AreaValue,
				CategoryValue = categoryValue,
				SubCategoryValue = subCategoryValue
			}, null);

			var category = categories.FirstOrDefault(
				it => it.AreaValue == model.AreaValue &&
				(it.CategoryValue == categoryValue || (string.IsNullOrWhiteSpace(it.CategoryValue) && categoryValue == "")) &&
				(it.SubCategoryValue == subCategoryValue || (string.IsNullOrWhiteSpace(it.SubCategoryValue) && subCategoryValue == ""))
				);

			if (category == null)
			{
				return View("Edit", model);
			}

			item.Category = category;

			item = await catalog.StoreAsync(item);

			model.Id = item.Id;

			return View("Edit", model);
		}

		[HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            var catalog = new ArticleCatalog();

            await catalog.DeleteAsync(id);

            if (Request.IsAjaxRequest())
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK, "Статията беше успешно изтрита");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Published(string id, bool published)
        {
            var catalog = new ArticleCatalog();

            var item = await catalog.GetAsync(id);

            item.Published = published;

			item.Created = DateTime.UtcNow;

			await catalog.StoreAsync(item);

            if (Request.IsAjaxRequest())
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }

            return RedirectToAction("Index");
        }

		public async Task<ActionResult> GetCategories(string area)
		{
			var result = await BuildCategories(area);

			return PartialView("_category", result);
		}

		public ActionResult Gallery(string selectCallback)
        {
            ViewBag.Callback = selectCallback;
            var path = Server.MapPath(SystemSettings.PhotoContainer);

            string[] files = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly).Select(it =>
            {
                var position = it.IndexOf("content");
                return string.Concat("~/", it.Substring(position, it.Length - position).Replace("\\", "/"));
            }).ToArray();

            ViewBag.Folders = GetDirectory(new DirectoryInfo(path));

            if (Request.IsAjaxRequest())
            {
                return PartialView("_galleryTitles", files);
            }

            return View("_galleryEditor", files);
        }

		private List<string> Keywords(string text, int weight = 1, int count = 20)
		{
			var list = Regex
				.Split(text.ToLowerInvariant().Trim(), @"\W+")
				.Where(it => !string.IsNullOrWhiteSpace(it) && it.Length >= 5)
				.OrderBy(it => it);

			var keyword = list.GroupBy(x => x)
				.Select(g => new { Value = g.Key, Count = g.Count() })
				.OrderByDescending(x => x.Count).Where(it => it.Count > weight).Select(it => it.Value).Take(count).ToList();

			if (keyword.Count() < count)
			{
				var more = list.GroupBy(x => x)
				.Select(g => new { Value = g.Key, Count = g.Count() })
				.OrderByDescending(x => x.Count)
				.Where(it => it.Count <= weight).Select(it => it.Value).Take(count - keyword.Count()).ToList();
				keyword.AddRange(more);
			}

			return keyword;
		}

        public ActionResult Images(string folder)
        {
            var path = Server.MapPath(SystemSettings.PhotoContainer + folder + "/");
            var list = Directory.GetFiles(path);
            string[] files = list.Select(it =>
            {
                var position = it.IndexOf("content");
                return string.Concat("~/", it.Substring(position, it.Length - position).Replace("\\", "/"));
            }).ToArray();

            return PartialView("_images", files);
        }

        private List<string> GetDirectory(DirectoryInfo directory)
        {
            var result = new List<string>();

            if (directory == null) return result;

            DirectoryInfo[] SubDirectories = directory.GetDirectories();

            foreach (var item in SubDirectories)
            {
                result.Add(item.Name);
            }

            return result;
        }        

        private string ProcessCategory(string areaValue, string categoryValue, string subCategoryValue)
        {
            var result = areaValue;
            if (!string.IsNullOrWhiteSpace(categoryValue))
            {
                result += " > " + categoryValue;
            }
            if (!string.IsNullOrWhiteSpace(subCategoryValue))
            {
                result += " > " + subCategoryValue;
            }
            return result;
        }

        private async Task<List<SelectListItem>> BuildAreas()
        {
            var postCategoryCatalog = new ArticleCategoryCatalog();
            var categories = await postCategoryCatalog.GetAsync(new PostCategoryListFilter(), null);

            var areas = categories
                                .GroupBy(i => new { i.AreaValue, i.AreaName })
                                .Select(i => new SelectListItem { Value = i.Key.AreaValue, Text = i.Key.AreaName }).ToList();

            areas.Insert(0, new SelectListItem { Value = "", Text = "[избор]" });

            return areas;
        }

        private async Task<List<SelectListItem>> BuildCategories(string areaValue)
        {
            var postCategoryCatalog = new ArticleCategoryCatalog();
            var categories = await postCategoryCatalog.GetAsync(new PostCategoryListFilter(), null);

            var list = new List<SelectListItem>();
            if (!string.IsNullOrWhiteSpace(areaValue))
            {
                list = categories
                        .Where(it => it.AreaValue == areaValue)
                        .GroupBy(i => new { i.AreaValue, i.CategoryValue, i.SubCategoryValue })
                        .Select(i => new SelectListItem
                        {
                            Value = i.Key.AreaValue + "." + i.Key.CategoryValue + "." + i.Key.SubCategoryValue,
                            Text = ProcessCategory(i.Key.AreaValue, i.Key.CategoryValue, i.Key.SubCategoryValue)
                        }).ToList();

                list.Insert(0, new SelectListItem { Value = "", Text = "[избор]" });               
            }

            return list;
        }
	}
}