namespace Blogic.Crm.Infrastructure.Queries;

/// <summary>
///     Query the database for client records in exportable form.
/// </summary>
public sealed record GetConsultantsToExportQuery(ConsultantQueryString QueryString) : IQuery<List<ConsultantToExport>>;

/// <summary>
///     Processes the <see cref="GetConsultantsToExportQuery" /> query.
/// </summary>
public sealed class GetConsultantsToExportQueryHandler : IQueryHandler<GetConsultantsToExportQuery, List<ConsultantToExport>>
{
    private readonly DataContext _dataContext;

    public GetConsultantsToExportQueryHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<List<ConsultantToExport>> Handle(GetConsultantsToExportQuery request,
        CancellationToken cancellationToken)
    {
        // Get consultants from the database.
        var consultants = await _dataContext.Consultants.AsNoTracking()
            .Filter(request.QueryString)
            .Search(request.QueryString)
            .Sort(request.QueryString)
            .ToListAsync(cancellationToken);

        // Mapping consultants to the model to be exported.
        return consultants.Adapt<List<ConsultantToExport>>();
    }
}