using Blogic.Crm.Domain.Data.Entities;
using Blogic.Crm.Infrastructure.Authentication;
using Blogic.Crm.Infrastructure.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Blogic.Crm.Infrastructure.TypeExtensions.StringExtensions;

namespace Blogic.Crm.Infrastructure.Commands;

public sealed record UpdateClientCommand(long Id, string Email, string Password, string GivenName, string FamilyName,
                                         string Phone, DateTime DateBorn, string BirthNumber) : ICommand<Unit>;

public sealed class UpdateClientCommandHandler : ICommandHandler<UpdateClientCommand, Unit>
{
	public UpdateClientCommandHandler(DataContext dataContext, ISecurityStampProvider securityStampProvider,
	                                  IEmailLookupNormalizer emailLookupNormalizer,
	                                  IPhoneLookupNormalizer phoneLookupNormalizer, IPasswordHasher passwordHasher)
	{
		_dataContext = dataContext;
		_securityStampProvider = securityStampProvider;
		_emailLookupNormalizer = emailLookupNormalizer;
		_phoneLookupNormalizer = phoneLookupNormalizer;
		_passwordHasher = passwordHasher;
	}

	private readonly DataContext _dataContext;
	private readonly ISecurityStampProvider _securityStampProvider;
	private readonly IEmailLookupNormalizer _emailLookupNormalizer;
	private readonly IPhoneLookupNormalizer _phoneLookupNormalizer;
	private readonly IPasswordHasher _passwordHasher;

	public async Task<Unit> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
	{
		Client? clientEntity= await _dataContext.Clients
		                                         .AsTracking()
		                                         .SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

		if (clientEntity == null)
		{
			return Unit.Value;
		}

		Client clientPrevious = (Client)clientEntity.Clone();
		request.Adapt(clientEntity);

		var generateSecurityStamp = false;
			
		var normalizedEmail =  _emailLookupNormalizer.Normalize(request.Email)!;
		if (EqualsNot(normalizedEmail,clientPrevious.NormalizedEmail, StringComparison.Ordinal))
		{
			generateSecurityStamp = true;
			clientEntity.NormalizedEmail = normalizedEmail;
		}
			
		var normalizedPhone = _phoneLookupNormalizer.Normalize(request.Phone)!;
		if (EqualsNot(normalizedPhone,clientPrevious.Phone, StringComparison.Ordinal))
		{
			generateSecurityStamp = true;
			clientEntity.Phone = normalizedPhone;
		}

		if (_passwordHasher.VerifyPassword(request.Password, clientPrevious.PasswordHash) == PasswordVerificationResult.Fail)
		{
			generateSecurityStamp = true;
			clientEntity.PasswordHash = _passwordHasher.HashPassword(request.Password);
		}

		if (generateSecurityStamp)
		{
			clientEntity.SecurityStamp = _securityStampProvider.GenerateSecurityStamp();
		}

		_dataContext.Clients.Update(clientEntity);
		await _dataContext.SaveChangesAsync(cancellationToken);

		return Unit.Value;
	}
}