using PhoneNumbers;

namespace Blogic.Crm.Infrastructure.Authentication;

/// <summary>
///     Telephone number normalizer provider.
/// </summary>
public sealed class PhoneLookupNormalizer : IPhoneLookupNormalizer
{
    public string? Normalize(string? phone)
    {
        return phone == null ? phone : PhoneNumberUtil.Normalize(phone);
    }
}