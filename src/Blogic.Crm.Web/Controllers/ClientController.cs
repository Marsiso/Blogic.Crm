using Blogic.Crm.Domain.Data.Dtos;
using Blogic.Crm.Domain.Data.Entities;
using Blogic.Crm.Domain.Exceptions;
using Blogic.Crm.Infrastructure.Commands;
using Blogic.Crm.Infrastructure.Pagination;
using Blogic.Crm.Infrastructure.Queries;
using Blogic.Crm.Infrastructure.Sorting;
using Blogic.Crm.Web.Views.Client;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
	public async Task<IActionResult> Dashboard(int pageSize, int pageNumber, string searchString,
	                                           ClientsSortOrder sortOrder, CancellationToken cancellationToken)
	{
		// Bind query string parameters.
		ClientQueryStringParameters queryStringParameters = new(pageSize, pageNumber, searchString, sortOrder,
		                                                        DateTime.MinValue, DateTime.MaxValue);

		// Get paginated client representations.
		GetPaginatedClientsQuery getPaginatedClientsQuery = new(queryStringParameters, false);

		var paginatedClientRepresentations = await _sender.Send(getPaginatedClientsQuery, cancellationToken);

		// Return View Model.
		return View(new ClientDashboardViewModel(paginatedClientRepresentations, queryStringParameters));
	}

	[HttpGet]
	[HttpPost]
	public async Task<IActionResult> Detail(long id, CancellationToken cancellationToken)
	{
		// Get client.
		GetClientByIdQuery getClientByIdQuery = new(id, false);
		Client? client = await _sender.Send(getClientByIdQuery, cancellationToken);

		// Create view model.
		return View(new ClientDetailViewModel(client));
	}

	[HttpGet]
	[HttpPost]
	public async Task<IActionResult> DeletePrompt(long id, string originAction, CancellationToken cancellationToken)
	{
		// Prompt user before deleting client.
		// Get client.
		GetClientByIdQuery getClientByIdQuery = new(id, false);
		Client? clientEntity = await _sender.Send(getClientByIdQuery, cancellationToken);
		if (clientEntity != null)
		{
			ClientRepresentation client = clientEntity.Adapt<ClientRepresentation>();

			return View(new ClientDeleteViewModel(client, originAction));
		}
		
		return View(new ClientDeleteViewModel(null, originAction));
	}
	
	[HttpGet]
	[HttpPost]
	public async Task<IActionResult> DeleteForm(long id, CancellationToken cancellationToken)
	{
		// Delete client.
		DeleteClientCommand deleteClientCommand = new(id);
		await _sender.Send(deleteClientCommand, cancellationToken);
			
		// Redirect user to dashboard.
		return RedirectToAction(nameof(Dashboard));
	}
}