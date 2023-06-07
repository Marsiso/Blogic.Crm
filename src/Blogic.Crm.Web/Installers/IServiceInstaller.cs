namespace Blogic.Crm.Web.Installers;

/// <summary>
///     Abstraction for service installer.
/// </summary>
public interface IServiceInstaller
{
	/// <summary>
	///     Registers services.
	/// </summary>
	void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment);
}