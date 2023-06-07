namespace Blogic.Crm.Infrastructure.Queries;

/// <summary>
///     Query the database for client record.
/// </summary>
public sealed record GetClientQuery(Entity Entity, bool TrackChanges) : IQuery<Client?>;

/// <summary>
///     Processes the <see cref="GetClientQuery" /> query.
/// </summary>
public sealed class GetClientQueryHandler : IQueryHandler<GetClientQuery, Client?>
{
    public GetClientQueryHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    private readonly DataContext _dataContext;

    public Task<Client?> Handle(GetClientQuery request, CancellationToken cancellationToken)
    {
        // Get client from the database.
        return request.TrackChanges
            ? _dataContext.Clients
                .AsTracking()
                .SingleOrDefaultAsync(c => c.Id == request.Entity.Id, cancellationToken)
            : _dataContext.Clients
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == request.Entity.Id, cancellationToken);
    }
}