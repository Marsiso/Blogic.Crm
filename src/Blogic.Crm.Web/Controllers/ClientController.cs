using System.Globalization;
using System.Net.Mime;
using Blogic.Crm.Web.Views.Client;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using static Blogic.Crm.Domain.Data.Entities.User;

namespace Blogic.Crm.Web.Controllers;

/// <summary>
///     Controller that handles requests related to the <see cref="Client" /> entity.
/// </summary>
public sealed class ClientController : Controller
{
	private readonly ISender _sender;

	public ClientController(ISender sender)
	{
		_sender = sender;
	}

	/// <summary>
	///     Gets the client dashboard panel with the default client data set.
	/// </summary>
	/// <param name="queryString"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpGet(Routes.Client.GetAll)]
	[HttpPost(Routes.Client.GetAll)]
	[IgnoreAntiforgeryToken]
	public async Task<IActionResult> GetClients(ClientQueryString queryString,
	                                            CancellationToken cancellationToken)
	{
		try
		{
			// Retrieve paginated client representation for the view model.
			GetClientsRepresentationsQuery query = new(queryString);
			var paginatedClients = await _sender.Send(query, cancellationToken);

			// Build the view model and return in to the client.
			return View(new GetClientsViewModel { Clients = paginatedClients, QueryString = queryString });
		}
		catch (Exception exception)
		{
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ClientController),
			          nameof(GetClients));

			return RedirectToAction(nameof(Index), "Home", new { });
		}
	}

	/// <summary>
	///     Gets the client details, owned contracts and related data.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpGet(Routes.Client.Get)]
	[HttpPost(Routes.Client.Get)]
	[IgnoreAntiforgeryToken]
	public async Task<IActionResult> GetClient(long id, CancellationToken cancellationToken)
	{
		try
		{
			// Retrieve the client with provided ID.
			GetClientQuery query = new(new Entity { Id = id }, false);
			var clientEntity = await _sender.Send(query, cancellationToken);

			// Build the view model and return in to the client.
			if (clientEntity == null)
			{
				return View(new GetClientViewModel(null));
			}

			var client = clientEntity.Adapt<ClientRepresentation>();
			return View(new GetClientViewModel(client));
		}
		catch (Exception exception)
		{
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ClientController),
			          nameof(GetClients));

			return RedirectToAction(nameof(GetClients));
		}
	}

	/// <summary>
	///     Gets the create client form with no data.
	/// </summary>
	/// <returns></returns>
	[HttpGet(Routes.Client.Create)]
	public IActionResult CreateClient()
	{
		try
		{
			return View(new ClientCreateViewModel
			{
				Client = new ClientInput
				{
					DateBorn = MinimalDateBorn
				}
			});
		}
		catch (Exception exception)
		{
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ClientController),
			          nameof(CreateClient));

			return RedirectToAction(nameof(GetClients));
		}
	}

	/// <summary>
	///     Handles the requests made by the create client form, either creates the client in the persistence store
	///     or return the validation failures.
	/// </summary>
	/// <param name="clientInput"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpPost(Routes.Client.Create)]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> CreateClient(
		[Bind(Prefix = nameof(ClientCreateViewModel.Client))] ClientInput clientInput,
		CancellationToken cancellationToken)
	{
		try
		{
			// Validate and create the client in the persistence store.
			var command = clientInput.Adapt<CreateClientCommand>();
			var entity = await _sender.Send(command, cancellationToken);

			// If the client creation is successful then redirect user to the client details page. 
			return RedirectToAction("GetClient", "Client", new { entity.Id });
		}
		catch (ValidationException exception)
		{
			// If there are any validation failures then return them with the create client form data.
			return View(nameof(CreateClient), new ClientCreateViewModel(clientInput, exception));
		}
		catch (Exception exception)
		{
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ClientController),
			          nameof(CreateClient));

			return RedirectToAction(nameof(GetClients));
		}
	}

	/// <summary>
	///     Gets the update client form with no data.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpGet(Routes.Client.Update)]
	public async Task<IActionResult> UpdateClient(long id, CancellationToken cancellationToken)
	{
		var entity = new Entity { Id = id };

		try
		{
			// Retrieve the client with the provided ID.
			GetClientQuery query = new(entity, false);
			var clientEntity = await _sender.Send(query, cancellationToken);

			// Build the view model and return in to the client.
			if (clientEntity == null)
			{
				return RedirectToAction(nameof(GetClients), new { });
			}

			var client = clientEntity.Adapt<ClientInput>();
			return View(new ClientUpdateViewModel(id, client));
		}
		catch (Exception exception)
		{
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ClientController),
			          nameof(UpdateClient));

			return RedirectToAction(nameof(GetClient), new { entity.Id });
		}
	}

	/// <summary>
	///     Handles requests made by the update client form, either updates the persisted client
	///     or returns the update client form with validation failures.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="clientInput"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpPost(Routes.Client.Update)]
	public async Task<IActionResult> UpdateClient(long id,
	                                              [Bind(Prefix = nameof(ClientCreateViewModel.Client))]
	                                              ClientInput clientInput,
	                                              CancellationToken cancellationToken)
	{
		var entity = new Entity { Id = id };

		try
		{
			// Get client.
			var command = clientInput.Adapt<UpdateClientCommand>() with { Id = id };
			await _sender.Send(command, cancellationToken);

			// Build the view model and return in to the client.
			return RedirectToAction(nameof(GetClient), new { entity.Id });
		}
		catch (ValidationException validationException)
		{
			// If there are any client model validation failures then include them with the form data in the view model.
			return View(new ClientUpdateViewModel(id, clientInput, validationException));
		}
		catch (Exception exception)
		{
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ClientController),
			          nameof(UpdateClient));

			return RedirectToAction(nameof(GetClient), new { entity.Id });
		}
	}

	/// <summary>
	///     Handles the deletion of persisted client.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpPost(Routes.Client.Delete)]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteClient(long id, CancellationToken cancellationToken)
	{
		var entity = new Entity { Id = id };

		try
		{
			// Delete the persisted client.
			DeleteClientCommand command = new(entity);
			await _sender.Send(command, cancellationToken);

			// Redirect user to the client dashboard panel page.
			return RedirectToAction(nameof(GetClients));
		}
		catch (Exception exception)
		{
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ClientController),
			          nameof(DeleteClient));

			return RedirectToAction(nameof(GetClient), new { entity.Id });
		}
	}

	/// <summary>
	///     Handles requests made by the export to CSV file form.
	/// </summary>
	/// <param name="queryString"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpPost(Routes.Client.Export)]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ExportClients(ClientQueryString queryString, CancellationToken cancellationToken)
	{
		try
		{
			// Retrieve sorted, filtered and ordered client data set.
			GetClientRowsQuery query = new(queryString);
			var clientRows = await _sender.Send(query, cancellationToken);

			// Data set transformation. 
			var content = new MemoryStream();
			await using StreamWriter streamWriter = new(content);
			await using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

			await csvWriter.WriteRecordsAsync(clientRows, cancellationToken);

			// Return the requested data as CSV file.
			return File(content.GetBuffer(), MediaTypeNames.Application.Octet, "clients.csv");

		}
		catch (Exception exception)
		{
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ClientController),
			          nameof(ExportClients));

			return new StatusCodeResult(StatusCodes.Status500InternalServerError);

		}
	}
}