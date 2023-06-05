namespace Blogic.Crm.Infrastructure.Authentication;

/// <summary>
///     Password hashing provider abstraction.
/// </summary>
public interface IPasswordHasher
{
	/// <summary>
	///     Hashes provided password.
	/// </summary>
	/// <param name="password">Provided password to be hashed.</param>
	/// <returns>Password's hash.</returns>
	string HashPassword(ReadOnlySpan<char> password);

	/// <summary>
	///     Compares password and its password hash.
	/// </summary>
	/// <param name="password">Provided password to be verified.</param>
	/// <param name="passwordHash">Provided password hash to be password verified against.</param>
	/// <returns>Success - If the password matches its hash. False - Otherwise.</returns>
	PasswordVerificationResult VerifyPassword(ReadOnlySpan<char> password, ReadOnlySpan<char> passwordHash);
}