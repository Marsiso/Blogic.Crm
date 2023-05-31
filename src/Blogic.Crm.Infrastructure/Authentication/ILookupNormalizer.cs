namespace Blogic.Crm.Infrastructure.Authentication;

public interface ILookupNormalizer
{
	string? Normalize(string? value);
}