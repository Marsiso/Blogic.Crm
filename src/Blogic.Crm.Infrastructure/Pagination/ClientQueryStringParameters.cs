using Blogic.Crm.Infrastructure.Sorting;

namespace Blogic.Crm.Infrastructure.Pagination;

public sealed record ClientQueryStringParameters : QueryStringParameters
{
	public ClientQueryStringParameters()
	{
		SortOrder = ClientsSortOrder.FamilyName;
		MinDateBorn = DateTime.MinValue;
		MaxDateBorn =  DateTime.MaxValue;
	}

	public ClientQueryStringParameters(int pageSize, int pageNumber, string searchString,
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

	public ClientsSortOrder SortOrder { get; private init; }
	public DateTime MinDateBorn { get; private init; }
	public DateTime MaxDateBorn { get; private init; }

	public bool IsValidDateBorn => MaxDateBorn >= MinDateBorn;
}

