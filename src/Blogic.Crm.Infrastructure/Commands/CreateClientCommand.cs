using System.Diagnostics;
using Blogic.Crm.Domain.Data.Entities;
using Blogic.Crm.Infrastructure.Authentication;
using Blogic.Crm.Infrastructure.Data;
using Mapster;
using MediatR;
using static Blogic.Crm.Infrastructure.TypeExtensions.StringExtensions;

namespace Blogic.Crm.Infrastructure.Commands;

public record CreateClientCommand(string Email, string Password, string GivenName, string FamilyName, string Phone,
                                  DateTime DateBorn, string BirthNumber) : IRequest<Entity>;

public sealed class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, Entity>
{
	private readonly DataContext _dataContext;
	private readonly IPasswordHasher _passwordHasher;
	private readonly ISecurityStampProvider _securityStampProvider;
	private readonly IEmailLookupNormalizer _emailLookupNormalizer;
	private readonly IPhoneLookupNormalizer _phoneLookupNormalizer;

	public CreateClientCommandHandler(DataContext dataContext, IPasswordHasher passwordHasher,
	                                  ISecurityStampProvider securityStampProvider,
	                                  IEmailLookupNormalizer emailLookupNormalizer,
	                                  IPhoneLookupNormalizer phoneLookupNormalizer)
	{
		_dataContext = dataContext;
		_passwordHasher = passwordHasher;
		_securityStampProvider = securityStampProvider;
		_emailLookupNormalizer = emailLookupNormalizer;
		_phoneLookupNormalizer = phoneLookupNormalizer;
	}

	public async Task<Entity> Handle(CreateClientCommand request, CancellationToken cancellationToken)
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
		
		await _dataContext.Clients.AddAsync(clientEntity, cancellationToken);
		await _dataContext.SaveChangesAsync(cancellationToken);

		return clientEntity;
	}
}