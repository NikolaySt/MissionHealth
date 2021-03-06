namespace SocialNet.Backend.DataObjects
{
	public class EncryptPasswordFeature : Feature
	{
		public int Iterations { get; set; }
		public int SaltSize { get; set; }
		public int HashSize { get; set; }
	}
}
