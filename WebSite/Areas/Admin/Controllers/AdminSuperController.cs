using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SocialNet.WebSite.Areas.Admin.Controllers
{
    [UserAuthorization]
    public abstract class AdminSuperController : Controller
    {

        protected void ClearAuthenticationCookie()
        {
            var portalCookiesName = Request.Cookies.AllKeys.Where(it => it.StartsWith("MisiaZdrave."));

            foreach (var name in portalCookiesName)
            {
                AddCookie(name, "", false);
            }
        }

        protected void SetAuthenticationCookie(string token, bool isPersistent)
        {
            AddCookie("MisiaZdrave.oauth", token, isPersistent);
        }

        private void AddCookie(string name, string value, bool isPersistent)
        {
            var authTicket = new FormsAuthenticationTicket(1, "token", DateTime.Now, DateTime.Now.AddDays(7), isPersistent, value, FormsAuthentication.FormsCookiePath);

            var authCookie = new HttpCookie(name, FormsAuthentication.Encrypt(authTicket));

            if (authTicket.IsPersistent)
            {
                authCookie.Expires = authTicket.Expiration;
            }
            Response.Cookies.Add(authCookie);
        }
    }
}