using Serilog.Core;
using Serilog.Extensions.Hosting;

namespace Blogic.Crm.Infrastructure.Logging;

/// <summary>
///     Abstraction for the application logger provider.
/// </summary>
public interface ILoggerBuilder
{
    /// <summary>
    ///     Creates application logger instance. Used for the general purposes.
    /// </summary>
    /// <returns>Application logger instance.</returns>
    Logger CreateLogger();

    /// <summary>
    ///     Creates application logger instance for the application bootstrapping process.
    /// </summary>
    /// <returns>Application bootstrapping logger instance.</returns>
    ReloadableLogger CreateBootstrapLogger();
}