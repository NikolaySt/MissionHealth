using System.Collections.Generic;

namespace SocialNet.Backend.DataObjects
{
    public class Article: IdDataObject
    {
		public string TitleId { get; set; }

		public Content Title { get; set; }

		public string Review { get; set; }

		public string Author { get; set; }

        public Content Content { get; set; }

        public string Type { get; set; }

        public ArticleCategory Category { get; set; }

        public List<string> Keywords { get; set; }    
        
        public SocialNetwork SocialNetwork {get; set;}

        public bool Published { get; set; }

        public bool Interesting { get; set; }

        public bool Top { get; set; }

        public Article()
		{
			SocialNetwork = new SocialNetwork();
			Category = new ArticleCategory();
            Keywords = new List<string>();
			Content = new Content();
			Title = new Content();
		}
    }
}
