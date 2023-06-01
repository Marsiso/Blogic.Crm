using Blogic.Crm.Domain.Data.Entities;
using Blogic.Crm.Infrastructure.Authentication;
using Blogic.Crm.Infrastructure.Data;
using MediatR;

namespace Blogic.Crm.Infrastructure.Queries;

public record GetClientByEmailQuery(string Email, bool TrackChanges) : IRequest<Client?>;

public sealed class FindClientByEmailQueryHandler : IRequestHandler<GetClientByEmailQuery, Client?>
{
	private readonly ClientRepository _clientRepository;
	private readonly IEmailLookupNormalizer _emailLookupNormalizer;

	public FindClientByEmailQueryHandler(ClientRepository clientRepository, IEmailLookupNormalizer emailLookupNormalizer)
	{
		_clientRepository = clientRepository;
		_emailLookupNormalizer = emailLookupNormalizer;
	}

	public Task<Client?> Handle(GetClientByEmailQuery request, CancellationToken cancellationToken)
	{
		var normalizedEmail = _emailLookupNormalizer.Normalize(request.Email)!;
		return _clientRepository.FindByEmail(normalizedEmail, request.TrackChanges, cancellationToken);
	}
}