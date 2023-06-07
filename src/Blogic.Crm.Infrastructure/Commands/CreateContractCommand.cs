namespace Blogic.Crm.Infrastructure.Commands;

/// <summary>
///     The command to add a contract to the database.
/// </summary>
public sealed record CreateContractCommand(string RegistrationNumber, string Institution, DateTime DateConcluded,
    DateTime DateExpired, DateTime DateValid, long ClientId, long ManagerId,
    string ConsultantIds) : ICommand<Entity>;

/// <summary>
///     Processes the <see cref="CreateContractCommand" /> command.
/// </summary>
public sealed class CreateContractCommandHandler : ICommandHandler<CreateContractCommand, Entity>
{
    private readonly DataContext _dataContext;

    public CreateContractCommandHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public Task<Entity> Handle(CreateContractCommand request, CancellationToken cancellationToken)
    {
        // Map input data to the contract model.
        var contract = request.Adapt<Contract>();

        // Create a contract in the database.
        _dataContext.Contracts.Add(contract);

        // Apply changes.
        _dataContext.SaveChanges();

        // Add a manager as a contract consultant.
        var manager = new ContractConsultant
        {
            ContractId = contract.Id,
            ConsultantId = request.ManagerId
        };

        // Create a consultant in the database.
        _dataContext.ContractConsultants.Add(manager);

        // Create models of the links between consultants and the contract.
        var consultants = ParseNumbersToInt64(request.ConsultantIds)
            .Where(consultantId => consultantId != request.ManagerId)
            .Select(consultantId => new ContractConsultant(consultantId, contract.Id));

        // Create a consultants in the database.
        _dataContext.ContractConsultants.AddRange(consultants);

        // Apply changes.
        _dataContext.SaveChanges();

        // Return the identifier of the created consultant.
        return Task.FromResult((Entity)contract);
    }
}