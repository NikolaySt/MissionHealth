using System.Web;
using System.Web.Optimization;

namespace SocialNet.WebSite
{
	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/Scripts/jquery-{version}.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
						"~/Scripts/jquery-ui-{version}.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
						"~/Scripts/jquery.validate*"));

			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
					  "~/Scripts/bootstrap.js",
					  "~/Scripts/respond.js"));

			bundles.Add(new ScriptBundle("~/bundles/plugins").Include(
					  "~/assets/plugins/back-to-top.js",
					  "~/assets/js/app.js",
					  "~/assets/js/spinners.js",
					  "~/assets/js/custom.js",
					  "~/assets/plugins/smoothScroll.js"));

			bundles.Add(new StyleBundle("~/Content/css").Include(
					  "~/Content/bootstrap.css",
					  "~/Content/site.css")
					  .Include("~/assets/css/app.css", new CssRewriteUrlTransformWrapper())
					  .Include(
					  "~/assets/css/style.css",
					  "~/assets/css/headers/header-default.css",
					  "~/assets/css/footers/footer-v2.css",
					  "~/assets/css/theme-colors/blue.css"));

			bundles.Add(new StyleBundle("~/Content/css/custom").Include(
					  "~/assets/css/custom.css",
					  "~/assets/css/spinners.css"));

			bundles.Add(new StyleBundle("~/Content/css/fonts")
				.Include("~/assets/plugins/line-icons/line-icons.css", new CssRewriteUrlTransformWrapper())
				.Include("~/fonts/Simbal/fonts.css", new CssRewriteUrlTransformWrapper())
				.Include("~/assets/plugins/font-awesome/css/font-awesome.min.css", new CssRewriteUrlTransformWrapper()));

			bundles.Add(new StyleBundle("~/content/css/contact").
				Include(
				"~/assets/plugins/sky-forms-pro/skyforms/css/sky-forms.css",
				"~/assets/plugins/sky-forms-pro/skyforms/custom/custom-sky-forms.css",
				"~/assets/css/pages/page_contact.css"
				));

            bundles.Add(new StyleBundle("~/content/css/sky-forms").
                Include(
                "~/assets/plugins/sky-forms-pro/skyforms/css/sky-forms.css",
                "~/assets/plugins/sky-forms-pro/skyforms/custom/custom-sky-forms.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/contact").Include(
					  "~/assets/js/pages/page_contacts.js"));

			bundles.Add(new StyleBundle("~/content/css/search").
				Include(
				"~/assets/css/pages/page_search_inner.css"
				));

#if RELEASE
			BundleTable.EnableOptimizations = true;
#endif
#if DEBUG
			BundleTable.EnableOptimizations = false;
#endif

		}

		public class CssRewriteUrlTransformWrapper : IItemTransform
		{
			public string Process(string includedVirtualPath, string input)
			{
				return new CssRewriteUrlTransform().Process("~" + VirtualPathUtility.ToAbsolute(includedVirtualPath), input);
			}
		}
	}
}
