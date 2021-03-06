using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SocialNet.WebSite
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            RegisterCMS(routes);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SocialNet.WebSite.Controllers" }
            );
        }

        private static void RegisterCMS(RouteCollection routes)
        {
            routes.MapRoute(
                "Health_Area",
                "blog/{main}",
                new { action = "Index", controller = "Blog", main="Health" },
                namespaces: new[] { "SocialNet.WebSite.Controllers" }
            );

            routes.MapRoute(
                "Health_Category",
                "blog/{main}/{category}",
                new { action = "Index", controller = "Blog" },
                namespaces: new[] { "SocialNet.WebSite.Controllers" }
            );

            routes.MapRoute(
                "Health_SubCategory",
                "blog/{main}/{category}/{subcategory}",
                new { action = "Index", controller = "Blog" },
                namespaces: new[] { "SocialNet.WebSite.Controllers" }
            );

            routes.MapRoute(
                "Health_Article",
				"blog/{main}/{category}/{subcategory}/{title}/{id}",
                new { action = "Index", controller = "Blog", id = UrlParameter.Optional },
                namespaces: new[] { "SocialNet.WebSite.Controllers" }
            );
		}
    }
}
