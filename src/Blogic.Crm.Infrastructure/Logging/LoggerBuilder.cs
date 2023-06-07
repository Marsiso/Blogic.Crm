using System.Diagnostics;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Extensions.Hosting;

namespace Blogic.Crm.Infrastructure.Logging;

/// <summary>
///     Application logger builder.
/// </summary>
public sealed class LoggerBuilder : ILoggerBuilder
{
    /// <summary>
    ///     Logging service provider configuration.
    /// </summary>
    public readonly LoggerConfiguration LoggerConfiguration;

    public LoggerBuilder()
    {
        LoggerConfiguration = new LoggerConfiguration();
    }

    public Logger CreateLogger()
    {
        return LoggerConfiguration.CreateLogger();
    }

    public ReloadableLogger CreateBootstrapLogger()
    {
        return LoggerConfiguration.CreateBootstrapLogger();
    }

    /// <summary>
    ///     Adds support for logging to the console.
    /// </summary>
    public void AddConsole(ConsoleSinkOptions options)
    {
        Debug.Assert(options != null);
        Debug.Assert(!string.IsNullOrEmpty(options.OutputTemplate));

        LoggerConfiguration.WriteTo.Console(outputTemplate: options.OutputTemplate);
    }

    /// <summary>
    ///     Adds support for logging to the Seq.
    /// </summary>
    public void AddSeq(SeqSinkOptions options)
    {
        Debug.Assert(options != null);
        Debug.Assert(Uri.IsWellFormedUriString(options.ServerUrl, UriKind.Absolute));

        LoggerConfiguration.WriteTo.Seq(options.ServerUrl);
    }

    /// <summary>
    ///     Adds additional data to the logs.
    /// </summary>
    public void AddEnriches()
    {
        LoggerConfiguration.Enrich.WithProcessId()
            .Enrich.WithProcessName()
            .Enrich.WithThreadId()
            .Enrich.WithThreadName()
            .Enrich.WithEnvironmentName()
            .Enrich.WithEnvironmentUserName()
            .Enrich.WithMachineName()
            .Enrich.FromLogContext();
    }

    /// <summary>
    ///     Resets the minimum log severity levels for selected sources.
    /// </summary>
    public LoggerConfiguration OverrideMinimumLevels()
    {
        return LoggerConfiguration.MinimumLevel.Override("System", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning);
    }
}