using System.Diagnostics;
using Blogic.Crm.Domain.Data.Dtos;
using Blogic.Crm.Domain.Data.Entities;
using Blogic.Crm.Infrastructure.Authentication;
using Blogic.Crm.Infrastructure.Data;
using Mapster;
using MediatR;
using static Blogic.Crm.Infrastructure.Extensions.StringExtensions;

namespace Blogic.Crm.Infrastructure.Commands;

public record CreateClientCommand(string Email, string Password, string GivenName, string FamilyName, string Phone,
                                  DateTime DateBorn, string BirthNumber) : IRequest<CreateClientResult>;

public sealed class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, CreateClientResult>
{
	private readonly ClientRepository _clientRepository;
	private readonly IPasswordHasher _passwordHasher;
	private readonly ISecurityStampProvider _securityStampProvider;
	private readonly IEmailLookupNormalizer _emailLookupNormalizer;
	private readonly IPhoneLookupNormalizer _phoneLookupNormalizer;

	public CreateClientCommandHandler(ClientRepository clientRepository, IPasswordHasher passwordHasher,
	                                  ISecurityStampProvider securityStampProvider,
	                                  IEmailLookupNormalizer emailLookupNormalizer,
	                                  IPhoneLookupNormalizer phoneLookupNormalizer)
	{
		_clientRepository = clientRepository;
		_passwordHasher = passwordHasher;
		_securityStampProvider = securityStampProvider;
		_emailLookupNormalizer = emailLookupNormalizer;
		_phoneLookupNormalizer = phoneLookupNormalizer;
	}

	public async Task<CreateClientResult> Handle(CreateClientCommand request, CancellationToken cancellationToken)
	{
		Client clientEntity = request.Adapt<Client>();
			
		var normalizedEmail = _emailLookupNormalizer.Normalize(request.Email)!;
		Debug.Assert(IsNotNullOrEmpty(normalizedEmail));
		clientEntity.NormalizedEmail = normalizedEmail;
			
		var normalizedPhone = _phoneLookupNormalizer.Normalize(request.Phone)!;
		Debug.Assert(IsNotNullOrEmpty(normalizedPhone));
		clientEntity.Phone = _phoneLookupNormalizer.Normalize(clientEntity.Phone)!;
			
		var securityStamp = _securityStampProvider.GenerateSecurityStamp();
		Debug.Assert(IsNotNullOrEmpty(securityStamp));
		clientEntity.SecurityStamp = securityStamp;
				
		var passwordHash = _passwordHasher.HashPassword(request.Password);
		Debug.Assert(IsNotNullOrEmpty(passwordHash));
		clientEntity.PasswordHash = passwordHash;
		
		await _clientRepository.CreateAsync(clientEntity, cancellationToken);
		await _clientRepository.SaveChangesAsync(cancellationToken);

		return new CreateClientResult(clientEntity);
	}
}