using System;

namespace SocialNet.Backend.DataObjects
{
	public class Content : IdDataObject
	{
		public ContentType Type { get; set; }

		public string Text { get; set; }

        public string Html { get; set; }

        public string VideoUrl { get; set; }

        public string ImageUrl { get; set; }

        public string ImageSmallUrl { get; set; }

        public string ImageThumbnailUrl { get; set; }

        public Content()
        {
            Text = "";
            Html = "";
            VideoUrl = "";
            ImageUrl = "";
            ImageSmallUrl = "";
            ImageThumbnailUrl = "";
        }

        public Content Clone()
		{
			return new Content
			{
				Id = this.Id,
				Created = this.Created,
				Updated = this.Updated,
				Type = this.Type,
				Text = this.Text,
			};
		}
	}
}
