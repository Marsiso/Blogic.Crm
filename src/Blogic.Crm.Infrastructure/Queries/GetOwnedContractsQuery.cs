using Blogic.Crm.Domain.Data.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Blogic.Crm.Infrastructure.Queries;

public sealed record GetOwnedContractsQuery(Entity Entity) : IQuery<List<ContractRepresentation>>;

public sealed class GetOwnedContractsQueryHandler : IQueryHandler<GetOwnedContractsQuery, List<ContractRepresentation>>
{
	public GetOwnedContractsQueryHandler(DataContext dataContext)
	{
		_dataContext = dataContext;
	}

	private readonly DataContext _dataContext;

	public async Task<List<ContractRepresentation>> Handle(GetOwnedContractsQuery request, CancellationToken cancellationToken)
	{
		var contractEntities = await _dataContext.Contracts
		                                         .Where(c => c.ClientId == request.Entity.Id)
		                                         .ToListAsync(cancellationToken: cancellationToken);

		return contractEntities.Adapt<List<ContractRepresentation>>();
	}
}