using Blogic.Crm.Web.Views.Consultant;
using static Blogic.Crm.Domain.Data.Entities.User;

namespace Blogic.Crm.Web.Controllers;

/// <summary>
///     A controller that handles requests that are related to the contract consultants.
/// </summary>
public sealed class ConsultantController : Controller
{
	private readonly ISender _sender;

	public ConsultantController(ISender sender)
	{
		_sender = sender;
	}

	/// <summary>
	///     Returns a view with the consultant management panel.
	/// </summary>
	[HttpGet(Routes.Consultant.GetAll)]
	[HttpPost(Routes.Consultant.GetAll)]
	[IgnoreAntiforgeryToken]
	public async Task<IActionResult> GetConsultants(ConsultantQueryString queryString,
	                                            CancellationToken cancellationToken)
	{
		try
		{
			// Retrieving consultants from the database.
			GetConsultantsQuery query = new(queryString);
			var paginatedConsultants = await _sender.Send(query, cancellationToken);

			// Response to request.
			return View(new GetConsultantsViewModel { Consultants = paginatedConsultants, QueryString = queryString });
		}
		catch (Exception exception)
		{
			// Exception catching and logging.
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ConsultantController),
			          nameof(GetConsultants));

			return RedirectToAction(nameof(Index), "Home");
		}
	}

	/// <summary>
	///     Returns a view with the contract details.
	/// </summary>
	[HttpGet(Routes.Consultant.Get)]
	[HttpPost(Routes.Consultant.Get)]
	[IgnoreAntiforgeryToken]
	public async Task<IActionResult> GetConsultant(long id, CancellationToken cancellationToken)
	{
		try
		{
			// Represents an identifiable entity in the database.
			var entity = new Entity { Id = id };
			
			// Retrieving client from the database.
			GetConsultantQuery query = new(entity, false);
			var consultantEntity = await _sender.Send(query, cancellationToken);

			// When the consultant is not found then respond to the request.
			if (consultantEntity == null)
			{
				return View(new GetConsultantViewModel());
			}
			
			// Retrieving managed contracts from the database.
			GetContractsManagedQuery contractsManagedQuery = new(entity);
			var managedContracts = await _sender.Send(contractsManagedQuery, cancellationToken);
			
			// Retrieving consulted contracts from the database.
			GetContractsConsultedQuery contractsConsultedQuery = new(entity);
			var consultedContracts = await _sender.Send(contractsConsultedQuery, cancellationToken);		

			// Mapping consultant to the model to be exported.
			var consultant = consultantEntity.Adapt<ConsultantRepresentation>();
			
			// Response to request.
			return View(new GetConsultantViewModel(consultant, managedContracts, consultedContracts));
		}
		catch (Exception exception)
		{
			// Exception catching and logging.
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ConsultantController),
			          nameof(GetConsultants));

			return RedirectToAction(nameof(GetConsultants));
		}
	}

	/// <summary>
	///     Returns a view with the consultant registration form.
	/// </summary>
	[HttpGet(Routes.Consultant.Create)]
	public IActionResult CreateConsultant()
	{
		try
		{
			// Response to request.
			return View(new ConsultantCreateViewModel(new ConsultantInput(MinimalDateBorn)));
		}
		catch (Exception exception)
		{
			// Exception catching and logging.
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ConsultantController),
			          nameof(CreateConsultant));

			return RedirectToAction(nameof(GetConsultants));
		}
	}

	/// <summary>
	///     Returns a view with the consultant registration form and its validation failures if any.
	/// </summary>
	[HttpPost(Routes.Consultant.Create)]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> CreateConsultant(
		[Bind(Prefix = nameof(ConsultantCreateViewModel.Consultant))] ConsultantInput consultantInput,
		CancellationToken cancellationToken)
	{
		try
		{
			// Create a consultant in the database.
			var command = consultantInput.Adapt<CreateConsultantCommand>();
			var entity = await _sender.Send(command, cancellationToken);

			// If the consultant is successfully registered, display his details. 
			return RedirectToAction("GetConsultant", "Consultant", new { entity.Id });
		}
		catch (ValidationException exception)
		{
			// In case of validation errors in the consultant registration form, return the form with validation errors.
			return View(nameof(CreateConsultant), new ConsultantCreateViewModel(consultantInput, exception));
		}
		catch (Exception exception)
		{
			// Exception catching and logging.
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ConsultantController),
			          nameof(CreateConsultant));

			return RedirectToAction(nameof(GetConsultants));
		}
	}

	/// <summary>
	///     Returns a view with the consultant update form.
	/// </summary>
	[HttpGet(Routes.Consultant.Update)]
	public async Task<IActionResult> UpdateConsultant(long id, CancellationToken cancellationToken)
	{
		try
		{
			// Represents an identifiable entity in the database.
			var entity = new Entity(id);
			
			// Retrieving client from the database.
			GetConsultantQuery query = new(entity, false);
			var consultantEntity = await _sender.Send(query, cancellationToken);

			// When the consultant is not found then respond to the request with redirect to the consultant management panel.
			if (consultantEntity is null)
			{
				return RedirectToAction(nameof(GetConsultants), new { });
			}

			// Mapping consultant to the model to be exported.
			var consultant = consultantEntity.Adapt<ConsultantInput>();
			
			// Response to request.
			return View(new ConsultantUpdateViewModel(id, consultant));
		}
		catch (Exception exception)
		{
			// Exception catching and logging.
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ConsultantController),
			          nameof(UpdateConsultant));

			return RedirectToAction(nameof(GetConsultants));
		}
	}

	/// <summary>
	///     Returns a view with the consultant update form and its validation failures if any.
	/// </summary>
	[HttpPost(Routes.Consultant.Update)]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> UpdateConsultant(long id,
	                                                  [Bind(Prefix = nameof(ConsultantCreateViewModel.Consultant))]
	                                                  ConsultantInput consultantInput,
	                                                  CancellationToken cancellationToken)
	{
		try
		{
			// Represents an identifiable entity in the database.
			var entity = new Entity(id);
			
			// Update consultant in the database.
			var command = consultantInput.Adapt<UpdateConsultantCommand>() with { Entity = entity };
			await _sender.Send(command, cancellationToken);

			// If the consultant is successfully updated, display his details.
			return RedirectToAction(nameof(GetConsultant), new { entity.Id });
		}
		catch (ValidationException validationException)
		{
			// In case of validation errors in the consultant update form, return the form with validation errors.
			return View(new ConsultantUpdateViewModel(id, consultantInput, validationException));
		}
		catch (Exception exception)
		{
			// Exception catching and logging.
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ConsultantController),
			          nameof(UpdateConsultant));

			return RedirectToAction(nameof(GetConsultants));
		}
	}

	/// <summary>
	///     Handles the form for deleting consultants in the database.
	/// </summary>
	[HttpPost(Routes.Consultant.Delete)]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteConsultant(long id, CancellationToken cancellationToken)
	{
		try
		{
			// Represents an identifiable entity in the database.
			var entity = new Entity(id);
			
			// Delete the consultant in the database.
			DeleteConsultantCommand command = new(entity);
			await _sender.Send(command, cancellationToken);

			// If the consultant is successfully deleted, display the consultant management panel.
			return RedirectToAction(nameof(GetConsultants));
		}
		catch (Exception exception)
		{
			// Exception catching and logging.
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ConsultantController),
			          nameof(DeleteConsultant));

			return RedirectToAction(nameof(GetConsultants));
		}
	}

	/// <summary>
	///     Handles the form for exporting consultant records to CSV.
	/// </summary>
	[HttpPost(Routes.Consultant.Export)]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ExportConsultants(ConsultantQueryString queryString, CancellationToken cancellationToken)
	{
		try
		{
			// Retrieving consultants from the database.
			GetConsultantsToExportQuery query = new(queryString);
			var consultantRows = await _sender.Send(query, cancellationToken);

			// Export data set to CSV file. 
			var content = new MemoryStream();
			await using StreamWriter streamWriter = new(content);
			await using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

			await csvWriter.WriteRecordsAsync(consultantRows, cancellationToken);

			// Return the requested file.
			return File(content.GetBuffer(), MediaTypeNames.Application.Octet, "consultants.csv");

		}
		catch (Exception exception)
		{
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ConsultantController),
			          nameof(ExportConsultants));

			return new StatusCodeResult(StatusCodes.Status500InternalServerError);

		}
	}
}