using Blogic.Crm.Domain.Data.Dtos;
using Blogic.Crm.Domain.Exceptions;
using Blogic.Crm.Infrastructure.Authentication;
using Blogic.Crm.Infrastructure.Data;
using MediatR;
using static Blogic.Crm.Infrastructure.Extensions.StringExtensions;

namespace Blogic.Crm.Infrastructure.Commands;

public record UpdateClientCommand(long Id, string? Email, string? Password, string? GivenName, string? FamilyName,
                                  string? Phone, DateTime? DateBorn, string? BirthNumber) : IRequest<UpdateClientResult>;

public sealed class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, UpdateClientResult>
{
	private readonly ClientRepository _clientRepository;
	private readonly IEmailLookupNormalizer _emailLookupNormalizer;
	private readonly IPasswordHasher _passwordHasher;
	private readonly IPhoneLookupNormalizer _phoneLookupNormalizer;
	private readonly ISecurityStampProvider _securityStampProvider;

	public UpdateClientCommandHandler(ClientRepository clientRepository, IPasswordHasher passwordHasher,
	                                  IEmailLookupNormalizer emailLookupNormalizer,
	                                  IPhoneLookupNormalizer phoneLookupNormalizer,
	                                  ISecurityStampProvider securityStampProvider)
	{
		_clientRepository = clientRepository;
		_passwordHasher = passwordHasher;
		_emailLookupNormalizer = emailLookupNormalizer;
		_phoneLookupNormalizer = phoneLookupNormalizer;
		_securityStampProvider = securityStampProvider;
	}

	public async Task<UpdateClientResult> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
	{
		var clientEntity = await _clientRepository.FindById(request.Id, true);
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

			_clientRepository.Update(clientEntity);
			await _clientRepository.SaveChangesAsync(cancellationToken);

			return new UpdateClientResult(clientEntity);
		}

		throw new EntityNotFoundException("Client does not exist.", request.Id);
	}
}