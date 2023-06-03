using Blogic.Crm.Infrastructure.Pagination;
using Blogic.Crm.Infrastructure.Queries;
using Blogic.Crm.Infrastructure.Sorting;
using Blogic.Crm.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blogic.Crm.Web.Controllers;

public sealed class ClientController : Controller
{
	private readonly ISender _sender;

	public ClientController(ISender sender)
	{
		_sender = sender;
	}

	[HttpGet]
	[HttpPost]
	public async Task<IActionResult> Index(ClientQueryStringParameters? queryStringParameters,
	                                       CancellationToken cancellationToken)
	{
		queryStringParameters ??= new ClientQueryStringParameters()
		{
			PageNumber = QueryStringParameters.MinimumPageNumber,
			PageSize = QueryStringParameters.MinimumPageSize,
			SearchString = string.Empty,
			SortOrder = ClientQueryStringParameters.DefaultSortOrder
		};
		
		// Get paginated client entity representations
		GetPaginatedClientsQuery getPaginatedClientsQuery = new(queryStringParameters,false);
		
		var paginatedClientRepresentations= await _sender.Send(getPaginatedClientsQuery, cancellationToken);
		
		// Return View Model
		return View(new ClientViewModel(paginatedClientRepresentations, queryStringParameters));
	}
}