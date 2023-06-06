using System.Globalization;
using System.Net.Mime;
using Blogic.Crm.Web.Views.Consultant;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using static Blogic.Crm.Domain.Data.Entities.User;

namespace Blogic.Crm.Web.Controllers;

/// <summary>
///     Controller that handles requests related to the <see cref="Consultant" /> entity.
/// </summary>
public sealed class ConsultantController : Controller
{
	private readonly ISender _sender;

	public ConsultantController(ISender sender)
	{
		_sender = sender;
	}

	/// <summary>
	///     Gets the consultant dashboard panel with the default consultant data set.
	/// </summary>
	/// <param name="queryString"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpGet(Routes.Consultant.GetAll)]
	[HttpPost(Routes.Consultant.GetAll)]
	[IgnoreAntiforgeryToken]
	public async Task<IActionResult> GetConsultants(ConsultantQueryString queryString,
	                                            CancellationToken cancellationToken)
	{
		try
		{
			// Retrieve paginated consultant representation for the view model.
			GetConsultantRepresentationsQuery query = new(queryString);
			var paginatedConsultants = await _sender.Send(query, cancellationToken);

			// Build the view model and return in to the client.
			return View(new GetConsultantsViewModel { Consultants = paginatedConsultants, QueryString = queryString });
		}
		catch (Exception exception)
		{
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ConsultantController),
			          nameof(GetConsultants));

			return RedirectToAction(nameof(Index), "Home", new { });
		}
	}

	/// <summary>
	///     Gets the consultant details, owned contracts and related data.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpGet(Routes.Consultant.Get)]
	[HttpPost(Routes.Consultant.Get)]
	[IgnoreAntiforgeryToken]
	public async Task<IActionResult> GetConsultant(long id, CancellationToken cancellationToken)
	{
		try
		{
			// Retrieve the consultant with provided ID.
			GetConsultantQuery query = new(new Entity { Id = id }, false);
			var consultantEntity = await _sender.Send(query, cancellationToken);

			// Build the view model and return in to the consultant.
			if (consultantEntity == null)
			{
				return View(new GetConsultantViewModel(null));
			}

			var consultant = consultantEntity.Adapt<ConsultantRepresentation>();
			return View(new GetConsultantViewModel(consultant));
		}
		catch (Exception exception)
		{
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ConsultantController),
			          nameof(GetConsultants));

			return RedirectToAction(nameof(GetConsultants));
		}
	}

	/// <summary>
	///     Gets the create consultant form with no data.
	/// </summary>
	/// <returns></returns>
	[HttpGet(Routes.Consultant.Create)]
	public IActionResult CreateConsultant()
	{
		try
		{
			return View(new ConsultantCreateViewModel
			{
				Consultant = new ConsultantInput
				{
					DateBorn = MinimalDateBorn
				}
			});
		}
		catch (Exception exception)
		{
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ConsultantController),
			          nameof(CreateConsultant));

			return RedirectToAction(nameof(GetConsultants));
		}
	}

	/// <summary>
	///     Handles the requests made by the create consultant form, either creates the consultant in the persistence store
	///     or return the validation failures.
	/// </summary>
	/// <param name="consultantInput"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpPost(Routes.Consultant.Create)]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> CreateConsultant(
		[Bind(Prefix = nameof(ConsultantCreateViewModel.Consultant))] ConsultantInput consultantInput,
		CancellationToken cancellationToken)
	{
		try
		{
			// Validate and create the consultant in the persistence store.
			var command = consultantInput.Adapt<CreateConsultantCommand>();
			var entity = await _sender.Send(command, cancellationToken);

			// If the consultant creation is successful then redirect user to the consultant details page. 
			return RedirectToAction("GetConsultant", "Consultant", new { entity.Id });
		}
		catch (ValidationException exception)
		{
			// If there are any validation failures then return them with the create consultant form data.
			return View(nameof(CreateConsultant), new ConsultantCreateViewModel(consultantInput, exception));
		}
		catch (Exception exception)
		{
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ConsultantController),
			          nameof(CreateConsultant));

			return RedirectToAction(nameof(GetConsultants));
		}
	}

	/// <summary>
	///     Gets the update consultant form with no data.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpGet(Routes.Consultant.Update)]
	public async Task<IActionResult> UpdateConsultant(long id, CancellationToken cancellationToken)
	{
		var entity = new Entity { Id = id };

		try
		{
			// Retrieve the consultant with the provided ID.
			GetConsultantQuery query = new(entity, false);
			var consultantEntity = await _sender.Send(query, cancellationToken);

			// Build the view model and return in to the consultant.
			if (consultantEntity == null)
			{
				return RedirectToAction(nameof(GetConsultants), new { });
			}

			var consultant = consultantEntity.Adapt<ConsultantInput>();
			return View(new ConsultantUpdateViewModel(id, consultant));
		}
		catch (Exception exception)
		{
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ConsultantController),
			          nameof(UpdateConsultant));

			return RedirectToAction(nameof(GetConsultant), new { entity.Id });
		}
	}

	/// <summary>
	///     Handles requests made by the update consultant form, either updates the persisted consultant
	///     or returns the update consultant form with validation failures.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="consultantInput"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpPost(Routes.Consultant.Update)]
	public async Task<IActionResult> UpdateConsultant(long id,
	                                                  [Bind(Prefix = nameof(ConsultantCreateViewModel.Consultant))]
	                                                  ConsultantInput consultantInput,
	                                                  CancellationToken cancellationToken)
	{
		var entity = new Entity { Id = id };

		try
		{
			// Get consultant.
			var command = consultantInput.Adapt<UpdateConsultantCommand>() with { Id = id };
			await _sender.Send(command, cancellationToken);

			// Build the view model and return in to the consultant.
			return RedirectToAction(nameof(GetConsultant), new { entity.Id });
		}
		catch (ValidationException validationException)
		{
			// If there are any consultant model validation failures then include them with the form data in the view model.
			return View(new ConsultantUpdateViewModel(id, consultantInput, validationException));
		}
		catch (Exception exception)
		{
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ConsultantController),
			          nameof(UpdateConsultant));

			return RedirectToAction(nameof(GetConsultant), new { entity.Id });
		}
	}

	/// <summary>
	///     Handles the deletion of persisted consultant.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpPost(Routes.Consultant.Delete)]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteConsultant(long id, CancellationToken cancellationToken)
	{
		var entity = new Entity { Id = id };

		try
		{
			// Delete the persisted consultant.
			DeleteConsultantCommand command = new(entity);
			await _sender.Send(command, cancellationToken);

			// Redirect user to the consultant dashboard panel page.
			return RedirectToAction(nameof(GetConsultants));
		}
		catch (Exception exception)
		{
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ConsultantController),
			          nameof(DeleteConsultant));

			return RedirectToAction(nameof(GetConsultant), new { entity.Id });
		}
	}

	/// <summary>
	///     Handles requests made by the export to CSV file form.
	/// </summary>
	/// <param name="queryString"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpPost(Routes.Consultant.Export)]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ExportConsultants(ConsultantQueryString queryString, CancellationToken cancellationToken)
	{
		try
		{
			// Retrieve sorted, filtered and ordered consultant data set.
			GetConsultantRowsQuery query = new(queryString);
			var consultantRows = await _sender.Send(query, cancellationToken);

			// Data set transformation. 
			var content = new MemoryStream();
			await using StreamWriter streamWriter = new(content);
			await using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

			await csvWriter.WriteRecordsAsync(consultantRows, cancellationToken);

			// Return the requested data as CSV file.
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