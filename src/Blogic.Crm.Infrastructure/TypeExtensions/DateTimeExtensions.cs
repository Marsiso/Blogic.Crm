namespace Blogic.Crm.Infrastructure.TypeExtensions;

public static class DateTimeExtensions
{
    private static DateTime RemoveYears(DateTime value, int years)
    {
        years = years > 0 ? -years : years;
        return value.AddYears(years);
    }

    public static bool IsLegalAge(DateTime value, int legalAge)
    {
        var utcNow = DateTime.UtcNow;
        var legalAgeLowerBoundary = RemoveYears(utcNow, legalAge);
        return DateTime.Compare(value, legalAgeLowerBoundary) < 0;
    }
}