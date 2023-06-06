using System.Globalization;
using System.Net.Mime;
using Blogic.Crm.Web.Views.Contract;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Blogic.Crm.Web.Controllers;

public sealed class ContractController : Controller
{
	private readonly ISender _sender;

	public ContractController(ISender sender)
	{
		_sender = sender;
	}

	[HttpGet(Routes.Contract.GetAll)]
	[HttpPost(Routes.Contract.GetAll)]
	public async Task<IActionResult> GetContracts(ContractQueryString queryString,
	                                              CancellationToken cancellationToken)
	{
		try
		{
			// Retrieve paginated contract representation for the view model.
			GetContractRepresentationsQuery query = new(queryString);
			PaginatedList<ContractRepresentation> paginatedContracts = await _sender.Send(query, cancellationToken);
		
			// Build the view model and return it to the contract.
			return View(new GetContractsViewModel { Contracts  = paginatedContracts, QueryString = queryString });
		}
		catch (Exception exception)
		{
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ContractController),
			          nameof(GetContracts));

			return RedirectToAction(nameof(Index), "Home", new { });
		}
	}

	[HttpGet(Routes.Contract.Get)]
	[HttpPost(Routes.Contract.Get)]
	public async Task<IActionResult> GetContract(long id, CancellationToken cancellationToken)
	{
		try
		{
			// Retrieve contract representation for the view model.
			GetContractByIdQuery contractQuery = new(id, false);
			Contract? contractEntity = await _sender.Send(contractQuery, cancellationToken);
			
			// Build the view model and return in to the contract.
			if (contractEntity is null)
			{
				return View(new GetContractViewModel());
			}
			
			// Retrieve client representation for the view model.
			GetClientByIdQuery clientQuery = new(contractEntity.ClientId, false);
			Client? clientEntity = await _sender.Send(clientQuery, cancellationToken);
			
			// Retrieve consultant representation for the view model.
			if (contractEntity.ManagerId.HasValue is false)
			{
				return RedirectToAction(nameof(Index), "Home", new { });
			}

			GetConsultantByIdQuery consultantQuery = new(contractEntity.ManagerId.Value, false);
			Consultant? consultantEntity = await _sender.Send(consultantQuery, cancellationToken);
			
			// Map data to their respective representations.
			ContractRepresentation contract = contractEntity.Adapt<ContractRepresentation>();
			ClientRepresentation? client = clientEntity?.Adapt<ClientRepresentation>();
			ConsultantRepresentation? consultant = consultantEntity?.Adapt<ConsultantRepresentation>();
			
			return View(new GetContractViewModel(contract, client, consultant));
		}
		catch (Exception exception)
		{
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ContractController),
			          nameof(GetContract));

			return RedirectToAction(nameof(Index), "Home", new { });
		}
	}

	[HttpGet(Routes.Contract.Create)]
	public IActionResult CreateContract()
	{
		try
		{
			return View(new CreateContractViewModel
			{
				Contract = new ContractInput
				{
					DateConcluded = Contract.MinimalDateConcluded, 
					DateValid = Contract.MinimalDateConcluded,
					DateExpired = Contract.MinimalDateConcluded
				}
			});
		}
		catch (Exception exception)
		{
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ContractController),
			          nameof(CreateContract));

			return RedirectToAction(nameof(GetContracts));
		}
	}
	
	[HttpPost(Routes.Contract.Create)]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> CreateContract([Bind(Prefix = nameof(CreateContractViewModel.Contract))] ContractInput contractInput, CancellationToken cancellationToken)
	{
		try
		{
			// Validate and create the contract in the persistence store.
			var command = contractInput.Adapt<CreateContractCommand>();
			var entity = await _sender.Send(command, cancellationToken);

			// If the contract creation is successful then redirect user to the contract details page. 
			return RedirectToAction(nameof(GetContract), new { entity.Id });
		}
		catch (ValidationException exception)
		{
			// If there are any validation failures then return them with the create contract form data.
			return View(new CreateContractViewModel{ Contract = contractInput, ValidationException = exception });
		}
		catch (Exception exception)
		{
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ContractController),
			          nameof(CreateContract));

			return RedirectToAction(nameof(GetContracts));
		}
	}

	[HttpGet(Routes.Contract.Update)]
	public async Task<IActionResult> UpdateContract(long id, CancellationToken cancellationToken)
	{
		try
		{
			// Retrieve the contract with the provided ID.
			GetContractByIdQuery query = new(id, false);
			var contractEntity = await _sender.Send(query, cancellationToken);

			// Build the view model and return it to the contract.
			if (contractEntity == null)
			{
				return RedirectToAction(nameof(GetContracts), new { });
			}

			var contractInput = contractEntity.Adapt<ContractInput>();
			return View(new UpdateContractViewModel(id, contractInput));
		}
		catch (Exception exception)
		{
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ContractController),
			          nameof(UpdateContract));

			return RedirectToAction(nameof(GetContracts));
		}
	}
	
	[HttpPost(Routes.Contract.Update)]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> UpdateContract(long id,
	                                                [Bind(Prefix = nameof(CreateContractViewModel.Contract))] ContractInput contractInput,
	                                                CancellationToken cancellationToken)
	{
		try
		{
			// Retrieve the contract with the provided ID.
			UpdateContractCommand command = contractInput.Adapt<UpdateContractCommand>() with { Id = id };
			await _sender.Send(command, cancellationToken);

			return RedirectToAction(nameof(GetContract), new { id });
		}
		catch (ValidationException exception)
		{
			// If there are any validation failures then return them with the create contract form data.
			return View(new UpdateContractViewModel(id, contractInput, exception));
		}
		catch (Exception exception)
		{
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ContractController),
			          nameof(UpdateContract));

			return RedirectToAction(nameof(GetContracts));
		}
	}
	

	public IActionResult DeleteContractPrompt()
	{
		throw new NotImplementedException();
	}

	[HttpPost(Routes.Contract.Export)]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ExportContracts(ContractQueryString queryString, CancellationToken cancellationToken)
	{
		try
		{
			// Retrieve paginated contract representation for the view model.
			GetContractRowsQuery query = new(queryString);
			IEnumerable<ContractRow> contracts = await _sender.Send(query, cancellationToken);

			// Data set transformation. 
			var content = new MemoryStream();
			await using StreamWriter streamWriter = new(content);
			await using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

			await csvWriter.WriteRecordsAsync(contracts, cancellationToken);

			// Return the requested data as CSV file.
			return File(content.GetBuffer(), MediaTypeNames.Application.Octet, "contracts.csv");
		}
		catch (Exception exception)
		{
			Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
			          exception.Message,
			          nameof(ContractController),
			          nameof(ExportContracts));

			return new StatusCodeResult(StatusCodes.Status500InternalServerError);
		}
	}
}