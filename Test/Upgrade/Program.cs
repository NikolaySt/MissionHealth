using SocialNet.Backend.Catalogs;
using SocialNet.Backend.Configuration;
using SocialNet.Backend.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrade
{
	class Program
	{
		static void Main(string[] args)
		{
			Init();
			Console.WriteLine("Press [enter] to run");
			var key = Console.ReadKey();

			if (key.Key == ConsoleKey.Enter)
			{
				ScriptProcess();
			}
			else
			{
				Console.WriteLine("Customer Abort");
			}

			Console.ReadLine();
		}

		static async Task ScriptProcess()
		{
			Console.WriteLine("Process...");
			//await Script_1_0.Run();
			await Script_1_1.Run();

			Console.WriteLine("It's done");
		}


		static void Init()
		{

			SystemSettings.Load(AppDomain.CurrentDomain.BaseDirectory);

			var settings = new MongoProviderSettings
			{
				ConnectionString = SystemSettings.DatabaseConnectionString,
				DatabaseName = SystemSettings.DatabaseName,
				CollectionNamespace = SystemSettings.DatabaseCollectionNamespace,
				ConnectionTimeout = SystemSettings.ConnectionTimeout,
				ConnectionPoolSize = SystemSettings.ConnectionPoolSize,
				WaitQueueSize = SystemSettings.WaitQueueSize
			};

			MongoProvider.InitDefaultInstance(settings);

			DatabaseInitializer.Init();
		}
	}
}
