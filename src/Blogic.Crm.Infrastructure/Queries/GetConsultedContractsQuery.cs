using Blogic.Crm.Domain.Data.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Blogic.Crm.Infrastructure.Queries;

public sealed record GetConsultedContractsQuery(Entity Entity) : IQuery<List<ContractRepresentation>>;

public sealed class GetConsultedContractsQueryHandler : IQueryHandler<GetConsultedContractsQuery, List<ContractRepresentation>>
{
	public GetConsultedContractsQueryHandler(DataContext dataContext)
	{
		_dataContext = dataContext;
	}

	private readonly DataContext _dataContext;

	public async Task<List<ContractRepresentation>> Handle(GetConsultedContractsQuery request, CancellationToken cancellationToken)
	{
		var contractEntities = await _dataContext.ContractConsultants
		                                         .Where(c => c.ConsultantId == request.Entity.Id)
		                                         .ToListAsync(cancellationToken: cancellationToken);

		return contractEntities.Adapt<List<ContractRepresentation>>();
	}
}