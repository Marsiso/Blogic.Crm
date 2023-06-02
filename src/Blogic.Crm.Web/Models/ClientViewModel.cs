using Blogic.Crm.Domain.Data.Dtos;
using Blogic.Crm.Infrastructure.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Blogic.Crm.Web.Models;

public sealed class ClientViewModel : PageModel
{
	public ClientViewModel(PaginatedList<ClientRepresentation> clients, ClientQueryStringParameters queryStringParameters)
	{
		Clients = clients;
		QueryStringParameters = queryStringParameters;
	}

	public PaginatedList<ClientRepresentation> Clients { get; set; }
	
	[FromRoute]
	public ClientQueryStringParameters QueryStringParameters { get; set; }
}