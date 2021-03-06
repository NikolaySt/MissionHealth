using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SocialNet.WebSite.Areas.Book.Models
{
    public class BookRequestViewModel
    {
        [Required(ErrorMessage = "Моля въведете вашият Email.")]
        [Display(Name = @"Вашият Email")]
        [EmailAddress(ErrorMessage = "Текста не е коректен Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Моля въведете вашето име.")]
        [Display(Name = @"Вашето име")]
        public string Name { get; set; }

        [Required]
        [HiddenInput(DisplayValue = false)]
        public string BookId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string UserId { get; set; }
    }
}