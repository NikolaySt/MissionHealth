using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SystemConfiguration = System.Configuration.Configuration;


namespace SocialNet.Backend.Configuration
{
	public partial class SystemSettings
	{
		private static SystemConfiguration _common;
		private static SystemConfiguration _environment;

		private static string _secureToken;
		public static string SecureToken
		{
			get
			{
				return _secureToken;
			}
		}		

		private static string _photoContainer;
        private static string _thumbnailContainer;
        private static string _videoContainer;

		private static string _photoUri;
		private static string _videoUri;
        private static string _thumbnailUri;
		private static string _facebookPhotoUri;
		
		private static ReadOnlyCollection<string> _logger;


		public static IEnumerable<string> Logger
        {
            get { return _logger; }
        }
        
        public static string PhotoContainer
        {
            get { return _photoContainer; }
        }

        public static string VideoContainer
        {
            get { return _videoContainer; }
        }

        public static string ThumbnailContainer
        {
            get { return _thumbnailContainer; }
        }
        
		public static string PhotoUri
		{
			get { return _photoUri; }
		}


		public static string VideoUri
		{
			get { return _videoUri; }
		}

        public static string ThumbnailUri
		{
            get { return _thumbnailUri; }
		}

		public static string FacebookPhotoUri
		{
			get { return _facebookPhotoUri; }
		}
		

		private static List<string> GetList(string setting)
		{
			if (string.IsNullOrWhiteSpace(setting)) return new List<string>();

			return setting.Split(',').ToList().Select(it => it.Trim()).ToList();
		}

		private static Dictionary<string, string> GetMappings(string setting)
		{
			var results = new Dictionary<string, string>();
			if (string.IsNullOrWhiteSpace(setting)) return results;

			var mappings = setting.Split(',');
			foreach (var mapping in mappings)
			{
				var periods = mapping.Split(':');
				if (periods.Length < 2) continue;

				results.Add(periods[0].Trim(), periods[1].Trim());
			}

			return results;
		}

		private static string GetEnvironmentConfigPath()
		{
#if LOCAL
			return @"Config\Local.config";
#elif DEBUG
			return @"Config\Debug.config";
#else
			return @"Config\Release.config";
#endif
		}
	}
}
