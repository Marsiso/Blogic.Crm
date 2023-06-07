using Serilog.Core;
using Serilog.Extensions.Hosting;

namespace Blogic.Crm.Infrastructure.Logging;

/// <summary>
///     Abstraction for the application logger provider.
/// </summary>
public interface ILoggerBuilder
{
    /// <summary>
    ///     Creates an instance of the application logger. It is used for general purposes.
    /// </summary>
    Logger CreateLogger();

    /// <summary>
    ///     Creates an instance of the application logger. It is used for application bootstrapping purposes.
    /// </summary>
    ReloadableLogger CreateBootstrapLogger();
}