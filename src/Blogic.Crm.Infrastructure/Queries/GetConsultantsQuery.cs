namespace Blogic.Crm.Infrastructure.Queries;

/// <summary>
///     Query the database for consultant records.
/// </summary>
public sealed record GetConsultantsQuery(ConsultantQueryString QueryString) : IQuery<PaginatedList<ConsultantRepresentation>>;

/// <summary>
///     Processes the <see cref="GetConsultantsQuery" /> query.
/// </summary>
public sealed class GetConsultantsQueryHandler : IQueryHandler<GetConsultantsQuery,PaginatedList<ConsultantRepresentation>>
{
    public GetConsultantsQueryHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    private readonly DataContext _dataContext;

    public async Task<PaginatedList<ConsultantRepresentation>> Handle(GetConsultantsQuery request,
        CancellationToken cancellationToken)
    {
        // Get consultants from the database.
        var consultants = _dataContext.Consultants.AsNoTracking()
            .Filter(request.QueryString)
            .Search(request.QueryString);

        // Save the number of results found.
        var totalItems = consultants.Count();

        // Paginate the consultant record.
        var paginatedConsultants = await consultants
            .Sort(request.QueryString)
            .Skip((request.QueryString.PageNumber - 1) * request.QueryString.PageSize)
            .Take(request.QueryString.PageSize)
            .ToListAsync(cancellationToken);

        // Map clients to the model to be exported.
        var exportableConsultants = paginatedConsultants.Adapt<List<ConsultantRepresentation>>();

        // Return paginated consultant data set in representative state.
        return new PaginatedList<ConsultantRepresentation>(
            exportableConsultants, totalItems, request.QueryString.PageNumber,
            request.QueryString.PageSize);
    }
}