using System.Globalization;
using System.Net.Mime;
using Blogic.Crm.Infrastructure.Sorting;
using Blogic.Crm.Web.Views.Client;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;

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
		GetClientsRepresentationsQuery query = new(queryString);
		var paginatedClients = await _sender.Send(query, cancellationToken);

		// Return View Model.
		return View(new GetClientsViewModel(paginatedClients, queryString));
	}

	[HttpPost(Routes.Client.GetClients)]
	[IgnoreAntiforgeryToken]
	public async Task<IActionResult> GetClients(GetClientsViewModel viewModel, CancellationToken cancellationToken)
	{
		// Get paginated client representations.
		GetClientsRepresentationsQuery query = new(viewModel.QueryString);
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

	[HttpGet(Routes.Client.CreateClient)]
	public IActionResult CreateClient()
	{
		return View(new ClientCreateViewModel());
	}

	[HttpPost(Routes.Client.CreateClient)]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult>CreateClient(ClientCreateViewModel indexViewModel, CancellationToken cancellationToken)
	{
		try
		{
			CreateClientCommand command = indexViewModel.Client.Adapt<CreateClientCommand>();
			Entity entity = await _sender.Send(command, cancellationToken);
			return RedirectToAction("GetClient", "Client", new { entity.Id });
		}
		catch (ValidationException exception)
		{
			return View(nameof(CreateClient), new ClientCreateViewModel(indexViewModel.Client, exception));
		}
		catch (Exception)
		{
			return View(nameof(CreateClient), new ClientCreateViewModel());
		}
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

	[HttpPost(Routes.Client.ExportClients)]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ExportClients([FromQuery] ClientQueryString? queryString,
	                                               CancellationToken cancellationToken)
	{
		// Bind query string parameters.
		queryString ??= new ClientQueryString(MinimumPageSize, MinimumPageNumber, string.Empty,
		                                      ClientsSortOrder.Id, DateTime.MinValue, DateTime.MaxValue);
		
		// Get client.
		GetClientRowsQuery query = new(queryString);
		IEnumerable<ClientRow> clientRows = await _sender.Send(query, cancellationToken);

		// Write client rows to CSV file
		var content = new MemoryStream();
		await using StreamWriter streamWriter = new(content);
		await using CsvWriter csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

		await csvWriter.WriteRecordsAsync(clientRows, cancellationToken);
		
		// Get CSV file
		return File(content.GetBuffer(), MediaTypeNames.Application.Octet, "clients.csv");
	}
}