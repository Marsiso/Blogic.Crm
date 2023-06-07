namespace Blogic.Crm.Infrastructure.Commands;

/// <summary>
///     The command to update the contract in the database.
/// </summary>
public sealed record UpdateContractCommand(Entity Entity, string RegistrationNumber, string Institution,
    DateTime DateConcluded, DateTime DateExpired, DateTime DateValid, long ClientId, long ManagerId,
    string ConsultantIds) : ICommand<Unit>;

/// <summary>
///     Processes the <see cref="UpdateContractCommand" /> command.
/// </summary>
public sealed class UpdateContractCommandHandler : ICommandHandler<UpdateContractCommand, Unit>
{
    private readonly DataContext _dataContext;

    public UpdateContractCommandHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<Unit> Handle(UpdateContractCommand request, CancellationToken cancellationToken)
    {
        // Get the client from the database.
        var contract = await _dataContext.Contracts
            .AsTracking()
            .SingleOrDefaultAsync(c => c.Id == request.Entity.Id, cancellationToken);

        // If the client is not found, do not perform any action and return,
        // otherwise remove the client from the database and save the changes.
        if (contract is null) return Unit.Value;

        // Map input data to the client model.
        request.Adapt(contract);

        // Update a client in the database.
        _dataContext.Contracts.Update(contract);

        // Create models of the links between consultants and the contract to be updated.
        var current = ParseNumbersToInt64(request.ConsultantIds)
            .Where(c => c != request.ManagerId)
            .Select(consultantIdentifier =>
                new ContractConsultant(consultantIdentifier, contract.Id))
            .ToList();

        // Get contract consultants to be updated in the database.
        var original = await _dataContext.ContractConsultants
            .Where(ccOriginal => ccOriginal.ContractId == request.Entity.Id &&
                                 ccOriginal.ConsultantId != request.ManagerId)
            .ToListAsync(cancellationToken);

        // Get contract consultants to be deleted in the database.
        var toDelete = original.Where(ccOriginal =>
            current.All(ccCurrent => ccCurrent.ConsultantId != ccOriginal.ConsultantId));

        _dataContext.ContractConsultants.RemoveRange(toDelete);

        // Get contract consultants to be created in the database.
        var toCreate = current.Where(ccCurrent =>
            original.All(ccOriginal => ccOriginal.ConsultantId != ccCurrent.ConsultantId));

        _dataContext.ContractConsultants.AddRange(toCreate);

        // Apply changes.
        await _dataContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}