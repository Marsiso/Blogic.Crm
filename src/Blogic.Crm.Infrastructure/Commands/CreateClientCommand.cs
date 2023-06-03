using Blogic.Crm.Domain.Data.Entities;
using Blogic.Crm.Infrastructure.Authentication;
using Blogic.Crm.Infrastructure.Persistence;
using Mapster;
using MediatR;

namespace Blogic.Crm.Infrastructure.Commands;

public record CreateClientCommand(string Email, string Password, string GivenName, string FamilyName, string Phone,
                                  DateTime DateBorn, string BirthNumber) : ICommand<Unit>;

public sealed class CreateClientCommandHandler : ICommandHandler<CreateClientCommand, Unit>
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

	public Task<Unit> Handle(CreateClientCommand request, CancellationToken cancellationToken)
	{
		Client clientEntity = request.Adapt<Client>();
			
		clientEntity.NormalizedEmail = _emailLookupNormalizer.Normalize(request.Email)!;
		clientEntity.Phone = _phoneLookupNormalizer.Normalize(request.Phone)!;
		clientEntity.SecurityStamp = _securityStampProvider.GenerateSecurityStamp();
		clientEntity.PasswordHash = _passwordHasher.HashPassword(request.Password);
		
		_dataContext.Clients.Add(clientEntity);
		_dataContext.SaveChanges();

		return Task.FromResult(Unit.Value);
	}
}