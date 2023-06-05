using System.Globalization;
using System.Net.Mime;
using Blogic.Crm.Web.Views.Consultant;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Serilog;

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

			// Build the view model and return in to the consultant.
			return View(new GetConsultantsViewModel(paginatedConsultants, queryString));
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


	[HttpGet(Routes.Consultant.Get)]
	[HttpPost(Routes.Consultant.Get)]
	[IgnoreAntiforgeryToken]
	public async Task<IActionResult> GetConsultant(long id, CancellationToken cancellationToken)
	{
		try
		{
			// Retrieve paginated consultant representation for the view model.
			GetConsultantByIdQuery query = new(id, false);
			var consultantEntity = await _sender.Send(query, cancellationToken);


			// Build the view model and return in to the consultant.
			if (consultantEntity is null)
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
			          nameof(GetConsultant));

			return RedirectToAction(nameof(GetConsultants));
		}
	}


	[HttpGet(Routes.Consultant.Create)]
	public IActionResult CreateConsultant(CancellationToken cancellationToken)
	{
		try
		{
			return View(new ConsultantCreateViewModel());
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

	[HttpPost(Routes.Consultant.Create)]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> CreateConsultant(ConsultantUpdateViewModel viewModel,
	                                                  CancellationToken cancellationToken)
	{
		try
		{
			// Validate and create the consultant in the persistence store.
			var command = viewModel.Consultant.Adapt<CreateConsultantCommand>();
			var entity = await _sender.Send(command, cancellationToken);

			// If the consultant creation is successful then redirect user to the consultant details page. 
			return RedirectToAction(nameof(GetConsultant), new { entity.Id });
		}
		catch (ValidationException exception)
		{
			// If there are any validation failures then return them with the create consultant form data.
			return View(new ConsultantCreateViewModel(viewModel.Consultant, exception));
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

	[HttpGet(Routes.Consultant.Update)]
	public async Task<IActionResult> UpdateConsultant(long id, string originAction, CancellationToken cancellationToken)
	{
		try
		{
			// Retrieve the consultant with the provided ID.
			GetConsultantByIdQuery query = new(id, false);
			var consultantEntity = await _sender.Send(query, cancellationToken);

			// Build the view model and return it to the client.
			if (consultantEntity == null)
			{
				return RedirectToAction(nameof(GetConsultants), new { });
			}

			var consultantInput = consultantEntity.Adapt<ConsultantInput>();
			return View(new ConsultantUpdateViewModel(id, originAction, consultantInput));
		}
		catch (Exception exception)
		{
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ConsultantController),
			          nameof(UpdateConsultant));

			return RedirectToAction(nameof(GetConsultants));
		}
	}

	[HttpPost(Routes.Consultant.Update)]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> UpdateConsultant(long id, string originAction, ConsultantUpdateViewModel viewModel,
	                                                  CancellationToken cancellationToken)
	{
		try
		{
			// Validate and create the consultant in the persistence store.
			var command = viewModel.Consultant.Adapt<UpdateConsultantCommand>() with { Id = id };
			await _sender.Send(command, cancellationToken);

			// If the consultant creation is successful then redirect user to the consultant details page. 
			return RedirectToAction(nameof(GetConsultant), new { id });
		}
		catch (ValidationException exception)
		{
			// If there are any validation failures then return them with the create consultant form data.
			return View(new ConsultantUpdateViewModel(id, originAction, viewModel.Consultant, exception));
		}
		catch (Exception exception)
		{
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ConsultantController),
			          nameof(UpdateConsultant));

			return RedirectToAction(nameof(GetConsultants));
		}
	}

	[HttpGet(Routes.Consultant.DeletePrompt)]
	public async Task<IActionResult> DeleteConsultantPrompt(long id, string originAction,
	                                                        CancellationToken cancellationToken)
	{
		try
		{
			// Retrieve the persisted consultant by the provided ID.
			GetConsultantByIdQuery query = new(id, false);
			var consultantEntity = await _sender.Send(query, cancellationToken);

			// Build the view model and return in to the consultant.
			if (consultantEntity == null)
			{
				return View(new ConsultantDeleteViewModel(null, originAction));
			}

			var consultant = consultantEntity.Adapt<ConsultantRepresentation>();
			return View(new ConsultantDeleteViewModel(consultant, originAction));
		}
		catch (Exception exception)
		{
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ConsultantController),
			          nameof(UpdateConsultant));

			return RedirectToAction(nameof(GetConsultants));
		}
	}

	[HttpPost(Routes.Consultant.Delete)]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteConsultant(long id, CancellationToken cancellationToken)
	{
		try
		{
			// Delete the persisted consultant.
			DeleteConsultantCommand command = new(id);
			await _sender.Send(command, cancellationToken);
			
			// Redirect user to the consultant dashboard panel page.
			return RedirectToAction(nameof(GetConsultants));
		}
		catch (Exception exception)
		{
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ConsultantController),
			          nameof(UpdateConsultant));

			return RedirectToAction(nameof(GetConsultants));
		}
	}

	[HttpPost(Routes.Consultant.Export)]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ExportConsultants(ConsultantQueryString queryString,
	                                                   CancellationToken cancellationToken)
	{
		try
		{
			// Retrieve sorted, filtered and ordered client data set.
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