using SocialNet.Backend.Catalogs;
using SocialNet.Backend.DataFilters;
using SocialNet.Backend.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Upgrade
{
	public class Script_1_1
	{
		public static async Task Run()
		{
			//await Script_Failed_Mails();
			//await Script_Test();
		}

		private static async Task Script_Failed_Mails()
		{
			//DISABLED
			//return;

			var userCatalog = new UserCatalog();

			var users = await userCatalog.GetAsync(new UserListFilter() { });

			//var users  = await userCatalog.GetAsync(new UserListFilter() { BookDownloads = 0 } );

			var manager = new MailManager();

			foreach (var user in users)
			{
				try
				{
					await manager.SendBookAsync(user.Email, user.FirstName, "69f64e0b-ea65-4811-94b9-19a995b972bc", user.Id, "Наръчник за балансиране на хормоните");

					Console.WriteLine($"[success {user.Email}]");

					Thread.Sleep(300);
				}
				catch (Exception exception)
				{
					Console.WriteLine($"[failed {user.Email}]");
					Console.WriteLine($"{exception.Message}");

				}
			}
		}

		private static async Task Script_Test()
		{

			//var users  = await userCatalog.GetAsync(new UserListFilter() { BookDownloads = 0 } );
			var manager = new MailManager();

			try
			{
				await manager.SendBookAsync("", "", "69f64e0b-ea65-4811-94b9-19a995b972bc", "userId", "Наръчник за балансиране на хормоните");

				Console.WriteLine($"[success]");

			}
			catch (Exception exception)
			{
				Console.WriteLine($"[failed]");
				Console.WriteLine($"{exception.Message}");
				Console.WriteLine($"{exception.InnerException?.Message}");

			}

		}
	}
}
