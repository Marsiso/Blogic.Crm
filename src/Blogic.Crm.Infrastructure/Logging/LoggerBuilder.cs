using Serilog;
using Serilog.Core;
using Serilog.Extensions.Hosting;
using System.Diagnostics;

namespace Blogic.Crm.Infrastructure.Logging;

/// <summary>
/// Application logger builder.
/// </summary>
public sealed class LoggerBuilder : ILoggerBuilder
{
    public readonly LoggerConfiguration LoggerConfiguration;

    public LoggerBuilder()
    {
        LoggerConfiguration = new LoggerConfiguration();
    }

    /// <summary>
    /// Adds the logging to the console sink.
    /// </summary>
    /// <param name="options">Options used to configure the console sink.</param>
    /// <returns>The application logger builder instance that is being configured.</returns>
    public LoggerConfiguration AddConsole(ConsoleSinkOptions options)
    {
        Debug.Assert(options != null);
        Debug.Assert(!string.IsNullOrEmpty(options.OutputTemplate));

        return LoggerConfiguration.WriteTo.Console(outputTemplate: options.OutputTemplate);
    }

    /// <summary>
    /// Adds the logging to the Seq sink.
    /// </summary>
    /// <param name="options">Options used to configure the Seq sink.</param>
    /// <returns>The application logger builder instance that is being configured.</returns>
    public LoggerConfiguration AddSeq(SeqSinkOptions options)
    {
        Debug.Assert(options != null);
        Debug.Assert(Uri.IsWellFormedUriString(options.ServerUrl, UriKind.Absolute));

        return LoggerConfiguration.WriteTo.Seq(serverUrl: options.ServerUrl);
    }

    /// <summary>
    /// Adds an additional data to logs.
    /// </summary>
    /// <returns>The application logger builder instance that is being configured.</returns>
    public LoggerConfiguration AddEnriches()
    {
        return LoggerConfiguration.Enrich.WithProcessId()
            .Enrich.WithProcessName()
            .Enrich.WithThreadId()
            .Enrich.WithThreadName()
            .Enrich.WithEnvironmentName()
            .Enrich.WithEnvironmentUserName()
            .Enrich.WithMachineName()
            .Enrich.FromLogContext();
    }

    /// <summary>
    /// Overrides minimal logging levels for selected sources.
    /// </summary>
    /// <returns>The application logger builder instance that is being configured.</returns>
    public LoggerConfiguration OverrideMinimumLevels()
    {
        return LoggerConfiguration.MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning);
    }
    
    public Logger CreateLogger()
    {
        return LoggerConfiguration.CreateLogger();
    }

    public ReloadableLogger CreateBootstrapLogger()
    {
        return LoggerConfiguration.CreateBootstrapLogger();
    }
}
