using Microsoft.EntityFrameworkCore;

namespace Blogic.Crm.Infrastructure.Queries;

public sealed record GetContractByIdQuery(long Id, bool TrackChanges) : IQuery<Contract?>;

public sealed class GetContractByIdQueryHandler : IQueryHandler<GetContractByIdQuery, Contract?>
{
	public GetContractByIdQueryHandler(DataContext dataContext)
	{
		_dataContext = dataContext;
	}

	private readonly DataContext _dataContext;

	public Task<Contract?> Handle(GetContractByIdQuery request, CancellationToken cancellationToken)
	{
		return request.TrackChanges
			? _dataContext.Contracts.AsTracking()
			              .SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
			: _dataContext.Contracts.AsTracking()
			              .SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
	}
}