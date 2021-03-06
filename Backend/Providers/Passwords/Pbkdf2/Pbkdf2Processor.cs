using System;
using System.Security.Cryptography;

namespace SocialNet.Backend.Passwords
{
	public class Pbkdf2Processor : IPasswordProcessor
	{
		private const char Delimiter = ':';
		public const int IterationIndex = 0;
		public const int SaltIndex = 1;
		public const int HashIndex = 2;

		private readonly Pbkdf2ProcessorSettings _settings;

		public Pbkdf2Processor(Pbkdf2ProcessorSettings settings)
		{
			if (settings == null) throw new ArgumentException("settings");

			_settings = settings;
		}

		public string Process(string password)
		{
			var salt = new byte[_settings.SaltSize];
			using (var saltGenerator = new RNGCryptoServiceProvider())
			{
				saltGenerator.GetBytes(salt);
			}

			byte[] hash;
			using (var hashGenerator = new Rfc2898DeriveBytes(password, salt))
			{
				hashGenerator.IterationCount = _settings.Iterations;
				hash = hashGenerator.GetBytes(_settings.HashSize);
			}

			return String.Format(
				"{0}{1}{2}{3}{4}", 
				_settings.Iterations, 
				Delimiter,
				Convert.ToBase64String(salt), 
				Delimiter,
				Convert.ToBase64String(hash));
		}

		public bool Validate(string password, string output)
		{
			var values = output.Split(Delimiter);

			var iterations = Int32.Parse(values[IterationIndex]);
			var salt = Convert.FromBase64String(values[SaltIndex]);
			var hash = Convert.FromBase64String(values[HashIndex]);

			byte[] passwordHash;
			using (var hashGenerator = new Rfc2898DeriveBytes(password, salt))
			{
				hashGenerator.IterationCount = iterations;
				passwordHash = hashGenerator.GetBytes(_settings.HashSize);
			}

			return Compare(hash, passwordHash);
		}

		private static bool Compare(byte[] a, byte[] b)
		{
			var diff = (uint)a.Length ^ (uint)b.Length;
			for (var i = 0; i < a.Length && i < b.Length; i++)
				diff |= (uint)(a[i] ^ b[i]);
			return diff == 0;
		}
	}
}
