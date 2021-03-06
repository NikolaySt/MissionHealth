using Newtonsoft.Json;
using SocialNet.Backend.Catalogs;
using SocialNet.Backend.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SocialNet.WebSite.Controllers
{
	public class CaptchaResponse
	{
		[JsonProperty("success")]
		public bool Success { get; set; }

		[JsonProperty("error-codes")]
		public List<string> ErrorCodes { get; set; }
	}

	public class ContactController : SuperController
	{
		public ActionResult Index()
		{
			ViewBag.Success = TempData["Success"] ?? null;
			ViewBag.Message = TempData["Message"] ?? null;

			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public async Task<ActionResult> Index(ContactViewModel model)
		{
			try
			{
				if (!TryValidateModel(model))
				{
					throw new ArgumentException(string.Join(". ", ModelState.Values
														.SelectMany(x => x.Errors)
														.Select(x => x.ErrorMessage)));
				}

				ValidateCaptcha();

				var messageCatalog = new MessageCatalog();

				await messageCatalog.StoreAsync(new Message
				{
					Name = model.Name,
					EMail = model.EMail,
					Text = model.Text
				});

				TempData["Success"] = true;
				TempData["Message"] = "Съобщението беше изпратено успешно";

				return RedirectToAction("Index");
			}
			catch (Exception exception)
			{
				ViewBag.Success = false;
				ViewBag.Message = exception.Message;
			}			

			return View(model);
		}

		public void ValidateCaptcha()
		{
			var recaptcha = Request["g-recaptcha-response"];
			//secret that was generated in key value pair
			const string secret = "6LcUMSATAAAAAOtcpOsS-3_9BRqMFhKY5rE6PW91";

			var client = new WebClient();
			var response =
				client.DownloadString(
					string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, recaptcha));

			var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(response);

			//when response is false check for the error message
			if (!captchaResponse.Success)
			{
				if (captchaResponse.ErrorCodes.Count <= 0) return;

				var error = captchaResponse.ErrorCodes[0].ToLower();
				switch (error)
				{
					case ("missing-input-secret"):
						throw new ArgumentException("Моля отбележете, че не сте робот.");
					case ("invalid-input-secret"):
						throw new ArgumentException("Кодът е невалиден. Моля опитайте отново.");
					case ("missing-input-response"):
						throw new ArgumentException("Кодът е невалиден. Моля опитайте отново.");
					case ("invalid-input-response"):
						throw new ArgumentException("Кодът е невалиден. Моля опитайте отново.");
					default:
						throw new ArgumentException("Кодът е невалиден. Моля опитайте отново.");

				}
			}
		}
	}
}