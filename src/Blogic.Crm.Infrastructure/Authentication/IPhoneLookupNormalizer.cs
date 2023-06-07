namespace Blogic.Crm.Infrastructure.Authentication;

/// <summary>
///     An abstraction of the phone number lookup normalization provider used for database entity indexes.
/// </summary>
public interface IPhoneLookupNormalizer
{
	/// <summary>
	///     Normalizes phone number.
	/// </summary>
	/// <param name="phone">Phone number to be normalized.</param>
	/// <returns>Normalized phone number.</returns>
	string? Normalize(string? phone);
}