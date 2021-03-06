using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using SocialNet.Backend.DataObjects;
using System.Collections.Generic;

namespace SocialNet.WebSite.Areas.Admin.Models
{
    public class ArticleViewModel
    {
        [HiddenInput]
        public string Id { get; set; }

		[Required]
        public string Title { get; set; }

        [AllowHtml]
        [Required]
        public string HtmlRaw { get; set; }

		[Required]
		public string Text { get; set; }

		[Display(Name = @"Кратко представяне на статията")]
		public string Review { get; set; }

		[Display(Name = @"Ключови думи")]
		public string Keywords { get; set; }

		[Required]
		public string AreaValue { get; set; }
		
		[Required]
		public string CategoryValue { get; set; }

        [Required]
		public ContentType TitleType { get; set; }

        [Display(Name = @"Интересна")]
        public bool Interesting { get; set; }

        [Display(Name = @"Водеща")]
        public bool Top { get; set; }        

        public string VideoUrl { get; set; }

        public string ImageUrl { get; set; }

        public string ImageSmallUrl { get; set; }

        public string ImageThumbnailUrl { get; set; }

        [HiddenInput]
        public List<SelectListItem> Categories { get; set; }

        [HiddenInput]
        public List<SelectListItem> Areas { get; set; }

    }
    
}

