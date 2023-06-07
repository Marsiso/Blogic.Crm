namespace Blogic.Crm.Infrastructure.Commands;

/// <summary>
///     The command to remove the contract from the database.
/// </summary>
/// <param name="Entity"></param>
public sealed record DeleteContractCommand(Entity Entity) : ICommand<Unit>;

public sealed class DeleteContractCommandHandler : ICommandHandler<DeleteContractCommand, Unit>
{
    private readonly DataContext _dataContext;

    public DeleteContractCommandHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<Unit> Handle(DeleteContractCommand request, CancellationToken cancellationToken)
    {
        // Get the contract from the database.
        var contractEntity = await _dataContext.Contracts
            .AsTracking()
            .SingleOrDefaultAsync(c => c.Id == request.Entity.Id, cancellationToken);

        // If the contract is not found, do not perform any action and return,
        // otherwise remove the client from the database and save the changes.
        if (contractEntity is null) return Unit.Value;

        _dataContext.Contracts.Remove(contractEntity);
        await _dataContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}