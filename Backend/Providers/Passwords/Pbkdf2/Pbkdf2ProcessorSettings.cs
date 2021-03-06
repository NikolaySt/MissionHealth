namespace SocialNet.Backend.Passwords
{
	public class Pbkdf2ProcessorSettings
	{
		public int SaltSize { get; set; }
		public int HashSize { get; set; }
		public int Iterations { get; set; }
	}
}
