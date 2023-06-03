using Blogic.Crm.Infrastructure.Sorting;

namespace Blogic.Crm.Infrastructure.Pagination;

public sealed class ClientQueryString : QueryStringBase
{
	public ClientQueryString()
	{
		SortOrder = ClientsSortOrder.FamilyName;
		MinDateBorn = DateTime.MinValue;
		MaxDateBorn =  DateTime.MaxValue;
	}

	public ClientQueryString(int pageSize, int pageNumber, string searchString,
	                                   ClientsSortOrder sortOrder,
	                                   DateTime minDateBorn,
	                                   DateTime maxDateBorn) : base(pageSize, pageNumber, searchString)
	{
		SortOrder = sortOrder;
		if (maxDateBorn >= minDateBorn)
		{
			MinDateBorn = minDateBorn;
			MaxDateBorn = maxDateBorn;
		}
		else
		{
			MinDateBorn = maxDateBorn;
			MaxDateBorn =  minDateBorn;
		}
	}

	public const ClientsSortOrder DefaultSortOrder = ClientsSortOrder.FamilyName;

	public ClientsSortOrder SortOrder { get; set; }
	public DateTime MinDateBorn { get; set; }
	public DateTime MaxDateBorn { get; set; }

	public bool IsValidDateBorn => MaxDateBorn >= MinDateBorn;
}

