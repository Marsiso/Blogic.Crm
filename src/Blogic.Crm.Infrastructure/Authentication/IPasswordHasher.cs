namespace Blogic.Crm.Infrastructure.Authentication;

public interface IPasswordHasher
{
	string HashPassword(ReadOnlySpan<char> password);
	PasswordVerificationResult VerifyPassword(ReadOnlySpan<char> password, ReadOnlySpan<char> passwordHash);
}