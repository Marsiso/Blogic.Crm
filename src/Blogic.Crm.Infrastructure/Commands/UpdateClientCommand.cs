namespace Blogic.Crm.Infrastructure.Commands;

/// <summary>
///     The command to update the client in the database.
/// </summary>
public sealed record UpdateClientCommand(Entity Entity, string Email, string Password, string GivenName, string FamilyName,
    string Phone, DateTime DateBorn, string BirthNumber) : ICommand<Unit>;

/// <summary>
///     Processes the <see cref="UpdateClientCommand" /> command.
/// </summary>
public sealed class UpdateClientCommandHandler : ICommandHandler<UpdateClientCommand, Unit>
{
    private readonly DataContext _dataContext;
    private readonly IEmailLookupNormalizer _emailLookupNormalizer;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IPhoneLookupNormalizer _phoneLookupNormalizer;
    private readonly ISecurityStampProvider _securityStampProvider;

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

    public async Task<Unit> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        // Get the client from the database.
        var client = await _dataContext.Clients
            .AsTracking()
            .SingleOrDefaultAsync(c => c.Id == request.Entity.Id, cancellationToken);

        // If the client is not found, do not perform any action and return,
        // otherwise remove the client from the database and save the changes.
        if (client == null) return Unit.Value;

        // Create a copy of the client for comparison purposes.
        var clientPrevious = (Client)client.Clone();

        // Map input data to the client model.
        request.Adapt(client);
        client.BirthNumber = FormatBirthNumber(client.BirthNumber)!;

        // Keep track of changes to client login credentials.
        var generateSecurityStamp = false;

        // Normalize and complete the email address.
        var normalizedEmail = _emailLookupNormalizer.Normalize(request.Email)!;
        if (EqualsNot(normalizedEmail, clientPrevious.NormalizedEmail, StringComparison.Ordinal))
        {
            generateSecurityStamp = true;
            client.NormalizedEmail = normalizedEmail;
        }

        // Normalize and complete the telephone number.
        var normalizedPhone = _phoneLookupNormalizer.Normalize(request.Phone)!;
        if (EqualsNot(normalizedPhone, clientPrevious.Phone, StringComparison.Ordinal))
        {
            generateSecurityStamp = true;
            client.Phone = normalizedPhone;
        }

        // When client change the user password,
        // replace the original encrypted password with a replacement.
        if (_passwordHasher.VerifyPassword(request.Password, clientPrevious.PasswordHash) ==
            PasswordVerificationResult.Fail)
        {
            generateSecurityStamp = true;
            client.PasswordHash = _passwordHasher.HashPassword(request.Password);
        }

        // When client's credentials change then set new security stamp.
        if (generateSecurityStamp) client.SecurityStamp = _securityStampProvider.GenerateSecurityStamp();

        // Update a client in the database.
        _dataContext.Clients.Update(client);
        await _dataContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}