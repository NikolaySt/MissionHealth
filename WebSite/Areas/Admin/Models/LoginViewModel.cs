using System.ComponentModel.DataAnnotations;

namespace SocialNet.WebSite.Areas.Admin.Models
{
    public class LoginViewModel
    {
		public string ReturnUrl { get; set; }		

		[Required]
        [Display(Name = @"Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = @"Password")]
        public string Password { get; set; }

        [Display(Name = @"Remember me?")]
        public bool RememberMe { get; set; }
    }
}
