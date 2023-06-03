using Blogic.Crm.Infrastructure.Commands;
using Blogic.Crm.Infrastructure.Pagination;
using Blogic.Crm.Infrastructure.Queries;
using Blogic.Crm.Infrastructure.Sorting;
using Blogic.Crm.Web.Views.Client;
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
	public async Task<IActionResult> Dashboard(int? pageSize, int? pageNumber, string? searchString,
	                                           ClientsSortOrder? sortOrder, DateTime? minDateBorn,
	                                           DateTime? maxDateBorn,
	                                           CancellationToken cancellationToken)
	{
		// Construct default query string parameter if null
		var queryStringParameters = new ClientQueryStringParameters(
			pageSize ?? QueryStringParameters.MinimumPageSize,
			pageNumber ?? QueryStringParameters.MinimumPageNumber,
			searchString ?? string.Empty,
			sortOrder ?? ClientQueryStringParameters.DefaultSortOrder,
			minDateBorn ?? DateTime.MinValue,
			maxDateBorn  ?? DateTime.MaxValue
			);

		// Get paginated client entity representations
		GetPaginatedClientsQuery getPaginatedClientsQuery = new(queryStringParameters, false);

		var paginatedClientRepresentations = await _sender.Send(getPaginatedClientsQuery, cancellationToken);

		// Return View Model
		return View(new ClientDashboardViewModel(paginatedClientRepresentations, queryStringParameters));
	}

	[HttpGet]
	[HttpPost]
	public async Task<IActionResult> Detail(long id, CancellationToken cancellationToken)
	{
		// Get client
		var getClientByIdQuery = new GetClientByIdQuery(id, false);
		var client = await _sender.Send(getClientByIdQuery, cancellationToken);

		// Create view model
		return View(new ClientDetailViewModel(client));
	}

	[HttpGet]
	[HttpPost]
	public async Task<IActionResult> DeleteClient(long id, CancellationToken cancellationToken)
	{
		// Delete client
		var deleteClientCommand = new DeleteClientCommand(id);
		await _sender.Send(deleteClientCommand, cancellationToken);

		// Redirect user to dashboard
		return RedirectToAction(nameof(Dashboard));
	}
}