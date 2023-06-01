namespace Blogic.Crm.Web.Installers;

public sealed class RouteServiceInstaller : IServiceInstaller
{
	public void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
	{
		services.AddControllersWithViews();
	}
}