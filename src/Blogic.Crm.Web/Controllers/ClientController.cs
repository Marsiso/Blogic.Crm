using static Blogic.Crm.Domain.Data.Entities.User;
using Blogic.Crm.Web.Views.Client;

namespace Blogic.Crm.Web.Controllers;

/// <summary>
///     A controller that handles requests that are related to the clients.
/// </summary>
public sealed class ClientController : Controller
{
	private readonly ISender _sender;

	public ClientController(ISender sender)
	{
		_sender = sender;
	}

	/// <summary>
	///     Returns a view with the user management panel.
	/// </summary>
	[HttpGet(Routes.Client.GetAll)]
	[HttpPost(Routes.Client.GetAll)]
	[IgnoreAntiforgeryToken]
	public async Task<IActionResult> GetClients(ClientQueryString queryString,
	                                            CancellationToken cancellationToken)
	{
		try
		{
			// Retrieving clients from the database.
			GetClientsQuery query = new(queryString);
			var paginatedClients = await _sender.Send(query, cancellationToken);

			// Response to request.
			return View(new GetClientsViewModel { Clients = paginatedClients, QueryString = queryString });
		}
		catch (Exception exception)
		{
			// Exception catching and logging.
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ClientController),
			          nameof(GetClients));

			return RedirectToAction(nameof(Index), "Home");
		}
	}

	/// <summary>
	///     Returns a view with the user details.
	/// </summary>
	[HttpGet(Routes.Client.Get)]
	[HttpPost(Routes.Client.Get)]
	[IgnoreAntiforgeryToken]
	public async Task<IActionResult> GetClient(long id, CancellationToken cancellationToken)
	{
		try
		{
			// Represents an identifiable entity in the database.
			var entity = new Entity(id);
			
			// Retrieving client from the database.
			GetClientQuery clientQuery = new(entity, false);
			var clientEntity = await _sender.Send(clientQuery, cancellationToken);

			// When the client is not found then respond to the request.
			if (clientEntity is null)
			{
				return View(new GetClientViewModel(null, null));
			}

			// Retrieving owned contracts by client from the database.
			GetContractsOwnedQuery query = new(entity);
			var ownedContracts = await _sender.Send(query, cancellationToken);
			
			// Mapping client to the model to be exported.
			var client = clientEntity.Adapt<ClientRepresentation>();
			
			// Response to request.
			return View(new GetClientViewModel(client, ownedContracts));
		}
		catch (Exception exception)
		{
			// Exception catching and logging.
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ClientController),
			          nameof(GetClients));

			return RedirectToAction(nameof(GetClients));
		}
	}

	/// <summary>
	///     Returns a view with the user registration form.
	/// </summary>
	[HttpGet(Routes.Client.Create)]
	public IActionResult CreateClient()
	{
		try
		{
			// Response to request.
			return View(new ClientCreateViewModel(new ClientInput(MinimalDateBorn)));
		}
		catch (Exception exception)
		{
			// Exception catching and logging.
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ClientController),
			          nameof(CreateClient));

			return RedirectToAction(nameof(GetClients));
		}
	}

	/// <summary>
	///     Returns a view with the user registration form and its validation failures if any.
	/// </summary>
	[HttpPost(Routes.Client.Create)]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> CreateClient(
		[Bind(Prefix = nameof(ClientCreateViewModel.Client))] ClientInput clientInput,
		CancellationToken cancellationToken)
	{
		try
		{
			// Create a client in the database.
			var command = clientInput.Adapt<CreateClientCommand>();
			var entity = await _sender.Send(command, cancellationToken);

			// If the user is successfully registered, display his details.
			return RedirectToAction(nameof(GetClient), new { entity.Id });
		}
		catch (ValidationException exception)
		{
			// In case of validation errors in the client registration form, return the form with validation errors.
			return View(nameof(CreateClient), new ClientCreateViewModel(clientInput, exception));
		}
		catch (Exception exception)
		{
			// Exception catching and logging.
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ClientController),
			          nameof(CreateClient));

			return RedirectToAction(nameof(GetClients));
		}
	}

	/// <summary>
	///     Returns a view with the user update form.
	/// </summary>
	[HttpGet(Routes.Client.Update)]
	public async Task<IActionResult> UpdateClient(long id, CancellationToken cancellationToken)
	{
		try
		{
			// Represents an identifiable entity in the database.
			var entity = new Entity(id);
			
			// Retrieving client from the database.
			GetClientQuery query = new(entity, false);
			var clientEntity = await _sender.Send(query, cancellationToken);

			// When the client is not found then respond to the request with redirect to the client management panel.
			if (clientEntity is null)
			{
				return RedirectToAction(nameof(GetClients));
			}

			// Mapping client to the model to be exported.
			var client = clientEntity.Adapt<ClientInput>();
			
			// Response to request.
			return View(new ClientUpdateViewModel(id, client));
		}
		catch (Exception exception)
		{
			// Exception catching and logging.
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ClientController),
			          nameof(UpdateClient));

			return RedirectToAction(nameof(GetClients));
		}
	}

	/// <summary>
	///     Returns a view with the user update form and its validation failures if any.
	/// </summary>
	[HttpPost(Routes.Client.Update)]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> UpdateClient(long id,
	                                              [Bind(Prefix = nameof(ClientCreateViewModel.Client))]
	                                              ClientInput clientInput,
	                                              CancellationToken cancellationToken)
	{
		try
		{
			// Represents an identifiable entity in the database.
			var entity = new Entity(id);
			
			// Update client in the database.
			var command = clientInput.Adapt<UpdateClientCommand>() with { Entity = entity };
			await _sender.Send(command, cancellationToken);

			// If the user is successfully updated, display his details.
			return RedirectToAction(nameof(GetClient), new { entity.Id });
		}
		catch (ValidationException validationException)
		{
			// In case of validation errors in the client update form, return the form with validation errors.
			return View(new ClientUpdateViewModel(id, clientInput, validationException));
		}
		catch (Exception exception)
		{
			// Exception catching and logging.
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ClientController),
			          nameof(UpdateClient));

			return RedirectToAction(nameof(GetClients));
		}
	}

	/// <summary>
	///     Handles the form for deleting clients in the database.
	/// </summary>
	[HttpPost(Routes.Client.Delete)]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteClient(long id, CancellationToken cancellationToken)
	{
		try
		{
			// Represents an identifiable entity in the database.
			var entity = new Entity(id);
			
			// Delete the client in the database.
			DeleteClientCommand command = new(entity);
			await _sender.Send(command, cancellationToken);

			// If the user is successfully deleted, display the client management panel.
			return RedirectToAction(nameof(GetClients));
		}
		catch (Exception exception)
		{
			// Exception catching and logging.
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ClientController),
			          nameof(DeleteClient));

			return RedirectToAction(nameof(GetClients));
		}
	}

	/// <summary>
	///     Handles the form for exporting client records to CSV.
	/// </summary>
	[HttpPost(Routes.Client.Export)]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ExportClients(ClientQueryString queryString, CancellationToken cancellationToken)
	{
		try
		{
			// Retrieving clients from the database.
			GetClientsToExportQuery query = new(queryString);
			var clientRows = await _sender.Send(query, cancellationToken);

			// Export data set to CSV file. 
			var content = new MemoryStream();
			await using StreamWriter streamWriter = new(content);
			await using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

			await csvWriter.WriteRecordsAsync(clientRows, cancellationToken);

			// Return the requested file.
			return File(content.GetBuffer(), MediaTypeNames.Application.Octet, "clients.csv");

		}
		catch (Exception exception)
		{
			// Exception catching and logging.
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ClientController),
			          nameof(ExportClients));

			return new StatusCodeResult(StatusCodes.Status500InternalServerError);
		}
	}
}