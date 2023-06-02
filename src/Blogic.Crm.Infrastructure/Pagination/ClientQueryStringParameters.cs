using Blogic.Crm.Infrastructure.Sorting;

namespace Blogic.Crm.Infrastructure.Pagination;

public sealed class ClientQueryStringParameters : QueryStringParameters
{
	public const ClientsSortOrder DefaultSortOrder = ClientsSortOrder.FamilyNameDesc;

	public ClientsSortOrder SortOrder { get; set; } = DefaultSortOrder;
}