namespace Blogic.Crm.Infrastructure.Queries;

/// <summary>
///     Query the database for contract records owned by client in exportable form.
/// </summary>
public sealed record GetContractsOwnedQuery(Entity Entity) : IQuery<List<ContractRepresentation>>;

/// <summary>
///     Processes the <see cref="GetContractsOwnedQuery" /> query.
/// </summary>
public sealed class GetContractsOwnedQueryHandler : IQueryHandler<GetContractsOwnedQuery, List<ContractRepresentation>>
{
    private readonly DataContext _dataContext;

    public GetContractsOwnedQueryHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<List<ContractRepresentation>> Handle(GetContractsOwnedQuery request,
        CancellationToken cancellationToken)
    {
        // Get contracts from the database.
        var contracts = await _dataContext.Contracts
            .Where(c => c.ClientId == request.Entity.Id)
            .ToListAsync(cancellationToken);

        // Mapping contracts to the model to be exported.
        return contracts.Adapt<List<ContractRepresentation>>();
    }
}