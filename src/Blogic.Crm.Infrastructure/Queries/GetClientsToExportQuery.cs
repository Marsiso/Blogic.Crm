namespace Blogic.Crm.Infrastructure.Queries;

/// <summary>
///     Query the database for client records in exportable form.
/// </summary>
public record GetClientsToExportQuery(ClientQueryString QueryString) : IQuery<List<ClientToExport>>;

/// <summary>
///     Processes the <see cref="GetClientsToExportQuery" /> query.
/// </summary>
public sealed class GetClientsToExportQueryHandler : IQueryHandler<GetClientsToExportQuery, List<ClientToExport>>
{
    public GetClientsToExportQueryHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    private readonly DataContext _dataContext;

    public async Task<List<ClientToExport>> Handle(GetClientsToExportQuery request, CancellationToken cancellationToken)
    {
        // Get clients from the database.
        var clientEntities = await _dataContext.Clients.AsNoTracking()
            .Filter(request.QueryString)
            .Search(request.QueryString)
            .Sort(request.QueryString)
            .ToListAsync(cancellationToken);

        // Mapping clients to the model to be exported.
        return clientEntities.Adapt<List<ClientToExport>>();
    }
}