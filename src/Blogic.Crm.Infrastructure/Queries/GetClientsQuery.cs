namespace Blogic.Crm.Infrastructure.Queries;

/// <summary>
///     Query the database for client records.
/// </summary>
public sealed record GetClientsQuery(ClientQueryString QueryString) : IQuery<PaginatedList<ClientRepresentation>>;

/// <summary>
///     Processes the <see cref="GetClientsQuery" /> query.
/// </summary>
public sealed class GetClientsQueryHandler : IQueryHandler<GetClientsQuery,PaginatedList<ClientRepresentation>>
{
    public GetClientsQueryHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    private readonly DataContext _dataContext;

    public async Task<PaginatedList<ClientRepresentation>> Handle(GetClientsQuery request,
        CancellationToken cancellationToken)
    {
        // Filter the client record and look for property matches.
        var clients = _dataContext.Clients.AsNoTracking()
            .Filter(request.QueryString)
            .Search(request.QueryString);

        // Save the number of results found.
        var totalItems = clients.Count();

        // Paginate the client record.
        var paginatedClients = await clients
            .Sort(request.QueryString)
            .Skip((request.QueryString.PageNumber - 1) * request.QueryString.PageSize)
            .Take(request.QueryString.PageSize)
            .ToListAsync(cancellationToken);

        // Map clients to the model to be exported.
        var exportableClients = paginatedClients.Adapt<List<ClientRepresentation>>();

        // Return paginated client data set in representative state.
        return new PaginatedList<ClientRepresentation>(
            exportableClients, totalItems, request.QueryString.PageNumber,
            request.QueryString.PageSize);
    }
}