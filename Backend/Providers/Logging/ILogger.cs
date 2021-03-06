using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialNet.Backend.Logging
{
    public interface ILogger
    {
        Task LogErrorAsync(
            Exception ex, 
            string className, 
            string methodName, 
            Dictionary<string, object> context = null);

        Task LogErrorAsync(
            string message, 
            string className,
            string methodName, 
            Dictionary<string, object> context = null);

        Task LogWarningAsync(
            string message,
            string className,
            string methodName, 
            Dictionary<string, object> context = null);

        Task LogTraceAsync(
            string message,
            string className,
            string methodName, 
            Dictionary<string, object> context = null);
    }
}
