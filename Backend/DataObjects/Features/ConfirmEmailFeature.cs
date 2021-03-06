namespace SocialNet.Backend.DataObjects
{
	public class ConfirmEmailFeature : Feature
	{
		public MailAccountSettings MailAccountSettings { get; set; }

		public string MessageFrom { get; set; }

		public string MessageSubject { get; set; }

		public string MessageBody { get; set; }

		public bool IsMessageBodyHtml { get; set; }

		public int ConfirmationLinkLifeTime { get; set; }

		public string FailureRedirectUrl { get; set; }

        public string SuccessRedirectUrl { get; set; }
    }
}
