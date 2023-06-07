namespace Blogic.Crm.Infrastructure.Commands;

/// <summary>
///     The command to update the consultant in the database.
/// </summary>
public sealed record UpdateConsultantCommand(Entity Entity, string Email, string Password, string GivenName,
    string FamilyName, string Phone, DateTime DateBorn,
    string BirthNumber) : ICommand<Unit>;

/// <summary>
///     Processes the <see cref="UpdateConsultantCommand" /> command.
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
        // Get the consultant from the database.
        var consultant = await _dataContext.Consultants
            .AsTracking()
            .SingleOrDefaultAsync(c => c.Id == request.Entity.Id, cancellationToken);

        // If the consultant is not found, do not perform any action and return,
        // otherwise remove the consultant from the database and save the changes.
        if (consultant == null) return Unit.Value;

        // Create a copy of the consultant for comparison purposes.
        var consultantPrevious = (Consultant)consultant.Clone();

        // Map input data to the consultant model.
        request.Adapt(consultant);

        // Keep track of changes to client login credentials.
        var generateSecurityStamp = false;

        // Normalize and complete the email address.
        var normalizedEmail = _emailLookupNormalizer.Normalize(request.Email)!;
        if (EqualsNot(normalizedEmail, consultantPrevious.NormalizedEmail, StringComparison.Ordinal))
        {
            generateSecurityStamp = true;
            consultant.NormalizedEmail = normalizedEmail;
        }

        // Normalize and complete the telephone number.
        var normalizedPhone = _phoneLookupNormalizer.Normalize(request.Phone)!;
        if (EqualsNot(normalizedPhone, consultantPrevious.Phone, StringComparison.Ordinal))
        {
            generateSecurityStamp = true;
            consultant.Phone = normalizedPhone;
        }

        // When consultant change the user password,
        // replace the original encrypted password with a replacement.
        if (_passwordHasher.VerifyPassword(request.Password, consultantPrevious.PasswordHash) ==
            PasswordVerificationResult.Fail)
        {
            generateSecurityStamp = true;
            consultant.PasswordHash = _passwordHasher.HashPassword(request.Password);
        }

        // When consultant's credentials change then set new security stamp.
        if (generateSecurityStamp) consultant.SecurityStamp = _securityStampProvider.GenerateSecurityStamp();

        // Update a consultant in the database.
        _dataContext.Consultants.Update(consultant);
        await _dataContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}