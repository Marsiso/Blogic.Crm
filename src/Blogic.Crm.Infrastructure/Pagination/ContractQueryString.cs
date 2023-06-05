using Blogic.Crm.Infrastructure.Sorting;
using Microsoft.AspNetCore.Mvc;

namespace Blogic.Crm.Infrastructure.Pagination;

[BindProperties(SupportsGet = true)]
public sealed class ContractQueryString : QueryStringBase
{
	public const ContractSortOrder DefaultSortOrder = ContractSortOrder.Id;

	public ContractQueryString()
	{
		SortOrder = DefaultSortOrder;
	}

	public ContractQueryString(int pageSize, int pageNumber, string searchString, ContractSortOrder sortOrder) :
		base(pageSize, pageNumber, searchString)
	{
		SortOrder = sortOrder;
	}

	/// <summary>
	///     Used to sort the <see cref="Consultant" /> data set.
	/// </summary>
	public ContractSortOrder SortOrder { get; set; }
}