using System.Web.Mvc;

namespace SocialNet.WebSite.Controllers
{    
    [WhitespaceFilter]
#if RELEASE
	[OutputCache(CacheProfile = "CacheProfile")]
#endif
	public class SuperController : Controller
    {

    }
}