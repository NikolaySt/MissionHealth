using System;
using System.Linq;
using System.Web.Mvc;

namespace SocialNet.Website
{
    public enum MessageType
    {
        Success,
        Warning,
        Info,
        Error
    }

    public class Message
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public MessageType Type { get; set; }

        protected Message(string text, string title, MessageType type)
        {
            Text = text;
            Type = type;
            Title = title;
        }

        public static Message Success(string text, string title = "")
        {
            return new Message(text, title, MessageType.Success);
        }

        public static Message Warning(string text, string title = "")
        {
            return new Message(text, title, MessageType.Warning);
        }

        public static Message Info(string text, string title = "")
        {
            return new Message(text, title, MessageType.Info);
        }

        public static Message Error(string text, string title = "")
        {
            return new Message(text, title, MessageType.Error);
        }

        public static Message Error(Exception exception, string title = "")
        {
            var message = exception.Message;
            
            if (exception.InnerException != null)
            {
                message = message + ". " +exception.InnerException.Message;
            }
            return new Message(message, title, MessageType.Error);
        }

        public static Message Info(ModelStateDictionary modelState, string title = "")
        {
            var message = string.Join(". ", modelState.Values
                                                    .SelectMany(x => x.Errors)
                                                    .Select(x => x.ErrorMessage));
            return new Message(message, title, MessageType.Error);
        }

        public static Message Error(ModelStateDictionary modelState, string title = "")
        {            
            var message = string.Join(" ", modelState.Values
                                                    .SelectMany(x => x.Errors)
                                                    .Select(x => x.ErrorMessage));
            return new Message(message, title, MessageType.Error);
        }
    }   
}
