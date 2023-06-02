using Blogic.Crm.Domain.Data.Entities;
using Blogic.Crm.Domain.Exceptions;
using Blogic.Crm.Infrastructure.Authentication;
using Blogic.Crm.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Blogic.Crm.Infrastructure.TypeExtensions.StringExtensions;

namespace Blogic.Crm.Infrastructure.Commands;

public record UpdateClientCommand(long Id, string? Email, string? Password, string? GivenName, string? FamilyName,
                                  string? Phone, DateTime? DateBorn, string? BirthNumber) : IRequest<Entity>;

public sealed class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, Entity>
{
	private readonly DataContext _dataContext;
	private readonly IEmailLookupNormalizer _emailLookupNormalizer;
	private readonly IPasswordHasher _passwordHasher;
	private readonly IPhoneLookupNormalizer _phoneLookupNormalizer;
	private readonly ISecurityStampProvider _securityStampProvider;

	public UpdateClientCommandHandler(DataContext dataContext, IPasswordHasher passwordHasher,
	                                  IEmailLookupNormalizer emailLookupNormalizer,
	                                  IPhoneLookupNormalizer phoneLookupNormalizer,
	                                  ISecurityStampProvider securityStampProvider)
	{
		_dataContext = dataContext;
		_passwordHasher = passwordHasher;
		_emailLookupNormalizer = emailLookupNormalizer;
		_phoneLookupNormalizer = phoneLookupNormalizer;
		_securityStampProvider = securityStampProvider;
	}

	public async Task<Entity> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
	{
		var clientEntity = await _dataContext.Clients.SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
		if (clientEntity != null)
		{
			var generateSecurityStamp = false;
			if (IsNotNullOrEmpty(request.Password))
			{
				clientEntity.PasswordHash = _passwordHasher.HashPassword(request.Password);
				generateSecurityStamp = true;
			}

			if (IsNotNullOrEmpty(request.GivenName))
			{
				clientEntity.GivenName = request.GivenName;
			}

			if (IsNotNullOrEmpty(request.FamilyName))
			{
				clientEntity.FamilyName = request.FamilyName;
			}

			if (IsNotNullOrEmpty(request.Email))
			{
				var normalizedEmail = _emailLookupNormalizer.Normalize(request.Email)!;
				clientEntity.Email = normalizedEmail;
				clientEntity.IsEmailConfirmed = false;
				generateSecurityStamp = true;
			}

			if (IsNotNullOrEmpty(request.Phone))
			{
				var normalizedPhone = _phoneLookupNormalizer.Normalize(request.Phone)!;
				clientEntity.Phone = normalizedPhone;
				clientEntity.IsPhoneConfirmed = false;
				generateSecurityStamp = true;
			}

			if (IsNotNullOrEmpty(request.BirthNumber))
			{
				clientEntity.BirthNumber = request.BirthNumber;
			}
			
			if (request.DateBorn.HasValue)
			{
				clientEntity.DateBorn = request.DateBorn.Value;
			}

			if (generateSecurityStamp)
			{
				var securityStamp = _securityStampProvider.GenerateSecurityStamp();
				clientEntity.SecurityStamp = securityStamp;
			}

			_dataContext.Update(clientEntity);
			await _dataContext.SaveChangesAsync(cancellationToken);

			return clientEntity;
		}

		throw new EntityNotFoundException("Client does not exist.", request.Id);
	}
}