using System.Diagnostics;
using Blogic.Crm.Infrastructure.Authentication;
using Blogic.Crm.Infrastructure.Data;
using Blogic.Crm.Infrastructure.Logging;
using Microsoft.EntityFrameworkCore;
using Serilog;

#region Serilog

var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

var loggerOptions = configuration.GetSection(nameof(LoggerOptions)).Get<LoggerOptions>()
                    ?? throw new InvalidOperationException();

LoggerBuilder loggerBuilder = new();

loggerBuilder.AddConsole(loggerOptions.Console);
loggerBuilder.AddSeq(loggerOptions.Seq);
loggerBuilder.AddEnriches();
loggerBuilder.OverrideMinimumLevels();

Log.Logger = loggerBuilder.CreateBootstrapLogger();

#endregion

try
{
	var applicationBuilder = WebApplication.CreateBuilder(args);

	applicationBuilder.Host.UseSerilog();

	applicationBuilder.Services.AddControllersWithViews();

	var connectionString = applicationBuilder.Configuration.GetConnectionString(nameof(DataContext));
	Debug.Assert(!string.IsNullOrEmpty(connectionString));
	
	applicationBuilder.Services.AddDbContext<DataContext>(options =>
	{
		options.UseSqlServer(connectionString, builder =>
		{
			builder.MigrationsAssembly(typeof(Program).Assembly.GetName().FullName);
		});
	});

	applicationBuilder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
	applicationBuilder.Services.AddScoped<ISecurityStampProvider, SecurityStampProvider>();
	applicationBuilder.Services.AddScoped<ILookupNormalizer, LookupNormalizer>();
	
	var application = applicationBuilder.Build();

	application.UseSerilogRequestLogging();

	if (!application.Environment.IsDevelopment())
	{
		application.UseExceptionHandler("/Home/Error");
		application.UseHsts();
	}

	application.UseHttpsRedirection();
	application.UseStaticFiles();
	application.UseRouting();
	application.UseAuthorization();

	application.MapControllerRoute(
		"default",
		"{controller=Home}/{action=Index}/{id?}");

	Log.Information("Application is starting up");
	application.Run();
}
catch (Exception exception)
{
	Log.Error("Application is shutting down. Message {Message}", exception.Message);
}
finally
{
	Log.CloseAndFlush();
}