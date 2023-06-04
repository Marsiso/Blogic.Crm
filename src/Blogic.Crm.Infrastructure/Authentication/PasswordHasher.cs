using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Blogic.Crm.Infrastructure.Authentication;

/// <summary>
/// Password hashing provider that uses PBKDF2 key derivation and randomly generated salt. 
/// </summary>
public sealed class PasswordHasher : IPasswordHasher
{
	private const int KeySize = 32;
	private const int SaltSize = 16;
	private const int Cycles = 1_572_864;
	private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;
	
	public string HashPassword(ReadOnlySpan<char> password)
	{
		Debug.Assert(!password.IsEmpty);
		Debug.Assert(!password.IsWhiteSpace());
		
		// Encode the provided password. 
		Span<byte> passwordBytes = stackalloc byte[password.Length];
		Encoding.UTF8.GetBytes(password, passwordBytes);

		// Generate random salt.
		Span<byte> salt = stackalloc byte[SaltSize];
		RandomNumberGenerator.Fill(salt);

		// Derive key from the encoded password and salt.
		Span<byte> key = stackalloc byte[KeySize];
		Rfc2898DeriveBytes.Pbkdf2(passwordBytes, salt, key, Cycles, Algorithm);

		// Return derived key and salt separated by the delimiter.
		return $"{Convert.ToBase64String(key)};{Convert.ToBase64String(salt)}";
	}

	public PasswordVerificationResult VerifyPassword(ReadOnlySpan<char> password, ReadOnlySpan<char> passwordHash)
	{
		Debug.Assert(!password.IsEmpty);
		Debug.Assert(!password.IsWhiteSpace());
		Debug.Assert(!passwordHash.IsEmpty);
		Debug.Assert(!passwordHash.IsWhiteSpace());
		
		// Encode the provided password. 
		Span<byte> passwordBytes = stackalloc byte[password.Length];
		Encoding.UTF8.GetBytes(password, passwordBytes);

		// Separate derived key and salt using the delimiter.
		var delimiter = passwordHash.IndexOf(';');
		if (delimiter == -1)
		{
			throw new FormatException("Password hash has no delimiter");
		}

		var passwordHashKey = Convert.FromBase64String(passwordHash[..delimiter++].ToString());
		var passwordHashSalt = Convert.FromBase64String(passwordHash[delimiter..].ToString());

		// Derive key using the encoded password and salt. 
		Span<byte> key = stackalloc byte[KeySize];
		Rfc2898DeriveBytes.Pbkdf2(passwordBytes, passwordHashSalt, key, Cycles, Algorithm);

		// Compare derived key with the derived key contained within password hash in fixed time interval.
		return CryptographicOperations.FixedTimeEquals(key, passwordHashKey)
			? PasswordVerificationResult.Success
			: PasswordVerificationResult.Fail;
	}
}