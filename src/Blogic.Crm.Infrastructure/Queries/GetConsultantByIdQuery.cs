using Microsoft.EntityFrameworkCore;

namespace Blogic.Crm.Infrastructure.Queries;

public record GetConsultantByIdQuery(long Id, bool TrackChanges) : IQuery<Consultant?>;

public sealed class GetConsultantByIdQueryHandler : IQueryHandler<GetConsultantByIdQuery, Consultant?>
{
	private readonly DataContext _dataContext;

	public GetConsultantByIdQueryHandler(DataContext dataContext)
	{
		_dataContext = dataContext;
	}

	public Task<Consultant?> Handle(GetConsultantByIdQuery request, CancellationToken cancellationToken)
	{
		// Retrieve the client using the client ID.
		return request.TrackChanges
			? _dataContext.Consultants
			              .AsTracking()
			              .SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
			: _dataContext.Consultants
			              .AsNoTracking()
			              .SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
	}
}