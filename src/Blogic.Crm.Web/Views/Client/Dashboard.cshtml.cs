using Blogic.Crm.Domain.Data.Dtos;
using Blogic.Crm.Infrastructure.Pagination;
using Blogic.Crm.Infrastructure.Sorting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blogic.Crm.Web.Views.Client;

public sealed class ClientDashboardViewModel : PageModel
{
	public ClientDashboardViewModel(PaginatedList<ClientRepresentation> clients,
	                                ClientQueryStringParameters queryStringParameters)
	{
		Clients = clients;
		PageSize = queryStringParameters.PageSize;
		PageNumber = queryStringParameters.PageNumber;
		SearchString = queryStringParameters.SearchString;
		SortOrder = queryStringParameters.SortOrder;
		MinDateBorn = queryStringParameters.MinDateBorn;
		MaxDateBorn = queryStringParameters.MaxDateBorn;
	}

	public PaginatedList<ClientRepresentation> Clients { get; set; }
	public int PageSize { get; set; }
	public int PageNumber { get; set; }
	public string SearchString { get; set; }
	public ClientsSortOrder SortOrder { get; set; }
	public DateTime MinDateBorn { get; set; }
	public DateTime MaxDateBorn { get; set; }
}