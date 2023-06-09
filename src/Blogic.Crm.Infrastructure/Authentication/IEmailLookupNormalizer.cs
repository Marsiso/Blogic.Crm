using System.Diagnostics.CodeAnalysis;

namespace Blogic.Crm.Infrastructure.Authentication;

/// <summary>
///     An abstraction of the email address lookup normalization provider used for database entity indexes.
/// </summary>
public interface IEmailLookupNormalizer
{
	/// <summary>
	///     Normalizes email address.
	/// </summary>
	/// <param name="email">An email address to be normalized.</param>
	/// <returns>A normalized email address.</returns>
	string? Normalize([NotNullIfNotNull(nameof(email))] string? email);
}