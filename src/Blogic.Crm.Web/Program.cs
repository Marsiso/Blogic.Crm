using Blogic.Crm.Infrastructure.Logging;
using Serilog;

#region Serilog

IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

LoggerOptions loggerOptions = configuration.GetSection(nameof(LoggerOptions)).Get<LoggerOptions>()
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
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    builder.Services.AddControllersWithViews();

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    Log.Information("Application is starting up");
    app.Run();
}
catch (Exception exception)
{
    Log.Error("Application is shutting down. Message {Message}", exception.Message);
}
finally
{
    Log.CloseAndFlush();
}