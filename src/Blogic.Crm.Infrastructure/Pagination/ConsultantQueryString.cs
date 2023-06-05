using Blogic.Crm.Infrastructure.Sorting;

namespace Blogic.Crm.Infrastructure.Pagination;

/// <summary>
///     Query string parameters used by the searching, filtering sorting related to the <see cref="Consultant" /> data set
///     .
/// </summary>
public sealed class ConsultantQueryString : QueryStringBase
{
	public const ConsultantSortOrder DefaultSortOrder = ConsultantSortOrder.FamilyName;

	public ConsultantQueryString()
	{
		SortOrder = DefaultSortOrder;
	}

	public ConsultantQueryString(int pageSize, int pageNumber, string searchString, ConsultantSortOrder sortOrder) :
		base(pageSize, pageNumber, searchString)
	{
		SortOrder = sortOrder;
	}

	/// <summary>
	///     Used to sort the <see cref="Consultant" /> data set.
	/// </summary>
	public ConsultantSortOrder SortOrder { get; set; }
}