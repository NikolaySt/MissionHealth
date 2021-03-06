using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;

using SocialNet.Backend.DataObjects;

namespace SocialNet.Backend.Configuration
{
	public partial class SystemSettings
	{
		private static readonly object _configurationLock = new object();

		public static void Load(string root)
		{
			lock (_configurationLock)
			{
				if (_common != null) return;

				var fileMap = new ConfigurationFileMap(Path.Combine(root, @"Config\Common.config"));
				_common = ConfigurationManager.OpenMappedMachineConfiguration(fileMap);

				fileMap = new ConfigurationFileMap(Path.Combine(root, GetEnvironmentConfigPath()));
				_environment = ConfigurationManager.OpenMappedMachineConfiguration(fileMap);


				_tokenIssuer = _common.AppSettings.Settings["TokenIssuer"].Value;
				_tokenAudience = _common.AppSettings.Settings["TokenAudience"].Value;

				_secureToken = _environment.AppSettings.Settings["SecureToken"].Value;

				var path = _environment.AppSettings.Settings["TokenAudiencePublicKeyPath"].Value;
				_tokenAudiencePublicKey = File.ReadAllText(Path.Combine(root, path));

				path = _environment.AppSettings.Settings["TokenAudiencePrivateKeyPath"].Value;
				_tokenAudiencePrivateKey = File.ReadAllText(Path.Combine(root, path));

				TokenLifeTime = long.Parse(_common.AppSettings.Settings["TokenLifeTime"].Value);

				//SocialNetworks
				_twitterKey = _environment.AppSettings.Settings["TwitterKey"].Value;
				_twitterSecret = _environment.AppSettings.Settings["TwitterSecret"].Value;
				_facebookKey = _environment.AppSettings.Settings["FacebookKey"].Value;
				_facebookSecret = _environment.AppSettings.Settings["FacebookSecret"].Value;

				_photoUri = _environment.AppSettings.Settings["PhotoUri"].Value;
				_videoUri = _environment.AppSettings.Settings["VideoUri"].Value;
				_thumbnailUri = _environment.AppSettings.Settings["ThumbnailUri"].Value;
				_photoContainer = _environment.AppSettings.Settings["PhotoContainer"].Value;
				_videoContainer = _environment.AppSettings.Settings["VideoContainer"].Value;


				_databaseConnectionString = _environment.AppSettings.Settings["DatabaseConnectionString"].Value;
				_databaseName = _environment.AppSettings.Settings["DatabaseName"].Value;
				_databaseCollectionNamespace = _environment.AppSettings.Settings["DatabaseCollectionNamespace"].Value;

				LoadFeatures(_common, _environment, root);

				var list = GetList(_environment.AppSettings.Settings["Logger"].Value);
				_logger = new ReadOnlyCollection<string>(list);

				int.TryParse(_common.AppSettings.Settings["ConnectionPoolSize"].Value, out _connectionPoolSize);
				int.TryParse(_common.AppSettings.Settings["ConnectionTimeout"].Value, out _connetionTimeout);
				int.TryParse(_common.AppSettings.Settings["WaitQueueSize"].Value, out _waitQueueSize);

			}
		}

		private static void LoadFeatures(
			System.Configuration.Configuration common,
			System.Configuration.Configuration environment,
			string root)
		{
			var path = "";
			// Reset password feature
			var resetPasswordSection = common.GetSection("resetPasswordSettings") as AppSettingsSection;
			

			// Encrypt password feature
			var encryptPasswordSection = common.GetSection("encryptPasswordSettings") as AppSettingsSection;
			if (encryptPasswordSection != null)
			{
				EncryptPasswordFeature = new EncryptPasswordFeature
				{
					Enabled = bool.Parse(encryptPasswordSection.Settings["Enabled"].Value),
					Iterations = int.Parse(encryptPasswordSection.Settings["Iterations"].Value),
					SaltSize = int.Parse(encryptPasswordSection.Settings["SaltSize"].Value),
					HashSize = int.Parse(encryptPasswordSection.Settings["HashSize"].Value)
				};
			}

			// Download email feature
			var downloadlSection = common.GetSection("downloadBookSettings") as AppSettingsSection;
			if (downloadlSection != null)
			{
				var mailSettings = new MailAccountSettings
				{
					Host = downloadlSection.Settings["Host"].Value,
					Port = int.Parse(downloadlSection.Settings["Port"].Value),
                    Account = downloadlSection.Settings["Account"].Value,
                    Password = downloadlSection.Settings["Password"].Value,
					Ssl = bool.Parse(downloadlSection.Settings["Ssl"]?.Value ?? "false"),					
				};

				DownloadBookFeature = new DownloadBookFeature
				{
					MailAccountSettings = mailSettings,
					Enabled = bool.Parse(downloadlSection.Settings["Enabled"].Value),
					MessageSubject = downloadlSection.Settings["MessageSubject"].Value,
					MessageFrom = downloadlSection.Settings["MessageFrom"].Value,
				};

				path = downloadlSection.Settings["MessageBodyPath"].Value;
				DownloadBookFeature.MessageBody = File.ReadAllText(Path.Combine(root, path));
			}
		}
	}
}

