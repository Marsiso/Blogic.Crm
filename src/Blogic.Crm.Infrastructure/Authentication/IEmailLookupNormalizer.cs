using System.Diagnostics.CodeAnalysis;

namespace Blogic.Crm.Infrastructure.Authentication;

public interface IEmailLookupNormalizer
{
	string? Normalize([NotNullIfNotNull(nameof(email))] string? email);
}