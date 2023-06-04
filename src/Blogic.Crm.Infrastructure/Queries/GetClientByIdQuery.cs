using Blogic.Crm.Domain.Data.Entities;
using Blogic.Crm.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Blogic.Crm.Infrastructure.Queries;

public sealed record GetClientByIdQuery(long Id, bool TrackChanges) : IQuery<Client?>;

public sealed class GetClientByIdQueryHandler : IQueryHandler<GetClientByIdQuery, Client?>
{
	public GetClientByIdQueryHandler(DataContext dataContext)
	{
		_dataContext = dataContext;
	}

	private readonly DataContext _dataContext;

	public Task<Client?> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
	{
		return request.TrackChanges
			? _dataContext.Clients
			              .AsTracking()
			              .SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
			: _dataContext.Clients
			              .AsNoTracking()
			              .SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
	}
}