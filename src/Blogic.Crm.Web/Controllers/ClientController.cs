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
		// Get paginated client entity representations
		GetPaginatedClientRepresentationsQuery getPaginatedClientRepresentationsQuery =
			new(pageNumber ?? QueryStringParameters.MinimumPageNumber,
			    pageSize ?? QueryStringParameters.MinimumPageSize,
			    sortOrder ?? ClientSortOrder.FamilyNameDesc,
			    false);
		
		var paginatedClientRepresentations= await _sender.Send(getPaginatedClientRepresentationsQuery, cancellationToken);
		
		// Return View Model
		return View(paginatedClientRepresentations);
	}
}