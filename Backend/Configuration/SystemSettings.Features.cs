using SocialNet.Backend.DataObjects;

namespace SocialNet.Backend.Configuration
{
    public partial class SystemSettings
    {
		public static long TokenLifeTime { get; set; }
		public static ResetPasswordFeature ResetPasswordFeature { get; set; }
		public static EncryptPasswordFeature EncryptPasswordFeature { get; set; }
		public static ConfirmEmailFeature ConfirmEmailFeature { get; set; }
		public static DownloadBookFeature DownloadBookFeature { get; set; }
		
	}
}
