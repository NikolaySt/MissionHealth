using System;
using System.Linq;
using System.Text;

namespace SocialNet.Backend.DataObjects
{
    public class Log : IdDataObject
	{
        private const string Template =
           "Level = {0}\r\n"
           + "Component Name = {1}\r\n"
           + "Component Version = {2}\r\n"
           + "Class Name = {3}\r\n"
           + "Method Name = {4}\r\n"
           + "App Realm Id = {5}\r\n"
           + "App Id = {6}\r\n"
           + "User Id = {7}\r\n"
           + "Token Id = {8}\r\n"
           + "Message = {9}\r\n"
           + "Status = {10}\r\n"
           + "Exception = {11}\r\n"
           + "Inner Exception = {12}\r\n"
           + "Context \r\n"
           + "=============================== \r\n"
           + "{13}";

        public LogLevel Level { get; set; }

        public string ComponentName { get; set; }

        public string ComponentVersion { get; set; }

        public string ClassName { get; set; }

        public string MethodName { get; set; }

        public string AppRealmId { get; set; }

        public string UserId { get; set; }

        public string TokenId { get; set; }

        public string Message { get; set; }

        public LogException Exception { get; set; }

        public LogException InnerException { get; set; }

        public LogStatus Status { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var item in Context.Where(item => item.Value != null))
            {
                builder.AppendFormat("{0} = {1}\r\n", item.Key, item.Value);
            }

            return String.Format(
                Template ?? "",
                Level,
                ComponentName ?? "",
                ComponentVersion ?? "",
                ClassName ?? "",
                MethodName ?? "",
                AppRealmId ?? "",
                AppId ?? "",
                UserId ?? "",
                Message ?? "",
                Status ,
                Exception != null ? Exception.ToString() : "",
                InnerException != null ? InnerException.ToString() : "",
                builder.ToString(),"", "");
        }
    }
}
