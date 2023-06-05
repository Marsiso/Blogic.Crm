using Microsoft.EntityFrameworkCore;

namespace Blogic.Crm.Infrastructure.Queries;

/// <summary>
///     Retrieves the persisted client using the provided ID.
/// </summary>
/// <param name="Id"></param>
/// <param name="TrackChanges"></param>
public sealed record GetClientByIdQuery(long Id, bool TrackChanges) : IQuery<Client?>;

/// <summary>
///     <see cref="GetClientByIdQueryHandler" /> query handler.
/// </summary>
public sealed class GetClientByIdQueryHandler : IQueryHandler<GetClientByIdQuery, Client?>
{
	private readonly DataContext _dataContext;

	public GetClientByIdQueryHandler(DataContext dataContext)
	{
		_dataContext = dataContext;
	}

	public Task<Client?> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
	{
		// Retrieve the client using the client ID.
		return request.TrackChanges
			? _dataContext.Clients
			              .AsTracking()
			              .SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
			: _dataContext.Clients
			              .AsNoTracking()
			              .SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
	}
}