namespace Blogic.Crm.Infrastructure.Extensions;

public static class DateTimeExtensions
{
	public static DateTime RemoveYears(DateTime value, int years)
	{
		years = years > 0 ? -years : years;
		return value.AddYears(years);
	}
}