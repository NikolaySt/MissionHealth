using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SocialNet.WebSite.Areas.Book
{
    public class BookAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Book";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Book_default",
                "Book/{controller}/{action}/{id}",
                new { action = "Index", controller = "Home", id = UrlParameter.Optional },
                namespaces: new[] { "SocialNet.WebSite.Areas.Book.Controllers" }
            );

            context.MapRoute(
                "Book_title",
                "Book/Title/{title}/Review/{id}",
                new { action = "index", controller = "review", id = UrlParameter.Optional },
                namespaces: new[] { "SocialNet.WebSite.Areas.Book.Controllers" }
            );
        }
    }
}