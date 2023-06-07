namespace Blogic.Crm.Infrastructure.Queries;

/// <summary>
///     Query the database for contract record.
/// </summary>
public sealed record GetContractQuery(Entity Entity, bool TrackChanges) : IQuery<Contract?>;

/// <summary>
///     Processes the <see cref="GetContractQuery" /> query.
/// </summary>
public sealed class GetContractQueryHandler : IQueryHandler<GetContractQuery, Contract?>
{
    private readonly DataContext _dataContext;

    public GetContractQueryHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public Task<Contract?> Handle(GetContractQuery request, CancellationToken cancellationToken)
    {
        // Get contract from the database.
        return request.TrackChanges
            ? _dataContext.Contracts.AsTracking()
                .SingleOrDefaultAsync(c => c.Id == request.Entity.Id, cancellationToken)
            : _dataContext.Contracts.AsTracking()
                .SingleOrDefaultAsync(c => c.Id == request.Entity.Id, cancellationToken);
    }
}