namespace Blogic.Crm.Infrastructure.TypeExtensions;

public static class GuidExtensions
{
    public static bool IsValidRegistrationNumber(string registrationNumber)
    {
        return Guid.TryParse(registrationNumber, out _);
    }
}