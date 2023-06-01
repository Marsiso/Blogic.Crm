using System.Diagnostics.CodeAnalysis;

namespace Blogic.Crm.Infrastructure.Authentication;

public sealed class EmailLookupNormalizer : IEmailLookupNormalizer
{
	public string? Normalize([NotNullIfNotNull(nameof(email))] string? email)
	{
		return email == null ? email : email.Normalize().ToUpperInvariant();
	}
}