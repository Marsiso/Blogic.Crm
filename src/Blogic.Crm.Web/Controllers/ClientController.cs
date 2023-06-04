using System.Globalization;
using System.Net.Mime;
using Blogic.Crm.Infrastructure.Sorting;
using Blogic.Crm.Web.Views.Client;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using static Blogic.Crm.Infrastructure.TypeExtensions.StringExtensions;

namespace Blogic.Crm.Web.Controllers;

/// <summary>
/// Controller that handles requests related to the <see cref="Client"/> entity.
/// </summary>
public sealed class ClientController : Controller
{
	private readonly ISender _sender;

	public ClientController(ISender sender)
	{
		_sender = sender;
	}

	/// <summary>
	/// Gets the client dashboard panel with the default client data set.
	/// </summary>
	/// <param name="queryString"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpGet(Routes.Client.GetClients)]
	public async Task<IActionResult> GetClients(ClientQueryString? queryString, CancellationToken cancellationToken)
	{
		// When there are none query string parameters found in the route set default values.
		queryString ??= new ClientQueryString(MinimumPageSize, MinimumPageNumber, string.Empty,
		                                      ClientsSortOrder.Id, DateTime.MinValue, DateTime.MaxValue);

		// Retrieve paginated client representation for the view model.
		GetClientsRepresentationsQuery query = new(queryString);
		var paginatedClients = await _sender.Send(query, cancellationToken);

		// Build the view model and return in to the client.
		return View(new GetClientsViewModel(paginatedClients, queryString));
	}

	/// <summary>
	/// Gets the client dashboard panel with sorted, filtered and order client data set.
	/// </summary>
	/// <param name="viewModel"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpPost(Routes.Client.GetClients)]
	[IgnoreAntiforgeryToken]
	public async Task<IActionResult> GetClients(GetClientsViewModel viewModel, CancellationToken cancellationToken)
	{
		// Retrieve paginated client representation for the view model.
		GetClientsRepresentationsQuery query = new(viewModel.QueryString);
		var paginatedClients = await _sender.Send(query, cancellationToken);

		// Build the view model and return in to the client.
		return View(new GetClientsViewModel(paginatedClients, viewModel.QueryString));
	}

	/// <summary>
	/// Gets the client details, owned contracts and related data.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpGet(Routes.Client.GetClient)]
	public async Task<IActionResult> GetClient(long id, CancellationToken cancellationToken)
	{
		// Retrieve the client with provided ID.
		GetClientByIdQuery query = new(id, false);
		Client? clientEntity = await _sender.Send(query, cancellationToken);

		// Build the view model and return in to the client.
		if (clientEntity == null)
		{
			return View(new GetClientViewModel(null));
		}

		ClientRepresentation client = clientEntity.Adapt<ClientRepresentation>();
		return View(new GetClientViewModel(client));
	}

	/// <summary>
	/// Gets the create client form with no daata.
	/// </summary>
	/// <returns></returns>
	[HttpGet(Routes.Client.CreateClient)]
	public IActionResult CreateClient()
	{
		return View(new ClientCreateViewModel());
	}

	/// <summary>
	/// Handles the requests made by the create client form, either creates the client in the persistence store
	/// or return the validation failures.
	/// </summary>
	/// <param name="indexViewModel"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpPost(Routes.Client.CreateClient)]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult>CreateClient(ClientCreateViewModel indexViewModel, CancellationToken cancellationToken)
	{
		try
		{
			// Validate and create the client in the persistence store.
			CreateClientCommand command = indexViewModel.Client.Adapt<CreateClientCommand>();
			Entity entity = await _sender.Send(command, cancellationToken);
			
			// If the client creation is successful then redirect user to the client details page. 
			return RedirectToAction("GetClient", "Client", new { entity.Id });
		}
		catch (ValidationException exception)
		{
			// If there are any validation failures then return them with the create client form data.
			return View(nameof(CreateClient), new ClientCreateViewModel(indexViewModel.Client, exception));
		}
		catch (Exception)
		{
			// If unexpected error occurs during the create client process then redirect user to the create client form.
			return View(nameof(CreateClient), new ClientCreateViewModel());
		}
	}

	/// <summary>
	/// Gets the update client form with no data.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="originAction"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpGet(Routes.Client.UpdateClient)]
	public async Task<IActionResult> UpdateClient(long id, string originAction, CancellationToken cancellationToken)
	{
		// Retrieve the client with the provided ID.
		GetClientByIdQuery query = new(id, false);
		var clientEntity = await _sender.Send(query, cancellationToken);
		
		// Build the view model and return in to the client.
		if (clientEntity == null)
		{
			return RedirectToAction(nameof(GetClients), new { });
		}

		ClientInput client = clientEntity.Adapt<ClientInput>();
		return View(new ClientUpdateViewModel(id, originAction, client));
	}

	/// <summary>
	/// Handles requests made by the update client form, either updates the persisted client
	/// or returns the update client form with validation failures.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="viewModel"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpPost(Routes.Client.UpdateClient)]
	public async Task<IActionResult> UpdateClient(long id, ClientUpdateViewModel viewModel,
	                                              CancellationToken cancellationToken)
	{
		try
		{
			// Get client.
			UpdateClientCommand command = viewModel.Client.Adapt<UpdateClientCommand>() with { Id = id };
			await _sender.Send(command, cancellationToken);

			// Build the view model and return in to the client.
			// If the action that the update client request originated from such as client details page is available
			// then redirect the user to the provided origin otherwise redirect the user to the dashboard panel.
			if (IsNotNullOrEmpty(viewModel.OriginAction) &&
			    nameof(GetClient).Equals(viewModel.OriginAction, StringComparison.OrdinalIgnoreCase))
			{
				return RedirectToAction(nameof(GetClient), new { id });
			}

			return RedirectToAction(nameof(GetClients), new { });
		}
		catch (ValidationException validationException)
		{
			// If there are any client model validation failures then include them with the form data in the view model.
			return View(new ClientUpdateViewModel(viewModel.Id, viewModel.OriginAction, viewModel.Client, validationException));
		}
	}

	/// <summary>
	/// Handles the deletion of persisted client.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpPost(Routes.Client.DeleteClient)]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteClient(long id, CancellationToken cancellationToken)
	{
		// Delete the persisted client.
		DeleteClientCommand command = new(id);
		await _sender.Send(command, cancellationToken);

		// Redirect user to the client dashboard panel page.
		return RedirectToAction(nameof(GetClients));
	}

	/// <summary>
	/// Displays the delete client prompt before the persisted client deletion.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="originAction"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpGet(Routes.Client.DeleteClientPrompt)]
	public async Task<IActionResult> DeleteClientPrompt(long id, string originAction, CancellationToken cancellationToken)
	{
		// Retrieve the persisted client by the provided ID.
		GetClientByIdQuery query = new(id, false);
		var clientEntity = await _sender.Send(query, cancellationToken);
		
		// Build the view model and return in to the client.
		if (clientEntity == null)
		{
			return View(new DeleteClientViewModel(null, originAction));
		}

		var client = clientEntity.Adapt<ClientRepresentation>();
		return View(new DeleteClientViewModel(client, originAction));

	}

	/// <summary>
	/// Handles requests made by the export to CSV file form.
	/// </summary>
	/// <param name="queryString"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpPost(Routes.Client.ExportClients)]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ExportClients([FromQuery] ClientQueryString? queryString,
	                                               CancellationToken cancellationToken)
	{
		// When there are none query string parameters found in the route set default values.
		queryString ??= new ClientQueryString(MinimumPageSize, MinimumPageNumber, string.Empty,
		                                      ClientsSortOrder.Id, DateTime.MinValue, DateTime.MaxValue);
		
		// Retrieve sorted, filtered and ordered client data set.
		GetClientRowsQuery query = new(queryString);
		IEnumerable<ClientRow> clientRows = await _sender.Send(query, cancellationToken);

		// Data set transformation. 
		var content = new MemoryStream();
		await using StreamWriter streamWriter = new(content);
		await using CsvWriter csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

		await csvWriter.WriteRecordsAsync(clientRows, cancellationToken);
		
		// Return the requested data as CSV file.
		return File(content.GetBuffer(), MediaTypeNames.Application.Octet, "clients.csv");
	}
}