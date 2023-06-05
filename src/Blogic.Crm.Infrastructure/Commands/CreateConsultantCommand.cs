namespace Blogic.Crm.Infrastructure.Commands;

/// <summary>
///     Persists the consultant.
/// </summary>
/// <param name="Email">Consultant's email address.</param>
/// <param name="Password">Consultant's password used by the <see cref="IPasswordHasher" /> to securely store credentials.</param>
/// <param name="GivenName">Consultant's given name.</param>
/// <param name="FamilyName">Consultant's family name.</param>
/// <param name="Phone">Consultant's phone number.</param>
/// <param name="DateBorn">Consultant's date of birth.</param>
/// <param name="BirthNumber">Consultant's birth number.</param>
public record CreateConsultantCommand(string Email, string Password, string GivenName, string FamilyName, string Phone,
                                      DateTime DateBorn, string BirthNumber) : ICommand<Entity>;

/// <summary>
///     Handles <see cref="CreateConsultantCommand" /> command.
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
		// Map the data to the consultant model.
		var consultantEntity = request.Adapt<Consultant>();

		// Bind additional data such as indexers.
		consultantEntity.NormalizedEmail = _emailLookupNormalizer.Normalize(request.Email)!;
		consultantEntity.Phone = _phoneLookupNormalizer.Normalize(request.Phone)!;
		consultantEntity.SecurityStamp = _securityStampProvider.GenerateSecurityStamp();
		consultantEntity.PasswordHash = _passwordHasher.HashPassword(request.Password);

		// Persist the consultant.
		_dataContext.Consultants.Add(consultantEntity);
		_dataContext.SaveChanges();

		// Return the consultant ID.
		return Task.FromResult((Entity)consultantEntity);
	}
}