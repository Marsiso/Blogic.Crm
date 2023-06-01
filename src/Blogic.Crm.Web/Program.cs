using System.Diagnostics;
using Blogic.Crm.Infrastructure.Authentication;
using Blogic.Crm.Infrastructure.Commands;
using Blogic.Crm.Infrastructure.Data;
using Blogic.Crm.Infrastructure.Logging;
using Blogic.Crm.Infrastructure.Validations;
using Blogic.Crm.Infrastructure.Validators;
using Blogic.Crm.Web.Data;
using Blogic.Crm.Web.Installers;
using FluentValidation;
using MediatR;
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
	applicationBuilder.Services.InstallServicesInAssembly(applicationBuilder.Configuration,
	                                                      applicationBuilder.Environment, typeof(Program).Assembly);

	applicationBuilder.Services.AddHostedService<Seed>();
	
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
	Log.Error(exception, "Application is shutting down. Message {Message}", exception.Message);
}
finally
{
	Log.CloseAndFlush();
}