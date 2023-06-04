using Blogic.Crm.Domain.Data.Dtos;
using Blogic.Crm.Domain.Data.Entities;
using Blogic.Crm.Domain.Exceptions;
using Blogic.Crm.Domain.Routing;
using Blogic.Crm.Infrastructure.Commands;
using Blogic.Crm.Infrastructure.Pagination;
using Blogic.Crm.Infrastructure.Queries;
using Blogic.Crm.Infrastructure.Sorting;
using Blogic.Crm.Infrastructure.TypeExtensions;
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
		GetClientByIdQuery query = new(id, false);
		Client? clientEntity = await _sender.Send(query, cancellationToken);

		// Create view model.
		if (clientEntity == null)
		{
			return View(new GetClientViewModel(null));
		}

		ClientRepresentation client = clientEntity.Adapt<ClientRepresentation>();
		return View(new GetClientViewModel(client));
	}

	[HttpGet(Routes.Client.DeleteClientPrompt)]
	public async Task<IActionResult> DeleteClientPrompt(long id, string originAction, CancellationToken cancellationToken)
	{
		// Prompt user before deleting client.
		// Get client.
		GetClientByIdQuery query = new(id, false);
		var clientEntity = await _sender.Send(query, cancellationToken);
		
		// Create view model.
		if (clientEntity == null)
		{
			return View(new DeleteClientViewModel(null, originAction));
		}

		var client = clientEntity.Adapt<ClientRepresentation>();
		return View(new DeleteClientViewModel(client, originAction));

	}

	[HttpPost(Routes.Client.DeleteClient)]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteClient(long id, CancellationToken cancellationToken)
	{
		// Delete client.
		DeleteClientCommand command = new(id);
		await _sender.Send(command, cancellationToken);

		// Redirect user to dashboard.
		return RedirectToAction(nameof(GetClients));
	}

	[HttpGet(Routes.Client.UpdateClient)]
	public async Task<IActionResult> UpdateClient(long id, string originAction, CancellationToken cancellationToken)
	{
		// Get client.
		GetClientByIdQuery query = new(id, false);
		var clientEntity = await _sender.Send(query, cancellationToken);
		
		// Create view model.
		if (clientEntity == null)
		{
			return RedirectToAction(nameof(GetClients), new { });
		}

		ClientInput client = clientEntity.Adapt<ClientInput>();
		return View(new ClientUpdateViewModel(id, originAction, client));
	}
	
	[HttpPost(Routes.Client.UpdateClient)]
	public async Task<IActionResult> UpdateClient(long id, ClientUpdateViewModel viewModel,
	                                              CancellationToken cancellationToken)
	{
		try
		{
			// Get client.
			UpdateClientCommand command = viewModel.Client.Adapt<UpdateClientCommand>() with { Id = id };
			await _sender.Send(command, cancellationToken);

			if (StringExtensions.IsNotNullOrEmpty(viewModel.OriginAction) &&
			    nameof(GetClient).Equals(viewModel.OriginAction, StringComparison.OrdinalIgnoreCase))
			{
				return RedirectToAction(nameof(GetClient), new { id });
			}

			return RedirectToAction(nameof(GetClients), new { });
		}
		catch (ValidationException validationException)
		{
			return View(new ClientUpdateViewModel(viewModel.Id, viewModel.OriginAction, viewModel.Client, validationException));
		}
	}
}