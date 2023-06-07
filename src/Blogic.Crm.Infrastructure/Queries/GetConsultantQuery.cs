namespace Blogic.Crm.Infrastructure.Queries;

/// <summary>
///     Query the database for consultant record.
/// </summary>
public record GetConsultantQuery(Entity Entity, bool TrackChanges) : IQuery<Consultant?>;

/// <summary>
///     Processes the <see cref="GetConsultantQuery" /> query.
/// </summary>
public sealed class GetConsultantQueryHandler : IQueryHandler<GetConsultantQuery, Consultant?>
{
    public GetConsultantQueryHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    private readonly DataContext _dataContext;

    public Task<Consultant?> Handle(GetConsultantQuery request, CancellationToken cancellationToken)
    {
        // Get consultant from the database.
        return request.TrackChanges
            ? _dataContext.Consultants
                .AsTracking()
                .SingleOrDefaultAsync(c => c.Id == request.Entity.Id, cancellationToken)
            : _dataContext.Consultants
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == request.Entity.Id, cancellationToken);
    }
}