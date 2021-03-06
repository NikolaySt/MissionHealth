using SocialNet.Backend.DataObjects;
using System;
using System.Web;
using System.Web.Mvc;

namespace SocialNet.WebSite
{
	public static class UrlExtention
	{
		public static string Content(this string contentPath)
		{
			return VirtualPathUtility.ToAbsolute(contentPath ?? "~");
//#if DEBUG
//			return VirtualPathUtility.ToAbsolute(contentPath ?? "~");
//#endif
//#if RELEASE
//			return string.Format("{0}://{1}{2}",
//							   "https",
//							   "misiazdrave.r.worldssl.net",
//							   VirtualPathUtility.ToAbsolute(contentPath ?? "~"));
//#endif
		}

        public static string ContentSocialNetwork(this string contentPath)
        {			
#if DEBUG
			return VirtualPathUtility.ToAbsolute(contentPath ?? "~");
#endif
#if RELEASE
			return string.Format("{0}://{1}{2}",
							   "https",
							   "misiazdrave.bg",
							   VirtualPathUtility.ToAbsolute(contentPath ?? "~"));
#endif
        }

        public static string Cdn()
		{
			return "";
//#if DEBUG
//			return "";
//#endif
//#if RELEASE
//			return string.Format("{0}://{1}",
//							   "https",
//							   "misiazdrave.r.worldssl.net");
//#endif
		}
	}

	public static partial class HtmlHelperExtensions
	{
		public static bool IsLocalHost(this HttpRequestBase html)
		{
			var remoteAddress = html.UserHostAddress;
			// if unknown, assume not local
			if (string.IsNullOrWhiteSpace(remoteAddress))
				return false;

			// check if localhost
			if (remoteAddress == "127.0.0.1" || remoteAddress == "::1" || remoteAddress == "localhost")
				return true;
			return false;
		}



		public static string IsSelected(this HtmlHelper html, string area = null, string controller = null, string action = null, string cssClass = null)
		{
			cssClass = cssClass ?? "active";

			string currentArea = (string)html.ViewContext.RouteData.Values["main"];
			string currentAction = (string)html.ViewContext.RouteData.Values["action"];
			string currentController = (string)html.ViewContext.RouteData.Values["controller"];

			if (string.IsNullOrEmpty(controller))
				controller = currentController;

			if (string.IsNullOrEmpty(action))
				action = currentAction;

			if (string.IsNullOrEmpty(area))
				area = currentArea;

			return
				(area?.ToLower() == currentArea?.ToLower() || (area == "" && currentArea == null)) &&
				controller?.ToLower() == currentController?.ToLower() &&
				action?.ToLower() == currentAction?.ToLower()
				? cssClass
				: string.Empty;
		}

		public static string IsSelectedCategory(this HtmlHelper html, string area = null, string category = null, string subCategory = null, string cssClass = null)
		{
			cssClass = cssClass ?? "active";

			string currentArea = (string)html.ViewContext.RouteData.Values["main"] ?? "";
			string currentCategory = (string)html.ViewContext.RouteData.Values["category"] ?? "";
			string currentSubcategory = (string)html.ViewContext.RouteData.Values["subcategory"] ?? "";

			if (category == null)
				category = currentCategory;

			if (subCategory == null)
				subCategory = currentSubcategory;

			if (area == null)
				area = currentArea;

			return
				(area?.ToLower() == currentArea?.ToLower()) &&
				category?.ToLower() == currentCategory?.ToLower() &&
				subCategory?.ToLower() == currentSubcategory?.ToLower()
				? cssClass
				: string.Empty;
		}

		public static string ActionCategory(this UrlHelper url, string area = null, string category = null, string subCategory = null, string id = null, string titleId = null)
		{
			if (string.IsNullOrWhiteSpace(titleId))
			{
				return VirtualPathUtility.ToAbsolute(string.Concat("~/blog/", area, "/", category, "/", subCategory, "/", string.IsNullOrWhiteSpace(id) ? null : "?id=" + id));
			}
			else
			{
				if (string.IsNullOrWhiteSpace(category))
				{
					return VirtualPathUtility.ToAbsolute(string.Concat("~/blog/", area, "/post/", titleId , "/", string.IsNullOrWhiteSpace(id) ? null : "?id=" + id));
				}
				return VirtualPathUtility.ToAbsolute(string.Concat("~/blog/", area, "/", category, "/", subCategory, "/", titleId, "/", id));
			}
		}


		public static string ActionArticle(this UrlHelper url, Article article)
		{
			var category = article.Category;
			return url.ActionCategory(category.AreaValue, category.CategoryValue, category.SubCategoryValue ?? "post", article.Id, article.TitleId);
		}

		public static string ActionSubCategory(this UrlHelper url, Article article)
		{
			var category = article.Category;
			return url.ActionCategory(category.AreaValue, category.CategoryValue, category.SubCategoryValue);
		}

		public static string ActionCategory(this UrlHelper url, Article article)
		{
			var category = article.Category;
			return url.ActionCategory(category.AreaValue, category.CategoryValue);
		}

		public static string ActionArea(this UrlHelper url, Article article)
		{
			var category = article.Category;
			return url.ActionCategory(category.AreaValue);
		}

		public static string PageClass(this HtmlHelper html)
		{
			string currentAction = (string)html.ViewContext.RouteData.Values["action"];
			return currentAction;
		}

		public static string GetFullUrl(this HttpRequestBase request, string relativeUrl)
		{
			return string.Format("{0}://{1}{2}",
							request.Url.Scheme,
							request.Url.Authority,
							VirtualPathUtility.ToAbsolute(relativeUrl));
		}


		public static string EncodeBase64(string text)
		{
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(text);
			return Convert.ToBase64String(plainTextBytes);
		}

		public static string TimeAgo(DateTime dt)
		{
			TimeSpan span = DateTime.UtcNow - dt;
			if (span.Days > 365)
			{
				int years = (span.Days / 365);
				if (span.Days % 365 != 0)
				{
					years += 1;
				}
				return string.Format("преди {0} {1}", years, years == 1 ? "година" : "години");
			}
			if (span.Days > 30)
			{
				int months = (span.Days / 30);
				if (span.Days % 31 != 0)
				{
					months += 1;
				}
				return string.Format("преди {0} {1}", months, months == 1 ? "месец" : "месеца");
			}
			if (span.Days > 0) return string.Format("преди {0} {1}", span.Days, span.Days == 1 ? "ден" : "дни");

			if (span.Hours > 0)
				return string.Format("преди {0} {1}", span.Hours, span.Hours == 1 ? "час" : "часа");
			if (span.Minutes > 0)
				return string.Format("преди {0} {1}", span.Minutes, span.Minutes == 1 ? "минута" : "минути");
			if (span.Seconds > 5) return string.Format("преди {0} секунди", span.Seconds);
			if (span.Seconds <= 5) return "точно сега";

			return dt.ToString(MvcApplication.CultureInfo.DateTimeFormat.LongDatePattern, MvcApplication.CultureInfo);
		}

		public static string TruncateAtWord(this string input, int length)
		{
			if (input == null || input.Length < length)
				return input;
			int iNextSpace = input.LastIndexOf(" ", length);
			return string.Format("{0}", input.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim());
		}
	}
}
