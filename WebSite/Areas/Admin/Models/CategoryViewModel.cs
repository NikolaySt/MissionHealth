using System.ComponentModel.DataAnnotations;

namespace SocialNet.WebSite.Areas.Admin.Models
{
    public class CategoryViewModel
	{
		[Required]
		public string CategoryName { get; set; }

		[Required]
		public string CategoryValue { get; set; }

		[Required]
		public string AreaName { get; set; }

		[Required]
		public string AreaValue { get; set; }
	}
    
}