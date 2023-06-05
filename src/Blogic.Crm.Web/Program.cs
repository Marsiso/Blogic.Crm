using Blogic.Crm.Infrastructure.Logging;
using Blogic.Crm.Web.Installers;
using Blogic.Crm.Web.Persistence;
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
	applicationBuilder.Services.InstallServicesInAssembly(applicationBuilder.Configuration,
	                                                      applicationBuilder.Environment, typeof(Program).Assembly);

	var application = applicationBuilder.Build();

	using var scope = application.Services.CreateScope();
	using var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
	seeder.Seed();

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
	application.MapControllers();

	Log.Information("Application is starting up");
	application.Run();
}
catch (Exception exception)
{
	Log.Error(exception, "Application is shutting down. Message {Message}", exception.Message);
}
finally
{
	Log.CloseAndFlush();
}