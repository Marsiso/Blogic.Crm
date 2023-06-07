namespace Blogic.Crm.Infrastructure.Queries;

/// <summary>
///     Query the database for contract records in exportable form.
/// </summary>
public record GetContractsToExportQuery(ContractQueryString QueryString) : IQuery<List<ContractToExport>>;

/// <summary>
///     Processes the <see cref="GetContractsToExportQueryHandler" /> query.
/// </summary>
public sealed class GetContractsToExportQueryHandler : IQueryHandler<GetContractsToExportQuery, List<ContractToExport>>
{
    private readonly DataContext _dataContext;

    public GetContractsToExportQueryHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<List<ContractToExport>> Handle(GetContractsToExportQuery request,
        CancellationToken cancellationToken)
    {
        // Get contracts from the database.
        var contracts = await _dataContext.Contracts.AsNoTracking()
            .Filter(request.QueryString)
            .Search(request.QueryString)
            .Sort(request.QueryString)
            .ToListAsync(cancellationToken);

        // Mapping contracts to the model to be exported.
        return contracts.Adapt<List<ContractToExport>>();
    }
}