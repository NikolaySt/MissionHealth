using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

using SocialNet.Backend.Security;
using System.Threading;

namespace SocialNet.WebSite
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
	public class UserAuthorizationAttribute : AuthorizeAttribute
	{
		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			var principal = HttpContext.Current.User as ClaimsPrincipal;
			if (principal == null) return false;

			var claim = principal.Claims.FirstOrDefault(it => it.Type == Claims.UserId);
			if (claim == null) return false;

			return true;
		}
	}
}
