using Blogic.Crm.Infrastructure.Authentication;
using Blogic.Crm.Infrastructure.Persistence;
using Blogic.Crm.Infrastructure.Validators;
using Faker;
using FluentValidation;

namespace Blogic.Crm.Web.Persistence;

/// <summary>
///     Data seed provider for the database entities.
/// </summary>
public sealed class DatabaseSeeder : IDisposable
{
	public const int GeneratedClientsCount = 1000;

	public const int GeneratedConsultantsCount = 200;

	public const int GeneratedContractsCount = 5000;

	public const int GeneratedContractConsultantsUpperBound = 10;

	private readonly DataContext _dataContext;

	private readonly IEmailLookupNormalizer _emailNormalizer;

	private readonly IPasswordHasher _passwordHasher;

	private readonly IPhoneLookupNormalizer _phoneNormalizer;

	private readonly ISecurityStampProvider _securityStampProvider;

	private bool _disposed;

	public DatabaseSeeder(DataContext dataContext, IPasswordHasher passwordHasher,
	                      IEmailLookupNormalizer emailNormalizer, IPhoneLookupNormalizer phoneNormalizer,
	                      ISecurityStampProvider securityStampProvider)
	{
		_dataContext = dataContext;
		_passwordHasher = passwordHasher;
		_emailNormalizer = emailNormalizer;
		_phoneNormalizer = phoneNormalizer;
		_securityStampProvider = securityStampProvider;
	}

	public void Dispose()
	{
		Dispose(true);
	}

	/// <summary>
	///     Creates the database with a randomly generated data seed. When database exists then it will be recreated to
	///     provide freshly seeded data.
	/// </summary>
	public void Seed()
	{
		MigrateDatabase();
		var clients = SeedClients();
		var consultants = SeedConsultants();
		var contracts = SeedContracts(clients, consultants);
		SeedContractConsultants(consultants, contracts);
	}

	private Client GenerateClient(string passwordHash)
	{
		var email = Internet.Email();
		var phone = Phone.Number();
		var dateBorn = Identification.DateOfBirth();

		return new Client
		{
			GivenName = Name.First(),
			FamilyName = Name.Last(),
			PasswordHash = passwordHash,
			Email = email,
			NormalizedEmail = _emailNormalizer.Normalize(email)!,
			IsEmailConfirmed = true,
			Phone = _phoneNormalizer.Normalize(phone)!,
			IsPhoneConfirmed = true,
			DateBorn = Identification.DateOfBirth(),
			BirthNumber = $"{dateBorn:yyMMdd}{RandomNumber.Next(0, 9999):D4}",
			SecurityStamp = _securityStampProvider.GenerateSecurityStamp()
		};
	}

	private Consultant GenerateConsultant(string passwordHash)
	{
		var email = Internet.Email();
		var phone = Phone.Number();
		var dateBorn = Identification.DateOfBirth();

		return new Consultant
		{
			GivenName = Name.First(),
			FamilyName = Name.Last(),
			PasswordHash = passwordHash,
			Email = email,
			NormalizedEmail = _emailNormalizer.Normalize(email)!,
			IsEmailConfirmed = true,
			Phone = _phoneNormalizer.Normalize(phone)!,
			IsPhoneConfirmed = true,
			DateBorn = Identification.DateOfBirth(),
			BirthNumber = $"{dateBorn:yyMMdd}{RandomNumber.Next(0, 9999):D4}",
			SecurityStamp = _securityStampProvider.GenerateSecurityStamp()
		};
	}

	private Contract GenerateContract()
	{
		const int yearConcludedLowerBound = 1970;
		const int monthConcludedLowerBound = 1;
		const int dayConcludedLowerBound = 1;
		const int contractLifetimeYearUpperBound = 50;
		var currentYear = DateTime.UtcNow.Year;

		// Minimal possible date concluded.
		var dateConcluded = new DateTime(yearConcludedLowerBound,
		                                 monthConcludedLowerBound,
		                                 dayConcludedLowerBound);

		// Add years, month and days to the date concluded for uniqueness.
		dateConcluded = dateConcluded.AddYears(RandomNumber.Next(0, currentYear - yearConcludedLowerBound));
		dateConcluded = dateConcluded.AddMonths(RandomNumber.Next(0, 12));
		dateConcluded = dateConcluded.AddDays(RandomNumber.Next(0, 31));

		// Date contract is in effect should be same month or months later as date concluded.
		var dateValid = dateConcluded.AddMonths(RandomNumber.Next(0, 12));

		// Date expired should be same as date valid or later.
		var dateExpired =
			dateValid.AddYears(RandomNumber.Next(dateValid.Year, currentYear + contractLifetimeYearUpperBound));
		dateExpired = dateExpired.AddMonths(RandomNumber.Next(0, 12));
		dateExpired = dateExpired.AddDays(RandomNumber.Next(0, 31));

		// Create and return new contract.
		return new Contract
		{
			RegistrationNumber = Guid.NewGuid().ToString("D"),
			Institution = Company.Name(),
			DateConcluded = dateConcluded,
			DateValid = dateValid,
			DateExpired = dateExpired
		};
	}

	private void MigrateDatabase()
	{
		// Ensure database is freshly created.
		_dataContext.Database.EnsureDeleted();
		_dataContext.Database.EnsureCreated();
	}

	private List<Client> SeedClients()
	{
		IValidator<Client> validator = new ClientValidator();
		var passwordHash = _passwordHasher.HashPassword("$Password123$");

		// Generate predefined number of clients.
		List<Client> clients = new();
		for (var count = 0; count < GeneratedClientsCount; count++)
		{
			clients.Add(GenerateClient(passwordHash));
		}

		// Filter generated clients.
		clients = clients.Where(c => validator.Validate(c).IsValid)
		                 .DistinctBy(c => new { c.NormalizedEmail, c.Phone, c.BirthNumber })
		                 .ToList();

		// Add generated consultant to the collection.
		_dataContext.AddRange(clients);

		// Save changes to database entities.
		_dataContext.SaveChanges();
		return clients;
	}

	private List<Consultant> SeedConsultants()
	{
		var passwordHash = _passwordHasher.HashPassword("$Password123$");
		IValidator<Consultant> validator = new ConsultantValidator();

		// Generate predefined number of consultants.
		List<Consultant> consultants = new();
		for (var count = 0; count < GeneratedConsultantsCount; count++)
		{
			consultants.Add(GenerateConsultant(passwordHash));
		}

		// Filter generated consultants.
		consultants = consultants.Where(c => validator.Validate(c).IsValid)
		                         .DistinctBy(c => new { c.NormalizedEmail, c.Phone, c.BirthNumber })
		                         .ToList();

		// Add generated consultant to the collection.
		_dataContext.AddRange(consultants);

		// Save changes to database entities.
		_dataContext.SaveChanges();
		return consultants;
	}

	private List<Contract> SeedContracts(List<Client> clients,
	                                     List<Consultant> consultants)
	{
		IValidator<Contract> validator = new ContractValidator();

		// Generate predefined number of contracts;
		List<Contract> contracts = new();
		for (var count = 0; count < GeneratedContractsCount; count++)
		{
			contracts.Add(GenerateContract());
		}

		// Filter generated contracts.
		contracts = contracts.Where(c => validator.Validate(c).IsValid)
		                     .DistinctBy(c => new { c.RegistrationNumber })
		                     .ToList();

		// Assign randomly selected contract owner and manager.
		var clientsCount = clients.Count - 1;
		var consultantsCount = consultants.Count - 1;
		contracts.ForEach(c =>
		{
			c.ClientId = clients[RandomNumber.Next(0, clientsCount)].Id;
			c.ManagerId = consultants[RandomNumber.Next(0, consultantsCount)].Id;
		});

		// Add generated consultant to the collection.
		_dataContext.AddRange(contracts);

		// Save changes to database entities.
		_dataContext.SaveChanges();
		return contracts;
	}

	private void SeedContractConsultants(List<Consultant> consultants,
	                                     List<Contract> contracts)
	{
		// For each contract in the collection assign random contract manager and contract consultants.
		foreach (var contract in contracts)
		{
			// Seed contract manager.
			var contractConsultant = new ContractConsultant
			{
				ContractId = contract.Id,
				ConsultantId = contract.ManagerId!.Value
			};

			_dataContext.Add(contractConsultant);

			// Seed random number of contract consultants.
			var randomConsultants = consultants
			                        .Where(c => c.Id != contract.ManagerId)
			                        .OrderBy(_ => RandomNumber.Next())
			                        .Take(RandomNumber.Next(0, GeneratedContractConsultantsUpperBound))
			                        .Select(c => new ContractConsultant
			                        {
				                        ContractId = contract.Id,
				                        ConsultantId = c.Id
			                        });

			// Add generated contract to the collection.
			_dataContext.AddRange(randomConsultants);
		}

		// Save changes to database entities.
		_dataContext.SaveChanges();
	}

	private void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			if (disposing)
			{
				_dataContext.Dispose();
			}
		}

		_disposed = true;
	}
}