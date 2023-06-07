using System.Diagnostics.CodeAnalysis;

namespace Blogic.Crm.Infrastructure.Authentication;

/// <summary>
///     Email search normalizer provider with top variant.
/// </summary>
public sealed class EmailLookupNormalizer : IEmailLookupNormalizer
{
    public string? Normalize([NotNullIfNotNull(nameof(email))] string? email)
    {
        return email == null ? email : email.Normalize().ToUpperInvariant();
    }
}