namespace Blogic.Crm.Infrastructure.Authentication;

/// <summary>
///     An abstraction of a password hashing intermediary.
/// </summary>
public interface IPasswordHasher
{
	/// <summary>
	///     Encrypts passwords with randomly generated salt.
	/// </summary>
	/// <param name="password">Password to be encrypted.</param>
	/// <returns>Encrypted password.</returns>
	string HashPassword(ReadOnlySpan<char> password);

	/// <summary>
	///     Validates the password against its hash.
	/// </summary>
	/// <param name="password">The password to be verified.</param>
	/// <param name="passwordHash">The password hash against which the password will be validated.</param>
	/// <returns>Success when the password matches its encrypted counterpart, otherwise false.</returns>
	PasswordVerificationResult VerifyPassword(ReadOnlySpan<char> password, ReadOnlySpan<char> passwordHash);
}