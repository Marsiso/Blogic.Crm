using Blogic.Crm.Domain.Data.Entities;
using Blogic.Crm.Infrastructure.Sorting;

namespace Blogic.Crm.Infrastructure.Pagination;

/// <summary>
/// Query string parameters used by the searching, filtering sorting related to the <see cref="Client"/> data set .
/// </summary>
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

	/// <summary>
	/// Used to sort the <see cref="Client"/> data set.
	/// </summary>
	public ClientsSortOrder SortOrder { get; set; }
	
	/// <summary>
	/// Used for the <see cref="Client"/> data set filtering.
	/// </summary>
	public DateTime MinDateBorn { get; set; }
	
	/// <summary>
	/// Used for the <see cref="Client"/> data set filtering.
	/// </summary>
	public DateTime MaxDateBorn { get; set; }

	/// <summary>
	/// Validation for the minimal and maximal date born.
	/// </summary>
	public bool IsValidDateBorn => MaxDateBorn >= MinDateBorn;
}

