using Blogic.Crm.Domain.Data.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Blogic.Crm.Infrastructure.Queries;

public sealed record GetManagedContractsQuery(Entity Entity) : IQuery<List<ContractRepresentation>>;

public sealed class GetManagedContractsQueryHandler : IQueryHandler<GetManagedContractsQuery, List<ContractRepresentation>>
{
	public GetManagedContractsQueryHandler(DataContext dataContext)
	{
		_dataContext = dataContext;
	}

	private readonly DataContext _dataContext;

	public async Task<List<ContractRepresentation>> Handle(GetManagedContractsQuery request, CancellationToken cancellationToken)
	{
		var contractEntities = await _dataContext.Contracts
		                                         .Where(c => c.ManagerId == request.Entity.Id)
		                                         .ToListAsync(cancellationToken: cancellationToken);

		return contractEntities.Adapt<List<ContractRepresentation>>();
	}
}