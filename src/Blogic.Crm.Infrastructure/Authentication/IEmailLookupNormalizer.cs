using System.Diagnostics.CodeAnalysis;

namespace Blogic.Crm.Infrastructure.Authentication;

/// <summary>
/// Email address lookup normalizer provider abstraction used for the database entity indexes.
/// </summary>
public interface IEmailLookupNormalizer
{
	/// <summary>
	/// Normalizes email address.
	/// </summary>
	/// <param name="email">An email address to be normalized.</param>
	/// <returns>Normalized email address.</returns>
	string? Normalize([NotNullIfNotNull(nameof(email))] string? email);
}