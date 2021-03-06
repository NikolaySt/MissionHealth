using System.Web.Mvc;

namespace SocialNet.WebSite.Controllers
{
    public class ErrorController : SuperController
	{
		public ActionResult Http404()
		{
			return View("NotFoundError");
		}
	}
}