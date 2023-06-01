using System.Reflection;

namespace Blogic.Crm.Web.Installers;

public static class ServiceInstallerExtensions
{
	public static void InstallServicesInAssembly(this IServiceCollection services, IConfiguration configuration,
	                                             IWebHostEnvironment environment, Assembly assembly)
	{
		var installers = assembly.ExportedTypes
		                         .Where(exportedType => typeof(IServiceInstaller).IsAssignableFrom(exportedType)
		                                                && exportedType is { IsAbstract: false, IsInterface: false })
		                         .Select(Activator.CreateInstance)
		                         .Cast<IServiceInstaller>()
		                         .ToList();
		
		installers.ForEach(installer => installer.InstallServices(services, configuration,environment));
	}
}