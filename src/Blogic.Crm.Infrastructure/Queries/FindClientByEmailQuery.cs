using Blogic.Crm.Domain.Data.Entities;
using Blogic.Crm.Infrastructure.Authentication;
using Blogic.Crm.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blogic.Crm.Infrastructure.Queries;

public record FindClientByEmailQuery(string Email, bool TrackChanges) : IRequest<Client?>;

public sealed class FindClientByEmailQueryHandler : IRequestHandler<FindClientByEmailQuery, Client?>
{
	private readonly DataContext _dataContext;
	private readonly IEmailLookupNormalizer _emailLookupNormalizer;

	public FindClientByEmailQueryHandler(DataContext dataContext, IEmailLookupNormalizer emailLookupNormalizer)
	{
		_dataContext = dataContext;
		_emailLookupNormalizer = emailLookupNormalizer;
	}

	public Task<Client?> Handle(FindClientByEmailQuery request, CancellationToken cancellationToken)
	{
		var normalizedEmail = _emailLookupNormalizer.Normalize(request.Email)!;
		return request.TrackChanges
			? _dataContext.Clients.AsTracking()
			              .Where(c => c.NormalizedEmail == normalizedEmail)
			              .SingleOrDefaultAsync(cancellationToken)
			: _dataContext.Clients.AsNoTracking()
			              .Where(c => c.NormalizedEmail == normalizedEmail)
			              .SingleOrDefaultAsync(cancellationToken);
	}
}