
namespace SocialNet.Backend.DataObjects
{
    public class Message : IdDataObject
    {
        public Message()
        {
        }

		public bool Seen { get; set; }

		public string Name { get; set; }

        public string EMail { get; set; }

		public string Text { get; set; }
	}
}
