namespace Blogic.Crm.Web.Installers;

/// <summary>
///     Abstraction for the service installer that handles dependency injection services registration.
/// </summary>
public interface IServiceInstaller
{
	/// <summary>
	///     Registers provided services into the dependency injection container.
	/// </summary>
	/// <param name="services">Application service collection.</param>
	/// <param name="configuration">Application configuration.</param>
	/// <param name="environment">Application host environment.</param>
	void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment);
}