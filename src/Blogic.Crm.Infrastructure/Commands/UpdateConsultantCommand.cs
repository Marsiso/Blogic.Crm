using Microsoft.EntityFrameworkCore;
using static Blogic.Crm.Infrastructure.TypeExtensions.StringExtensions;

namespace Blogic.Crm.Infrastructure.Commands;

/// <summary>
///     Updates the persisted consultant.
/// </summary>
/// <param name="Id">Consultant's unique identifier used to retrieve the persisted Consultant entity.</param>
/// <param name="Email">Consultant's email address to be updated.</param>
/// <param name="Password">Consultant's password to be updated.</param>
/// <param name="GivenName">Consultant's given name to be updated.</param>
/// <param name="FamilyName">Consultant's family name to be updated.</param>
/// <param name="Phone">Consultant's phone number to be updated.</param>
/// <param name="DateBorn">Consultant's date born to be updated.</param>
/// <param name="BirthNumber">Consultant's birth number to be updated.</param>
public sealed record UpdateConsultantCommand(long Id, string Email, string Password, string GivenName,
                                             string FamilyName, string Phone, DateTime DateBorn,
                                             string BirthNumber) : ICommand<Unit>;

/// <summary>
///     Handles the <see cref="UpdateConsultantCommand" /> command.
/// </summary>
public sealed class UpdateConsultantCommandHandler : ICommandHandler<UpdateConsultantCommand, Unit>
{
	private readonly DataContext _dataContext;
	private readonly IEmailLookupNormalizer _emailLookupNormalizer;
	private readonly IPasswordHasher _passwordHasher;
	private readonly IPhoneLookupNormalizer _phoneLookupNormalizer;
	private readonly ISecurityStampProvider _securityStampProvider;

	public UpdateConsultantCommandHandler(DataContext dataContext, ISecurityStampProvider securityStampProvider,
	                                      IEmailLookupNormalizer emailLookupNormalizer,
	                                      IPhoneLookupNormalizer phoneLookupNormalizer, IPasswordHasher passwordHasher)
	{
		_dataContext = dataContext;
		_securityStampProvider = securityStampProvider;
		_emailLookupNormalizer = emailLookupNormalizer;
		_phoneLookupNormalizer = phoneLookupNormalizer;
		_passwordHasher = passwordHasher;
	}

	public async Task<Unit> Handle(UpdateConsultantCommand request, CancellationToken cancellationToken)
	{
		// Retrieve the persisted consultant using the provided ID.
		var consultantEntity = await _dataContext.Consultants
		                                         .AsTracking()
		                                         .SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

		// When the consultant is not persisted then take no action and return.
		if (consultantEntity == null)
		{
			return Unit.Value;
		}

		// Duplicate consultant entity for the original-new comparison.
		var consultantOriginal = (Consultant)consultantEntity.Clone();

		// Map the data to the original consultant entity.
		request.Adapt(consultantEntity);

		// Track if new security stamp should be assigned for security purposes.
		var generateSecurityStamp = false;

		// Normalize and set email address.
		var normalizedEmail = _emailLookupNormalizer.Normalize(request.Email)!;
		if (EqualsNot(normalizedEmail, consultantOriginal.NormalizedEmail, StringComparison.Ordinal))
		{
			generateSecurityStamp = true;
			consultantEntity.NormalizedEmail = normalizedEmail;
		}

		// Normalize and set phone number. 
		var normalizedPhone = _phoneLookupNormalizer.Normalize(request.Phone)!;
		if (EqualsNot(normalizedPhone, consultantOriginal.Phone, StringComparison.Ordinal))
		{
			generateSecurityStamp = true;
			consultantEntity.Phone = normalizedPhone;
		}

		// Update password when password and its supposed password hash doesn't match.
		if (_passwordHasher.VerifyPassword(request.Password, consultantOriginal.PasswordHash) ==
		    PasswordVerificationResult.Fail)
		{
			generateSecurityStamp = true;
			consultantEntity.PasswordHash = _passwordHasher.HashPassword(request.Password);
		}

		// When consultant's credential changed than generate new security stamp.
		if (generateSecurityStamp)
		{
			consultantEntity.SecurityStamp = _securityStampProvider.GenerateSecurityStamp();
		}

		// Persist changes to the consultant entity.
		_dataContext.Consultants.Update(consultantEntity);
		await _dataContext.SaveChangesAsync(cancellationToken);

		// Finish.
		return Unit.Value;
	}
}