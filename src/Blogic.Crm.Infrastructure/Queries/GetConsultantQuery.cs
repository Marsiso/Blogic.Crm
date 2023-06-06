using Microsoft.EntityFrameworkCore;

namespace Blogic.Crm.Infrastructure.Queries;

public record GetConsultantQuery(Entity Entity, bool TrackChanges) : IQuery<Consultant?>;

public sealed class GetConsultantQueryHandler : IQueryHandler<GetConsultantQuery, Consultant?>
{
	private readonly DataContext _dataContext;

	public GetConsultantQueryHandler(DataContext dataContext)
	{
		_dataContext = dataContext;
	}

	public Task<Consultant?> Handle(GetConsultantQuery request, CancellationToken cancellationToken)
	{
		// Retrieve the client using the client ID.
		return request.TrackChanges
			? _dataContext.Consultants
			              .AsTracking()
			              .SingleOrDefaultAsync(c => c.Id == request.Entity.Id, cancellationToken)
			: _dataContext.Consultants
			              .AsNoTracking()
			              .SingleOrDefaultAsync(c => c.Id == request.Entity.Id, cancellationToken);
	}
}