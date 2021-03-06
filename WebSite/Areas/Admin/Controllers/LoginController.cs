using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using System.Threading;
using System.Security.Principal;

using SocialNet.WebSite.Areas.Admin.Models;
using SocialNet.Backend.Managers;
using SocialNet.Backend.Configuration;
using SocialNet.Backend.Security;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SocialNet.WebSite.Areas.Admin.Controllers
{
    public class LoginController : AdminSuperController
    {
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            var emptyPrincipal = new GenericPrincipal(new GenericIdentity(string.Empty), null);

            Thread.CurrentPrincipal = emptyPrincipal;

            HttpContext.User = emptyPrincipal;

            ClearAuthenticationCookie();

            return RedirectToAction("", "", new { area = "" });
        }

        [AllowAnonymous]
        public ActionResult Index(string returnUrl)
        {
            if (Request.IsAuthenticated || !string.IsNullOrWhiteSpace(SecurityContext.UserId))
            {
                if (Url.IsLocalUrl(returnUrl)
                    && returnUrl.Length > 1
                    && returnUrl.StartsWith("/")
                    && !returnUrl.StartsWith("//")
                    && !returnUrl.StartsWith("/\\"))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("", "Home");
            }

            var model = new LoginViewModel() { ReturnUrl = returnUrl };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(string returnUrl, LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            try
            {
                var security = new SecurityManager();

                security.SetContext(SystemSettings.SecureToken);

                var manager = new UserManager();

                var token = await manager.LogInAsync(model.Email, model.Password);

                SetAuthenticationCookie(token, model.RememberMe);
            }
            catch(Exception exception)
            {
                ModelState.AddModelError("", "Invalid login attempt.");

                var logger = LoggerFactory.GetInstance("Portal");

                await logger.LogErrorAsync(exception, "Login", "Index", 
                    new Dictionary<string, object> { { "model", JsonConvert.SerializeObject(model) } });

                return View("Index", model);
            }

            if (Url.IsLocalUrl(returnUrl)
                && returnUrl.Length > 1
                && returnUrl.StartsWith("/")
                && !returnUrl.StartsWith("//")
                && !returnUrl.StartsWith("/\\"))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("", "Home");
        }
    }
}