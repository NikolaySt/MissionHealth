using System;

namespace SocialNet.Backend.DataObjects
{
    public class LogException
    {
        private const string Template =
            "Message = {0}\r\n"
            + "Source = {1}\r\n"
            + "Stack Trace = {2}\r\n"
            + "Type = {3}\r\n";

        public string Message { get; set; }

        public string Source { get; set; }

        public string StackTrace { get; set; }

        public string Type { get; set; }

        public override string ToString()
        {
            return String.Format(
                Template,
                Message ?? "",
                Source ?? "",
                StackTrace ?? "",
                Type ?? "");
        }
    }
}
