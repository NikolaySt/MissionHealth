namespace SocialNet.Backend.DataObjects
{
	public class DownloadBookFeature : Feature
	{
		public MailAccountSettings MailAccountSettings { get; set; }

		public string MessageFrom { get; set; }

		public string MessageSubject { get; set; }

		public string MessageBody { get; set; }
    }
}
