using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using SocialNet.Backend.Configuration;

namespace SocialNet.WebSite.Areas.Admin.Controllers
{
	public class GalleryController : AdminSuperController
	{

		public ActionResult Index(string id)
		{
			var serverDirPath = Server.MapPath(SystemSettings.PhotoContainer);

			var serverfilesPath = Server.MapPath(SystemSettings.PhotoContainer + id);

			string[] files = Directory.GetFiles(serverfilesPath, "*.*", SearchOption.TopDirectoryOnly).Select(it =>
			{
				var position = it.IndexOf("content");
				return string.Concat("~/", it.Substring(position, it.Length - position).Replace("\\", "/"));
			}).ToArray();

			ViewBag.Directories = GetDirectories(new DirectoryInfo(serverDirPath));

			ViewBag.SubDir = id;

			return View(files);
		}

		private List<string> GetDirectories(DirectoryInfo directory)
		{
			var result = new List<string>();

			if (directory == null) return result;

			DirectoryInfo[] SubDirectories = directory.GetDirectories();

			foreach (var item in SubDirectories)
			{
				result.Add(item.Name);
			}

			return result;
		}

		public ActionResult NewGallery(string id)
		{
			var serverPath = Server.MapPath(SystemSettings.PhotoContainer + id);
			if (!Directory.Exists(serverPath))
			{
				Directory.CreateDirectory(serverPath);
			}

			return RedirectToAction("Index", new { id });
		}

		[HttpPost]
		public async Task<ActionResult> Upload(string id)
		{
			var path = Server.MapPath(SystemSettings.PhotoContainer + id);
			foreach (var fileKey in Request.Files.AllKeys)
			{
				var file = Request.Files[fileKey];
				try
				{
					if (file != null)
					{
						var memoryStream = new MemoryStream();
						file.InputStream.CopyTo(memoryStream);

						var fileStream = new FileStream(path + "\\" + file.FileName, FileMode.OpenOrCreate);
						try
						{
							memoryStream.Position = 0;
                            memoryStream.CopyTo(fileStream);
                        }
						finally
						{
							fileStream.Close();
							fileStream.Dispose();
						}
					}
				}
				catch (Exception ex)
				{
					return Json(new { ex.Message });
				}
			}
			return Json(new { Message = "File was successfully saved" });
		}

		[HttpGet]
		public ActionResult Delete(string id)
		{
			var serverFile = Server.MapPath(id);
			try
			{
				System.IO.File.Delete(serverFile);
				return Content("File was successfully saved");
			}
			catch(Exception ex)
			{
				return Content(ex.Message);
			}			
		}
	}
}