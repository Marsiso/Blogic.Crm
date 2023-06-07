#nullable disable

namespace Blogic.Crm.Infrastructure.Logging;

/// <summary>
///     Logging service provider settings.
/// </summary>
public sealed class LoggerOptions
{
    /// <summary>
    ///     Settings related to console.
    /// </summary>
    public ConsoleSinkOptions Console { get; set; }

    /// <summary>
    ///     Settings related to Seq.
    /// </summary>
    public SeqSinkOptions Seq { get; set; }
}

public sealed class ConsoleSinkOptions
{
    /// <summary>
    ///     Template to display log messages to the console.
    /// </summary>
    public string OutputTemplate { get; set; }
}

public sealed class SeqSinkOptions
{
    /// <summary>
    ///     Server address for centralized logging.
    /// </summary>
    public string ServerUrl { get; set; }
}