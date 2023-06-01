namespace Blogic.Crm.Web.Installers;

public interface IServiceInstaller
{
	void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment);
}