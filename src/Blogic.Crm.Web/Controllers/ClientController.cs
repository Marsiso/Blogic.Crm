using Blogic.Crm.Domain.Data.Dtos;
using Blogic.Crm.Domain.Routing;
using Blogic.Crm.Infrastructure.Commands;
using Blogic.Crm.Infrastructure.Pagination;
using Blogic.Crm.Infrastructure.Queries;
using Blogic.Crm.Infrastructure.Sorting;
using Blogic.Crm.Web.Views.Client;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Blogic.Crm.Infrastructure.Pagination.QueryStringBase;

namespace Blogic.Crm.Web.Controllers;

public sealed class ClientController : Controller
{
	private readonly ISender _sender;

	public ClientController(ISender sender)
	{
		_sender = sender;
	}

	[HttpGet(Routes.Client.GetClients)]
	public async Task<IActionResult> GetClients(ClientQueryString? queryString, CancellationToken cancellationToken)
	{
		// Bind query string parameters.
		queryString ??= new ClientQueryString(MinimumPageSize, MinimumPageNumber, string.Empty,
		                                      ClientsSortOrder.Id, DateTime.MinValue, DateTime.MaxValue);

		// Get paginated client representations.
		GetPaginatedClientsQuery query = new(queryString, false);
		var paginatedClients = await _sender.Send(query, cancellationToken);

		// Return View Model.
		return View(new GetClientsViewModel(paginatedClients, queryString));
	}

	[HttpPost(Routes.Client.GetClients)]
	[IgnoreAntiforgeryToken]
	public async Task<IActionResult> GetClients(GetClientsViewModel viewModel, CancellationToken cancellationToken)
	{
		// Get paginated client representations.
		GetPaginatedClientsQuery query = new(viewModel.QueryString, false);
		var paginatedClients = await _sender.Send(query, cancellationToken);

		// Return View Model.
		return View(new GetClientsViewModel(paginatedClients, viewModel.QueryString));
	}

	[HttpGet(Routes.Client.GetClient)]
	public async Task<IActionResult> GetClient(long id, CancellationToken cancellationToken)
	{
		// Get client.
		GetClientByIdQuery getClientByIdQuery = new(id, false);
		var client = await _sender.Send(getClientByIdQuery, cancellationToken);

		// Create view model.
		return View(new GetClientViewModel(client));
	}

	[HttpGet(Routes.Client.DeleteClientPrompt)]
	public async Task<IActionResult> DeleteClientPrompt(long id, string originAction, CancellationToken cancellationToken)
	{
		// Prompt user before deleting client.
		// Get client.
		GetClientByIdQuery getClientByIdQuery = new(id, false);
		var clientEntity = await _sender.Send(getClientByIdQuery, cancellationToken);
		if (clientEntity != null)
		{
			var client = clientEntity.Adapt<ClientRepresentation>();

			return View(new DeleteClientViewModel(client, originAction));
		}

		return View(new DeleteClientViewModel(null, originAction));
	}

	[HttpPost(Routes.Client.DeleteClient)]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteClient(long id, CancellationToken cancellationToken)
	{
		// Delete client.
		DeleteClientCommand deleteClientCommand = new(id);
		await _sender.Send(deleteClientCommand, cancellationToken);

		// Redirect user to dashboard.
		return RedirectToAction(nameof(GetClients));
	}
}