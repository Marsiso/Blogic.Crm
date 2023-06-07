using static Blogic.Crm.Domain.Data.Entities.Contract;
using static System.String;

namespace Blogic.Crm.Web.Controllers;

/// <summary>
///     A controller that handles requests that are related to the contracts.
/// </summary>
public sealed class ContractController : Controller
{
    private readonly ISender _sender;

    public ContractController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    ///     Returns a view with the contract management panel.
    /// </summary>
    [HttpGet(Routes.Contract.GetAll)]
    [HttpPost(Routes.Contract.GetAll)]
    public async Task<IActionResult> GetContracts(ContractQueryString queryString,
        CancellationToken cancellationToken)
    {
        try
        {
            // Retrieving contracts from the database.
            GetContractsQuery query = new(queryString);
            var paginatedContracts = await _sender.Send(query, cancellationToken);

            // Response to request.
            return View(new GetContractsViewModel { Contracts = paginatedContracts, QueryString = queryString });
        }
        catch (Exception exception)
        {
            // Exception catching and logging.
            Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
                exception.Message,
                nameof(ContractController),
                nameof(GetContracts));

            return RedirectToAction(nameof(Index), "Home");
        }
    }

    /// <summary>
    ///     Returns a view with the contract details.
    /// </summary>
    [HttpGet(Routes.Contract.Get)]
    [HttpPost(Routes.Contract.Get)]
    public async Task<IActionResult> GetContract(long id, CancellationToken cancellationToken)
    {
        try
        {
            // Represents an identifiable entity in the database.
            var entity = new Entity(id);

            // Retrieving contract from the database.
            GetContractQuery contractQuery = new(entity, false);
            var contractEntity = await _sender.Send(contractQuery, cancellationToken);

            // When the contract is not found then respond to the request.
            if (contractEntity is null) return View(new GetContractViewModel());

            // Retrieving contract owner from the database.
            GetClientQuery clientQuery = new(new Entity { Id = contractEntity.ClientId }, false);
            var clientEntity = await _sender.Send(clientQuery, cancellationToken);

            // When the contract is not found then respond to the request.
            if (contractEntity.ManagerId.HasValue is false) return RedirectToAction(nameof(GetContracts));

            // Retrieving contract manager from the database.
            GetConsultantQuery consultantQuery = new(new Entity { Id = contractEntity.ManagerId.Value }, false);
            var consultantEntity = await _sender.Send(consultantQuery, cancellationToken);

            // Retrieving contract consultants from the database.
            GetContractConsultantsQuery consultantsQuery = new(entity);
            var contractConsultants = await _sender.Send(consultantsQuery, cancellationToken);

            // Mapping data to exportation models.
            var contract = contractEntity.Adapt<ContractRepresentation>();
            var client = clientEntity?.Adapt<ClientRepresentation>();
            var consultant = consultantEntity?.Adapt<ConsultantRepresentation>();

            // Response to request.
            return View(new GetContractViewModel(contract, client, consultant, contractConsultants));
        }
        catch (Exception exception)
        {
            // Exception catching and logging.
            Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
                exception.Message,
                nameof(ContractController),
                nameof(GetContract));

            return RedirectToAction(nameof(GetContracts));
        }
    }

    /// <summary>
    ///     Returns a view with the contract registration form.
    /// </summary>
    [HttpGet(Routes.Contract.Create)]
    public IActionResult CreateContract()
    {
        try
        {
            // Response to request.
            return View(new CreateContractViewModel(new ContractInput(MinimalDateConcluded, MinimalDateConcluded,
                MinimalDateConcluded)));
        }
        catch (Exception exception)
        {
            // Exception catching and logging.
            Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
                exception.Message,
                nameof(ContractController),
                nameof(CreateContract));

            return RedirectToAction(nameof(GetContracts));
        }
    }

    /// <summary>
    ///     Returns a view with the contract registration form.
    /// </summary>
    [HttpPost(Routes.Contract.Create)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateContract(
        [Bind(Prefix = nameof(CreateContractViewModel.Contract))]
        ContractInput contractInput,
        CancellationToken cancellationToken)
    {
        try
        {
            // Create a contract in the database.
            var command = contractInput.Adapt<CreateContractCommand>();
            var entity = await _sender.Send(command, cancellationToken);

            // If the contract is successfully registered, display his details. 
            return RedirectToAction(nameof(GetContract), new { entity.Id });
        }
        catch (ValidationException exception)
        {
            // In case of validation errors in the contract registration form, return the form with validation errors.
            return View(new CreateContractViewModel(contractInput, exception));
        }
        catch (Exception exception)
        {
            // Exception catching and logging.
            Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
                exception.Message,
                nameof(ContractController),
                nameof(CreateContract));

            return RedirectToAction(nameof(GetContracts));
        }
    }

    /// <summary>
    ///     Returns a view with the contract update form.
    /// </summary>
    [HttpGet(Routes.Contract.Update)]
    public async Task<IActionResult> UpdateContract(long id, CancellationToken cancellationToken)
    {
        try
        {
            // Retrieving contract from the database.
            var entity = new Entity(id);
            
            // Get the contract with the provided ID.
            GetContractQuery contractQuery = new(entity, false);
            var contractEntity = await _sender.Send(contractQuery, cancellationToken);

            // When the contract is not found then respond to the request with redirect to the contract management panel.
            if (contractEntity == null) return RedirectToAction(nameof(GetContracts), new { });

            // Retrieving contract consultants from the database.
            GetContractConsultantsQuery contractConsultantsQuery = new(entity);
            var contractConsultants = await _sender.Send(contractConsultantsQuery, cancellationToken);

            // Mapping contract to the model to be exported.
            var contractInput = contractEntity.Adapt<ContractInput>();
            var consultantIds = contractConsultants
                .Where(c => c.Id != contractEntity.ManagerId)
                .Select(c => c.Id);

            contractInput.ConsultantIds = Join(", ", consultantIds);

            // Response to request.
            return View(new UpdateContractViewModel(id, contractInput));
        }
        catch (Exception exception)
        {
            // Exception catching and logging.
            Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
                exception.Message,
                nameof(ContractController),
                nameof(UpdateContract));

            return RedirectToAction(nameof(GetContracts));
        }
    }

    /// <summary>
    ///     Returns a view with the contract update form and its validation failures if any.
    /// </summary>
    [HttpPost(Routes.Contract.Update)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateContract(long id,
        [Bind(Prefix = nameof(CreateContractViewModel.Contract))]
        ContractInput contractInput,
        CancellationToken cancellationToken)
    {
        try
        {
            // Represents an identifiable entity in the database.
            var entity = new Entity(id);
            
            // Update contract in the database.
            var command = contractInput.Adapt<UpdateContractCommand>() with { Entity = entity };
            await _sender.Send(command, cancellationToken);

            // If the contract is successfully updated, display his details.
            return RedirectToAction(nameof(GetContract), new { id });
        }
        catch (ValidationException exception)
        {
            // In case of validation errors in the contract update form, return the form with validation errors.
            return View(new UpdateContractViewModel(id, contractInput, exception));
        }
        catch (Exception exception)
        {
            // Exception catching and logging.
            Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
                exception.Message,
                nameof(ContractController),
                nameof(DeleteContract));

            return RedirectToAction(nameof(GetContracts));
        }
    }

    /// <summary>
    ///     Handles the form for deleting contracts in the database.
    /// </summary>
    [HttpPost(Routes.Contract.Delete)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteContract(long id, CancellationToken cancellationToken)
    {
        try
        {
            // Represents an identifiable entity in the database.
            var entity = new Entity(id);
            
            // Delete the contract in the database.
            DeleteContractCommand command = new(entity);
            await _sender.Send(command, cancellationToken);

            // If the contract is successfully deleted, display the contract management panel.
            return RedirectToAction(nameof(GetContracts));
        }
        catch (Exception exception)
        {
            // Exception catching and logging.
            Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
                exception.Message,
                nameof(ContractController),
                nameof(DeleteContract));

            return RedirectToAction(nameof(GetContracts));
        }
    }

    /// <summary>
    ///     Handles the form for exporting contract records to CSV.
    /// </summary>
    [HttpPost(Routes.Contract.Export)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ExportContracts(ContractQueryString queryString,
        CancellationToken cancellationToken)
    {
        try
        {
            // Retrieving contracts from the database.
            GetContractsToExportQuery query = new(queryString);
            var contracts = await _sender.Send(query, cancellationToken);

            // Export data set to CSV file. 
            var content = new MemoryStream();
            await using StreamWriter streamWriter = new(content);
            await using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

            await csvWriter.WriteRecordsAsync(contracts, cancellationToken);

            // Return the requested file.
            return File(content.GetBuffer(), MediaTypeNames.Application.Octet, "contracts.csv");
        }
        catch (Exception exception)
        {
            // Exception catching and logging.
            Log.Error(exception, "{Message}. Controller: '{Controller}'. Action: '{Action}'",
                exception.Message,
                nameof(ContractController),
                nameof(ExportContracts));

            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}