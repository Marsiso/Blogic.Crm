namespace Blogic.Crm.Web.Installers;

/// <summary>
///     Installer for the routing, controller and view related services.
/// </summary>
public sealed class RouteServiceInstaller : IServiceInstaller
{
	public void InstallServices(IServiceCollection services, IConfiguration configuration,
	                            IWebHostEnvironment environment)
	{
		services.AddControllersWithViews();
	}
}