namespace Blogic.Crm.Infrastructure.Queries;

/// <summary>
///     Query the database for contract records managed by contract manager in exportable form.
/// </summary>
public sealed record GetContractsManagedQuery(Entity Entity) : IQuery<List<ContractRepresentation>>;

/// <summary>
///     Processes the <see cref="GetContractsManagedQuery" /> query.
/// </summary>
public sealed class GetContractsManagedQueryHandler
    : IQueryHandler<GetContractsManagedQuery, List<ContractRepresentation>>
{
    private readonly DataContext _dataContext;

    public GetContractsManagedQueryHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<List<ContractRepresentation>> Handle(GetContractsManagedQuery request,
        CancellationToken cancellationToken)
    {
        // Get contracts from the database.
        var contracts = await _dataContext.Contracts
            .Where(c => c.ManagerId == request.Entity.Id)
            .ToListAsync(cancellationToken);

        // Mapping contracts to the model to be exported.
        return contracts.Adapt<List<ContractRepresentation>>();
    }
}