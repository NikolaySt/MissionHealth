using System;
using System.Threading.Tasks;

using SocialNet.Backend.Passwords;
using SocialNet.Backend.Configuration;

namespace SocialNet.Backend.Managers
{
	public static class PasswordProcessorFactory
	{
		public static async Task<IPasswordProcessor> GetInstance()
		{

			var settings = SystemSettings.EncryptPasswordFeature;
			if (settings == null) throw new ApplicationException("Encryption password feature not configured");

			var pbkdf2ProcessorSettings = new Pbkdf2ProcessorSettings
			{
				Iterations = settings.Iterations,
				SaltSize = settings.SaltSize,
				HashSize = settings.HashSize
			};

			return new Pbkdf2Processor(pbkdf2ProcessorSettings);
		}
	}
}
