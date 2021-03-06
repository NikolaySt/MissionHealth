using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Web.Optimization;
using System.Web.Routing;
using System.Globalization;
using System.Threading;
using System.Web.Security;
using System.Web.Helpers;
using System.Security.Claims;

using SocialNet.Backend.Configuration;
using SocialNet.Backend.Database;
using SocialNet.Backend.Catalogs;
using SocialNet.Backend.Managers;

namespace SocialNet.WebSite
{
    public class MvcApplication : HttpApplication
    {
        const string culture = "bg-BG";

        private static CultureInfo _cultureInfo;
        public static CultureInfo CultureInfo
        {
            get
            {
                if (_cultureInfo == null) _cultureInfo = new CultureInfo(culture);
                return _cultureInfo;
            }
        }

		public bool IsLocal
		{
			get
			{
				var remoteAddress = Request.UserHostAddress;
				// if unknown, assume not local
				if (string.IsNullOrWhiteSpace(remoteAddress))
					return false;

				// check if localhost
				if (remoteAddress == "127.0.0.1" || remoteAddress == "::1" || remoteAddress == "localhost")
					return true;

				return false;
			}
		}

        public MvcApplication()
        {
            PostAuthenticateRequest += Application_PostAuthenticateRequest;
        }

        protected void Application_Start()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo;
            Thread.CurrentThread.CurrentUICulture = CultureInfo;

            SystemSettings.Load(HttpContext.Current.Server.MapPath("~/") + "\\bin");

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var settings = new MongoProviderSettings
            {
                ConnectionString = SystemSettings.DatabaseConnectionString,
                DatabaseName = SystemSettings.DatabaseName,
                CollectionNamespace = SystemSettings.DatabaseCollectionNamespace,
                ConnectionTimeout = SystemSettings.ConnectionTimeout,
                ConnectionPoolSize = SystemSettings.ConnectionPoolSize,
                WaitQueueSize = SystemSettings.WaitQueueSize
            };
            MongoProvider.InitDefaultInstance(settings);

            DatabaseInitializer.Init();

			AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
		}

        protected void Application_PostAuthenticateRequest(object sender, EventArgs eventArgs)
        {
            var cookie = Request.Cookies["MisiaZdrave.oauth"];

            if (cookie == null) return;

            var ticket = FormsAuthentication.Decrypt(cookie.Value);

            if (ticket == null) return;

            if (ticket.Expired) return;

            if (string.IsNullOrWhiteSpace(ticket.UserData)) return;            

            var security = new SecurityManager();

            security.SetContext(ticket.UserData);
        }

        protected void Application_BeginRequest()
		{
			if (!IsLocal && FormsAuthentication.RequireSSL && !Request.IsSecureConnection)
			{
				Response.Redirect(Request.Url.AbsoluteUri.Replace("http://", "https://"));
			}
		}

		protected void Application_Error()
		{
			var exception = Server.GetLastError();
			if (exception == null) return;

			var httpException = exception as HttpException;
			if (httpException != null)
			{
				switch (httpException.GetHttpCode())
				{
					case 404: return;
				}
			}

			var context = new Dictionary<string, object>();
			if (HttpContext.Current != null)
			{
				var url = HttpContext.Current.Request.Url.ToString();

				context.Add("Request Uri", url);
			}

			var logger = LoggerFactory.GetInstance("Portal");

			logger.LogErrorAsync(exception, "Portal", "Application_Error", context);

			//Server.ClearError();
		}
	}
}
