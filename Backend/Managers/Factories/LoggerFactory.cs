using System.Reflection;
using SocialNet.Backend.Logging;
using SocialNet.Backend.Security;
using SocialNet.Backend.Catalogs;

namespace SocialNet.Backend.Managers
{
    public static class LoggerFactory
    {
        public static ILogger GetInstance(string componentName, string fallbackFileName = "Log.txt")
        {
            var settings = new LoggerSettings
            {
                ComponentName = componentName,
                ComponentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                AppRealmId = SecurityContext.AppRealmId,
                AppId = SecurityContext.AppId,
                UserId = SecurityContext.UserId,
                TokenId = SecurityContext.TokenId
            };

			var catalog = new LogCatalog();

            return new Logger(settings, catalog, fallbackFileName);
        }
    }
}