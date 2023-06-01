namespace Blogic.Crm.Infrastructure.Authentication;

public interface IPhoneLookupNormalizer
{
	string? Normalize(string? phone);
}