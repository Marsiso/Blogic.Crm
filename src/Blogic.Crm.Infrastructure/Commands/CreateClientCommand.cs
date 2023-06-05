namespace Blogic.Crm.Infrastructure.Commands;

/// <summary>
///     Persists the client.
/// </summary>
/// <param name="Email">Client's email address.</param>
/// <param name="Password">Client's password used by the <see cref="IPasswordHasher" /> to securely store credentials.</param>
/// <param name="GivenName">Client's given name.</param>
/// <param name="FamilyName">Client's family name.</param>
/// <param name="Phone">Client's phone number.</param>
/// <param name="DateBorn">Client's date of birth.</param>
/// <param name="BirthNumber">Client's birth number.</param>
public record CreateClientCommand(string Email, string Password, string GivenName, string FamilyName, string Phone,
                                  DateTime DateBorn, string BirthNumber) : ICommand<Entity>;

/// <summary>
///     Handlers <see cref="CreateClientCommand" /> command.
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
		// Map the data to the client model.
		var clientEntity = request.Adapt<Client>();

		// Bind additional data such as indexers.
		clientEntity.NormalizedEmail = _emailLookupNormalizer.Normalize(request.Email)!;
		clientEntity.Phone = _phoneLookupNormalizer.Normalize(request.Phone)!;
		clientEntity.SecurityStamp = _securityStampProvider.GenerateSecurityStamp();
		clientEntity.PasswordHash = _passwordHasher.HashPassword(request.Password);

		// Persist the client.
		_dataContext.Clients.Add(clientEntity);
		_dataContext.SaveChanges();

		// Return the client ID.
		return Task.FromResult((Entity)clientEntity);
	}
}