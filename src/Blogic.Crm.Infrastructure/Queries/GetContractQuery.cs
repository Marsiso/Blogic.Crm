using Microsoft.EntityFrameworkCore;

namespace Blogic.Crm.Infrastructure.Queries;

public sealed record GetContractQuery(Entity Entity, bool TrackChanges) : IQuery<Contract?>;

public sealed class GetContractQueryHandler : IQueryHandler<GetContractQuery, Contract?>
{
	public GetContractQueryHandler(DataContext dataContext)
	{
		_dataContext = dataContext;
	}

	private readonly DataContext _dataContext;

	public Task<Contract?> Handle(GetContractQuery request, CancellationToken cancellationToken)
	{
		return request.TrackChanges
			? _dataContext.Contracts.AsTracking()
			              .SingleOrDefaultAsync(c => c.Id == request.Entity.Id, cancellationToken)
			: _dataContext.Contracts.AsTracking()
			              .SingleOrDefaultAsync(c => c.Id == request.Entity.Id, cancellationToken);
	}
}