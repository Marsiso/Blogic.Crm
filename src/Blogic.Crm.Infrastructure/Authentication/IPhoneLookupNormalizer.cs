namespace Blogic.Crm.Infrastructure.Authentication;

/// <summary>
///     Phone number lookup normalizer provider abstraction used for the database entity indexes.
/// </summary>
public interface IPhoneLookupNormalizer
{
	/// <summary>
	///     Normalizes phone number.
	/// </summary>
	/// <param name="phone">Provided phone number to be normalized.</param>
	/// <returns>Normalized phone number.</returns>
	string? Normalize(string? phone);
}