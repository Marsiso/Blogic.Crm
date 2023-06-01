using Blogic.Crm.Infrastructure.Paging;
using Blogic.Crm.Infrastructure.Queries;
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
	public async Task<IActionResult> Index(int? pageSize, int? pageNumber, ClientSortOrder? sortOrder,
	                                       CancellationToken cancellationToken)
	{
		ViewData[nameof(ClientSortOrder)] = ClientSortOrder.FamilyNameDesc;
		
		// Builder query string
		ClientQueryStringParameters queryStringParameters = new()
		{
			PageNumber = pageNumber ?? QueryStringParameters.MinimumPageNumber,
			PageSize = pageSize ?? QueryStringParameters.MinimumPageSize,
			SortOrder = sortOrder ?? ClientSortOrder.FamilyNameDesc
		};

		// Get paginated client entity representations
		GetPaginatedClientRepresentationsQuery getPaginatedClientRepresentationsQuery = new(queryStringParameters, false);
		var paginatedClientRepresentations= await _sender.Send(getPaginatedClientRepresentationsQuery, cancellationToken);
		
		// Return View Model
		return View(paginatedClientRepresentations);
	}
}