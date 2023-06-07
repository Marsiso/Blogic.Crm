namespace Blogic.Crm.Infrastructure.Queries;

/// <summary>
///     Query the database for contract records consulted by contract consultant in exportable form.
/// </summary>
public sealed record GetContractsConsultedQuery(Entity Entity) : IQuery<List<ContractRepresentation>>;

/// <summary>
///     Processes the <see cref="GetContractsConsultedQuery" /> query.
/// </summary>
public sealed class GetContractsConsultedQueryHandler
    : IQueryHandler<GetContractsConsultedQuery, List<ContractRepresentation>>
{
    private readonly DataContext _dataContext;

    public GetContractsConsultedQueryHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<List<ContractRepresentation>> Handle(GetContractsConsultedQuery request,
        CancellationToken cancellationToken)
    {
        // Get contract consultants from the database.
        var contractConsultants = await _dataContext.ContractConsultants
            .Where(c => c.ConsultantId == request.Entity.Id)
            .ToListAsync(cancellationToken);

        // Mapping contract consultants to the model to be exported.
        return contractConsultants.Adapt<List<ContractRepresentation>>();
    }
}