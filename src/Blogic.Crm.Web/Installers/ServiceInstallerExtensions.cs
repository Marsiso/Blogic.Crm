using System.Reflection;

namespace Blogic.Crm.Web.Installers;

/// <summary>
///     Extensions for the <see cref="IServiceInstaller" /> implementations.
/// </summary>
public static class ServiceInstallerExtensions
{
	/// <summary>
	///     Executes every concrete implementation of <see cref="IServiceInstaller" /> abstraction in the given assembly.
	/// </summary>
	/// <param name="services">Application's service collection.</param>
	/// <param name="configuration">Application's configuration.</param>
	/// <param name="environment">Application's host environment.</param>
	/// <param name="assembly">Assembly from which the concrete installer implementations should be executed.</param>
	public static void InstallServicesInAssembly(this IServiceCollection services, IConfiguration configuration,
	                                             IWebHostEnvironment environment, Assembly assembly)
	{
		// Search for the concrete implementations in the assembly.
		var installers = assembly.ExportedTypes
		                         .Where(exportedType => typeof(IServiceInstaller).IsAssignableFrom(exportedType)
		                                                && exportedType is { IsAbstract: false, IsInterface: false })
		                         .Select(Activator.CreateInstance)
		                         .Cast<IServiceInstaller>()
		                         .ToList();

		// Execute every installer.
		installers.ForEach(installer => installer.InstallServices(services, configuration, environment));
	}
}