using SocialNet.Backend.DataObjects;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SocialNet.WebSite
{
	public class ContactViewModel
	{
		[Required(AllowEmptyStrings = false, ErrorMessage = "Името е задължително поле")]
		[Display(Name = @"Име")]
		public string Name { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "EMail е задължително поле")]
		[Display(Name = @"EMail")]
		[EmailAddress]
		public string EMail { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "Моля, въвеведе текст.")]
		[Display(Name = @"Текст")]
		public string Text { get; set; }
	}
}