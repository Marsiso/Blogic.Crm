using System.Diagnostics;
using Blogic.Crm.Infrastructure.Persistence;
using Blogic.Crm.Web.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Blogic.Crm.Web.Installers;

/// <summary>
/// Installer for the database related services.
/// </summary>
public sealed class DbServiceInstaller : IServiceInstaller
{
	public void InstallServices(IServiceCollection services, IConfiguration configuration,
	                                          IWebHostEnvironment environment)
	{
		var connectionString = configuration.GetConnectionString(nameof(DataContext));
		Debug.Assert(StringExtensions.IsNotNullOrEmpty(connectionString));
	
		services.AddDbContext<DataContext>(options =>
		{
			options.UseSqlServer(connectionString, builder =>
			{
				builder.MigrationsAssembly(typeof(Program).Assembly.GetName().FullName);
			});

			options.EnableSensitiveDataLogging();
			options.EnableDetailedErrors();
		});

	
		services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
		services.AddScoped<DatabaseSeeder>();
	}
}