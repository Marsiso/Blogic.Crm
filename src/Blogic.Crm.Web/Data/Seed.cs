using Blogic.Crm.Domain.Data.Entities;
using Blogic.Crm.Infrastructure.Commands;
using Blogic.Crm.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blogic.Crm.Web.Data;

public sealed class Seed : IHostedService
{
	public Seed(IServiceScopeFactory serviceScopeFactory)
	{
		_serviceScopeFactory = serviceScopeFactory;
	}

	private readonly IServiceScopeFactory _serviceScopeFactory;
	public const int GeneratedClientsNumber = 100;

	public async Task StartAsync(CancellationToken cancellationToken)
	{
		// Get required services
		await using var serviceScope = _serviceScopeFactory.CreateAsyncScope();
		
		await using var dataContext = serviceScope.ServiceProvider.GetService<DataContext>()
		                              ?? throw new InvalidOperationException();

		// Ensure database is created
		await MigrateDatabase(cancellationToken);
		
		// Seed clients
		SeedClients(cancellationToken);
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}

	public async Task MigrateDatabase(CancellationToken cancellationToken)
	{
		// Get required services
		await using var serviceScope = _serviceScopeFactory.CreateAsyncScope();
		
		await using var dataContext = serviceScope.ServiceProvider.GetService<DataContext>()
		                              ?? throw new InvalidOperationException();
		
		// Ensure database is created
		var databaseExists = await dataContext.Database.EnsureCreatedAsync(cancellationToken);
		if (!databaseExists)
		{
			await dataContext.Database.EnsureDeletedAsync(cancellationToken);
			await dataContext.Database.MigrateAsync(cancellationToken);
		}
	}

	public void SeedClients(CancellationToken cancellationToken)
	{
		// Seed clients
		Parallel.For(0, GeneratedClientsNumber, (_, _) =>
		{
			try
			{
				// Get required services
				using var serviceScope = _serviceScopeFactory.CreateScope();
				ISender sender = serviceScope.ServiceProvider.GetService<ISender>() ??
				                 throw new InvalidOperationException();

				CreateClientCommand createClientCommand = GenerateCreateClientCommand();
				Task<Entity> createClientResultTask = sender.Send(createClientCommand, cancellationToken);
				createClientResultTask.Wait(cancellationToken);
			}
			catch (Exception)
			{
				// ignored
			}
		});
	}

	public static CreateClientCommand GenerateCreateClientCommand()
	{
		const string password = $"Pass123$";
		const string dateFormat = "yyMMdd";
		const string numberFormat = "D4";
		
		var givenName = Faker.Name.First();
		var familyName = Faker.Name.Last();
		var email = Faker.Internet.Email($"{givenName.Normalize().ToUpperInvariant()}.{familyName.Normalize().ToUpperInvariant()}");
		var phone = Faker.Phone.Number();
		var dateBorn = Faker.Identification.DateOfBirth();
		var birthNumber = $"{dateBorn.ToString(dateFormat)}{Faker.RandomNumber.Next(0, 9999).ToString(numberFormat)}";
		
		return new CreateClientCommand(email, password, givenName, familyName, phone, dateBorn, birthNumber);
	}
}