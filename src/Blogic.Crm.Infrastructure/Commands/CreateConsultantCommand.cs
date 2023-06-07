namespace Blogic.Crm.Infrastructure.Commands;

/// <summary>
///     Create a consultant in the database.
/// </summary>
public record CreateConsultantCommand(string Email, string Password, string GivenName, string FamilyName, string Phone,
    DateTime DateBorn, string BirthNumber) : ICommand<Entity>;

/// <summary>
///     Processes the <see cref="CreateConsultantCommand" /> command.
/// </summary>
public sealed class CreateConsultantCommandHandler : ICommandHandler<CreateConsultantCommand, Entity>
{
    private readonly DataContext _dataContext;
    private readonly IEmailLookupNormalizer _emailLookupNormalizer;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IPhoneLookupNormalizer _phoneLookupNormalizer;
    private readonly ISecurityStampProvider _securityStampProvider;

    public CreateConsultantCommandHandler(DataContext dataContext, IPasswordHasher passwordHasher,
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

    public Task<Entity> Handle(CreateConsultantCommand request, CancellationToken cancellationToken)
    {
        // Map input data to the consultant model.
        var consultant = request.Adapt<Consultant>();

        // Normalize and complete the selected properties.
        consultant.NormalizedEmail = _emailLookupNormalizer.Normalize(request.Email)!;
        consultant.Phone = _phoneLookupNormalizer.Normalize(request.Phone)!;
        consultant.SecurityStamp = _securityStampProvider.GenerateSecurityStamp();
        consultant.PasswordHash = _passwordHasher.HashPassword(request.Password);

        // Create a consultant in the database.
        _dataContext.Consultants.Add(consultant);
        _dataContext.SaveChanges();

        // Return the identifier of the created consultant.
        return Task.FromResult((Entity)consultant);
    }
}