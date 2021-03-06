namespace SocialNet.Backend.DataFilters
{
	public partial class MessageListFilter : SuperListFilter
	{
        public string EMail { get; set; }

        public string Name { get; set; }

		public bool? Seen { get; set; }
	}
}
