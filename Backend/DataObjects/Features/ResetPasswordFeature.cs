namespace SocialNet.Backend.DataObjects
{
	public class ResetPasswordFeature : Feature
	{
		public MailAccountSettings MailAccountSettings { get; set; }

        public ResetPasswordTemplatePageFeature ResetPasswordTemplatePageFeature { get; set; }

        public string MessageFrom { get; set; }

		public string MessageSubject { get; set; }

		public string MessageBody { get; set; }

		public bool IsMessageBodyHtml { get; set; }

		public int ResetLinkLifeTime { get; set; }             

        public string SuccessRedirectUrl { get; set; }

        public string FailureRedirectUrl { get; set; }
    }
}
