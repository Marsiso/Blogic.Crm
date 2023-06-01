namespace Blogic.Crm.Infrastructure.Paging;

public sealed class ClientQueryStringParameters : QueryStringParameters
{
	public ClientSortOrder SortOrder { get; set; } = ClientSortOrder.FamilyNameDesc;
}