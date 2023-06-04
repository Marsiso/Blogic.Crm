using Blogic.Crm.Domain.Data.Entities;
using Blogic.Crm.Infrastructure.Authentication;
using Blogic.Crm.Infrastructure.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Blogic.Crm.Infrastructure.TypeExtensions.StringExtensions;

namespace Blogic.Crm.Infrastructure.Commands;

/// <summary>
/// Updates the persisted client.
/// </summary>
/// <param name="Id">Client's unique identifier used to retrieve the persisted client entity.</param>
/// <param name="Email">Client's email address to be updated.</param>
/// <param name="Password">Client's password to be updated.</param>
/// <param name="GivenName">Client's given name to be updated.</param>
/// <param name="FamilyName">Client's family name to be updated.</param>
/// <param name="Phone">Client's phone number to be updated.</param>
/// <param name="DateBorn">Client's date born to be updated.</param>
/// <param name="BirthNumber">Client's birth number to be updated.</param>
public sealed record UpdateClientCommand(long Id, string Email, string Password, string GivenName, string FamilyName,
                                         string Phone, DateTime DateBorn, string BirthNumber) : ICommand<Unit>;

/// <summary>
/// Handles the <see cref="UpdateClientCommand"/> command.
/// </summary>
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
		// Retrieve the persisted client using the provided ID.
		Client? clientEntity= await _dataContext.Clients
		                                         .AsTracking()
		                                         .SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

		// When the client is not persisted then take no action and return.
		if (clientEntity == null)
		{
			return Unit.Value;
		}

		// Duplicate client entity for the original-new comparison.
		Client clientPrevious = (Client)clientEntity.Clone();
		
		// Map the data to the original client entity.
		request.Adapt(clientEntity);

		// Track if new security stamp should be assigned for security purposes.
		var generateSecurityStamp = false;
			
		// Normalize and set email address.
		var normalizedEmail =  _emailLookupNormalizer.Normalize(request.Email)!;
		if (EqualsNot(normalizedEmail,clientPrevious.NormalizedEmail, StringComparison.Ordinal))
		{
			generateSecurityStamp = true;
			clientEntity.NormalizedEmail = normalizedEmail;
		}
		
		// Normalize and set phone number. 
		var normalizedPhone = _phoneLookupNormalizer.Normalize(request.Phone)!;
		if (EqualsNot(normalizedPhone,clientPrevious.Phone, StringComparison.Ordinal))
		{
			generateSecurityStamp = true;
			clientEntity.Phone = normalizedPhone;
		}

		// Update password when password and its supposed password hash doesn't match.
		if (_passwordHasher.VerifyPassword(request.Password, clientPrevious.PasswordHash) == PasswordVerificationResult.Fail)
		{
			generateSecurityStamp = true;
			clientEntity.PasswordHash = _passwordHasher.HashPassword(request.Password);
		}

		// When client's credential changed than generate new security stamp.
		if (generateSecurityStamp)
		{
			clientEntity.SecurityStamp = _securityStampProvider.GenerateSecurityStamp();
		}
		
		// Persist changes to the client entity.
		_dataContext.Clients.Update(clientEntity);
		await _dataContext.SaveChangesAsync(cancellationToken);

		// Finish.
		return Unit.Value;
	}
}