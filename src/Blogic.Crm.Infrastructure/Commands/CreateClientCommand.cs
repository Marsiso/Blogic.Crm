namespace Blogic.Crm.Infrastructure.Commands;

/// <summary>
///     Command for creating a client in the database.
/// </summary>
public sealed record CreateClientCommand(string Email, string Password, string GivenName, string FamilyName,
    string Phone,
    DateTime DateBorn, string BirthNumber) : ICommand<Entity>;

/// <summary>
///     Processes the <see cref="CreateClientCommand" /> command.
/// </summary>
public sealed class CreateClientCommandHandler : ICommandHandler<CreateClientCommand, Entity>
{
    private readonly DataContext _dataContext;
    private readonly IEmailLookupNormalizer _emailLookupNormalizer;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IPhoneLookupNormalizer _phoneLookupNormalizer;
    private readonly ISecurityStampProvider _securityStampProvider;

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

    public Task<Entity> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        // Map input data to the client model.
        var client = request.Adapt<Client>();
        client.BirthNumber = FormatBirthNumber(client.BirthNumber)!;

        // Normalize and complete the selected properties.
        client.NormalizedEmail = _emailLookupNormalizer.Normalize(request.Email)!;
        client.Phone = _phoneLookupNormalizer.Normalize(request.Phone)!;
        client.SecurityStamp = _securityStampProvider.GenerateSecurityStamp();
        client.PasswordHash = _passwordHasher.HashPassword(request.Password);

        // Create a client in the database.
        _dataContext.Clients.Add(client);
        _dataContext.SaveChanges();

        // Return the identifier of the created client.
        return Task.FromResult((Entity)client);
    }
}