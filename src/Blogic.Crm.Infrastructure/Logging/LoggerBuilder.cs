using Serilog;
using Serilog.Core;
using Serilog.Extensions.Hosting;
using System.Diagnostics;

namespace Blogic.Crm.Infrastructure.Logging;

public sealed class LoggerBuilder : ILoggerBuilder
{
    public readonly LoggerConfiguration LoggerConfiguration;

    public LoggerBuilder()
    {
        LoggerConfiguration = new LoggerConfiguration();
    }

    public LoggerConfiguration AddConsole(ConsoleSinkOptions options)
    {
        Debug.Assert(options != null);
        Debug.Assert(!string.IsNullOrEmpty(options.OutputTemplate));

        return LoggerConfiguration.WriteTo.Console(outputTemplate: options.OutputTemplate);
    }

    public LoggerConfiguration AddSeq(SeqSinkOptions options)
    {
        Debug.Assert(options != null);
        Debug.Assert(Uri.IsWellFormedUriString(options.ServerUrl, UriKind.Absolute));

        return LoggerConfiguration.WriteTo.Seq(serverUrl: options.ServerUrl);
    }

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
