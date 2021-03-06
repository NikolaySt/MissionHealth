using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialNet.WebSite.Controllers
{
    public class AboutmeController : SuperController
	{
        public ActionResult Index()
        {
            return View();
        }
    }
}