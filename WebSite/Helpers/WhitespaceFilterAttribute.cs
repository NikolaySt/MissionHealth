using SocialNet.Backend.Helpers;
using System.Web.Mvc;

namespace SocialNet.WebSite
{
    public class WhitespaceFilterAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
#if RELEASE
			//var request = filterContext.HttpContext.Request;
            var response = filterContext.HttpContext.Response;

            response.Filter = MinifyResponseHelper.Process(response.Filter);
#endif
        }
    }
}