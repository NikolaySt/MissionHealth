using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SocialNet.Backend.Catalogs;
using SocialNet.Backend.DataObjects;

namespace SocialNet.Backend.Logging
{
    public class Logger : ILogger
    {
        private readonly LoggerSettings _settings;
        private readonly LogCatalog _catalog;

        private string _fallbackFileName;
        private object _fileLock = new object();


        public Logger(LoggerSettings settings, LogCatalog catalog, string fallbackFileName)
        {
            if (settings == null) throw new ArgumentException("settings");
            if (catalog == null) throw new ArgumentException("catalog");
            if (string.IsNullOrWhiteSpace(fallbackFileName)) throw new ArgumentException("fallbackFileName");

            _settings = settings;
            _catalog = catalog;
            _fallbackFileName = fallbackFileName;
        }

        public async Task LogErrorAsync(
            Exception ex,
            string className,
            string methodName,
            Dictionary<string, object> context = null)
        {

            var log = GetLog(LogLevel.Error, className, methodName, context);

            log.Message = ex.Message;

            log.Exception = GetLogException(ex);

            if (ex.InnerException != null)
                log.InnerException = GetLogException(ex.InnerException);

            await DoLog(log);
        }

        public async Task LogErrorAsync(
            string message,
            string className,
            string methodName,
            Dictionary<string, object> context = null)
        {
            var log = GetLog(LogLevel.Error, className, methodName, context);

            log.Message = message;

            await DoLog(log);
        }

        public async Task LogWarningAsync(
            string message,
            string className,
            string methodName,
            Dictionary<string, object> context = null)
        {
            var log = GetLog(LogLevel.Warning, className, methodName, context);

            log.Message = message;

            await DoLog(log);
        }

        public async Task LogTraceAsync(
            string message,
            string className,
            string methodName,
            Dictionary<string, object> context = null)
        {
            var log = GetLog(LogLevel.Warning, className, methodName, context);

            log.Message = message;

            await DoLog(log);
        }

        private async Task DoLog(Log log)
        {
            try
            {
                await _catalog.StoreAsync(log);
            }
            catch
            {
                lock (_fileLock)
                {
                    using (var writer = File.AppendText(_fallbackFileName))
                    {
                        writer.Write(log.ToString());
                        writer.Flush();
                    }
                }
            }
        }

        private Log GetLog(
            LogLevel level,
            string className,
            string methodName,
            Dictionary<string, object> context)
        {
            var log = new Log
            {
                Level = level,
                ComponentName = _settings.ComponentName,
                ComponentVersion = _settings.ComponentVersion,
                ClassName = className,
                MethodName = methodName,
                AppRealmId = _settings.AppRealmId,
                AppId = _settings.AppId,
                UserId = _settings.UserId,
                Status = LogStatus.New,
            };

            if (context == null) return log;

            log.Context = new Dictionary<string, object>();

            foreach (var item in context)
            {
                log.Context.Add(item.Key, item.Value != null ? item.Value.ToString() : "");
            }

            return log;
        }

        private LogException GetLogException(
            Exception ex)
        {
            return new LogException
             {
                 Type = ex.GetType().ToString(),
                 Message = ex.Message,
                 Source = ex.Source,
                 StackTrace = ex.StackTrace,
             };
        }
    }
}
