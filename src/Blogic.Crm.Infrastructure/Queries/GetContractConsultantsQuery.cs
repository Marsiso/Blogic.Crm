namespace Blogic.Crm.Infrastructure.Queries;

/// <summary>
///     Query the database for contract consultant record in exportable form.
/// </summary>
public sealed record GetContractConsultantsQuery(Entity Entity) : IQuery<List<ConsultantRepresentation>>;

/// <summary>
///     Processes the <see cref="GetContractConsultantsQuery" /> query.
/// </summary>
public sealed class GetContractConsultantsQueryHandler
    : IQueryHandler<GetContractConsultantsQuery, List<ConsultantRepresentation>>
{
    private readonly DataContext _dataContext;

    public GetContractConsultantsQueryHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public Task<List<ConsultantRepresentation>> Handle(GetContractConsultantsQuery request,
        CancellationToken cancellationToken)
    {
        // Get contract consultants from the database.
        var contractConsultants = from contractConsultant in _dataContext.ContractConsultants
            where contractConsultant.ContractId == request.Entity.Id
            join consultant in _dataContext.Consultants
                on contractConsultant.ConsultantId equals consultant.Id
            select consultant;

        // Mapping clients to the model to be exported.
        return Task.FromResult(contractConsultants.Adapt<List<ConsultantRepresentation>>());
    }
}