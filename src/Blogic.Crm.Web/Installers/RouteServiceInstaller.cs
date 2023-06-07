namespace Blogic.Crm.Web.Installers;

/// <summary>
///     Installer services that are related to routing, controllers, and views.
/// </summary>
public sealed class RouteServiceInstaller : IServiceInstaller
{
	public void InstallServices(IServiceCollection services, IConfiguration configuration,
	                            IWebHostEnvironment environment)
	{
		services.AddControllersWithViews();
	}
}