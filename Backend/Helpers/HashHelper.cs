using System;
using System.Security.Cryptography;

namespace SocialNet.Backend.Helpers
{
	/// <summary>
	/// Salted password hashing with PBKDF2-SHA1.
	/// Author: havoc AT defuse.ca
	/// www: http://crackstation.net/hashing-security.htm
	/// Compatibility: .NET 3.0 and later.
	/// </summary>
	public class HashHelper
	{
		// The following constants may be changed without breaking existing hashes.
		public const int SALT_BYTES = 24;
		public const int HASH_BYTES = 24;
		public const int PBKDF2_ITERATIONS = 1000;

		public const int ITERATION_INDEX = 0;
		public const int SALT_INDEX = 1;
		public const int PBKDF2_INDEX = 2;

		/// <summary>
		/// Creates a salted PBKDF2 hash of the password, using input salt string.
		/// </summary>
		/// <param name="password">The password to hash.</param>
		/// <returns>The hash of the password.</returns>
		public static string CreateHash(string password, string salt)
		{
			if (password == null)
			{
				return null;
			}

			// Generate a random salt
			//RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider();

			//byte[] saltBytes = new byte[SALT_BYTES];
			//csprng.GetBytes(saltBytes);

			byte[] saltBytes = System.Text.Encoding.Unicode.GetBytes(salt);
			//byte[] saltBytes = Convert.FromBase64String(salt);

			// Hash the password and encode the parameters
			byte[] hash = PBKDF2(password, saltBytes, PBKDF2_ITERATIONS, HASH_BYTES);

			return Convert.ToBase64String(hash);
		}

		/// <summary>
		/// Creates a salted PBKDF2 hash of the password.
		/// </summary>
		/// <param name="password">The password to hash.</param>
		/// <returns>The hash of the password.</returns>
		public static string CreateHash(string password, out string salt)
		{
			if (password == null)
			{
				salt = null;
				return null;
			}

			// Generate a random salt
			RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider();

			byte[] saltBytes = new byte[SALT_BYTES];
			csprng.GetBytes(saltBytes);

			// Hash the password and encode the parameters
			byte[] hash = PBKDF2(password, saltBytes, PBKDF2_ITERATIONS, HASH_BYTES);

			salt = Convert.ToBase64String(saltBytes);

			return Convert.ToBase64String(hash);
		}

		/// <summary>
		/// Validates a password given a hash of the correct one.
		/// </summary>
		/// <param name="password">The password to check.</param>
		/// <param name="goodHash">A hash of the correct password.</param>
		/// <returns>True if the password is correct. False otherwise.</returns>
		public static bool ValidatePassword(string password, string goodHash)
		{
			// Extract the parameters from the hash
			char[] delimiter = { ':' };
			string[] split = goodHash.Split(delimiter);
			int iterations = Int32.Parse(split[ITERATION_INDEX]);
			byte[] salt = Convert.FromBase64String(split[SALT_INDEX]);
			byte[] hash = Convert.FromBase64String(split[PBKDF2_INDEX]);

			byte[] testHash = PBKDF2(password, salt, iterations, hash.Length);
			return SlowEquals(hash, testHash);
		}

		/// <summary>
		/// Compares two byte arrays in length-constant time. This comparison
		/// method is used so that password hashes cannot be extracted from
		/// on-line systems using a timing attack and then attacked off-line.
		/// </summary>
		/// <param name="a">The first byte array.</param>
		/// <param name="b">The second byte array.</param>
		/// <returns>True if both byte arrays are equal. False otherwise.</returns>
		private static bool SlowEquals(byte[] a, byte[] b)
		{
			uint diff = (uint)a.Length ^ (uint)b.Length;
			for (int i = 0; i < a.Length && i < b.Length; i++)
				diff |= (uint)(a[i] ^ b[i]);
			return diff == 0;
		}

		/// <summary>
		/// Computes the PBKDF2-SHA1 hash of a password.
		/// </summary>
		/// <param name="password">The password to hash.</param>
		/// <param name="salt">The salt.</param>
		/// <param name="iterations">The PBKDF2 iteration count.</param>
		/// <param name="outputBytes">The length of the hash to generate, in bytes.</param>
		/// <returns>A hash of the password.</returns>
		private static byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
		{
			Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt);
			pbkdf2.IterationCount = iterations;
			return pbkdf2.GetBytes(outputBytes);
		}

		public static int GetRandomNumber(int min, int max)
		{
			var rng = new RNGCryptoServiceProvider();
			var buffer = new byte[32];

			rng.GetBytes(buffer);
			int result = BitConverter.ToInt32(buffer, 0);

			return new Random(result).Next(min, max);
		}
	}
}
