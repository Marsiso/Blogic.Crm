namespace Blogic.Crm.Infrastructure.Queries;

/// <summary>
///     Query the database for client record.
/// </summary>
public sealed record GetContractsQuery(ContractQueryString QueryString) : IQuery<PaginatedList<ContractRepresentation>>;

/// <summary>
///     Processes the <see cref="GetContractsQuery" /> query.
/// </summary>
public sealed class GetContractsQueryHandler : IQueryHandler<GetContractsQuery, PaginatedList<ContractRepresentation>>
{
    private readonly DataContext _dataContext;

    public GetContractsQueryHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<PaginatedList<ContractRepresentation>> Handle(GetContractsQuery request,
        CancellationToken cancellationToken)
    {
        // Get contracts from the database.
        var contracts = _dataContext.Contracts
            .AsNoTracking()
            .Filter(request.QueryString)
            .Search(request.QueryString);

        // Save the number of results found.
        var totalItems = contracts.Count();
        
        // Paginate the contract record.
        var paginatedContracts = await contracts
            .Sort(request.QueryString)
            .Skip((request.QueryString.PageNumber - 1) * request.QueryString.PageSize)
            .Take(request.QueryString.PageSize)
            .ToListAsync(cancellationToken);

        // Map contracts to the model to be exported.
        var contractRepresentations = paginatedContracts.Adapt<List<ContractRepresentation>>();
        
        // Return paginated contract data set in representative state.
        return new PaginatedList<ContractRepresentation>(contractRepresentations, totalItems,
            request.QueryString.PageNumber, request.QueryString.PageSize);
    }
}