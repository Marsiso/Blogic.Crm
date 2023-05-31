namespace Blogic.Crm.Infrastructure.Authentication;

public sealed class LookupNormalizer : ILookupNormalizer
{
	public string? Normalize(string? value)
	{
		return value == null ? value : value.Normalize().ToUpperInvariant();
	}
}