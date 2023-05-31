using Serilog.Core;
using Serilog.Extensions.Hosting;

namespace Blogic.Crm.Infrastructure.Logging;

public interface ILoggerBuilder
{
    Logger CreateLogger();
    ReloadableLogger CreateBootstrapLogger();
}
