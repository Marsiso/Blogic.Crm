#nullable disable

using Blogic.Crm.Domain.Data.Dtos;
using Blogic.Crm.Infrastructure.Pagination;
using Blogic.Crm.Infrastructure.Sorting;
using static Blogic.Crm.Infrastructure.Pagination.QueryStringBase;

namespace Blogic.Crm.Web.Views.Client;

public sealed class GetClientsViewModel
{
	public GetClientsViewModel()
	{
		Clients = new PaginatedList<ClientRepresentation>(new List<ClientRepresentation>(), 0, MinimumPageNumber,
		                                                  MinimumPageSize);
		
		QueryString = new ClientQueryString(MinimumPageSize, MinimumPageNumber, string.Empty,
		                                                        ClientsSortOrder.Id, DateTime.MinValue,
		                                                        DateTime.MaxValue);
	}

	public GetClientsViewModel(PaginatedList<ClientRepresentation> clients,
	                                ClientQueryString queryString)
	{
		Clients = clients;
		QueryString = queryString;
	}

	public PaginatedList<ClientRepresentation> Clients { get; set; }
	public ClientQueryString QueryString { get; set; }
}