using Blogic.Crm.Infrastructure.Authentication;

namespace Blogic.Crm.Web.Installers;

/// <summary>
/// Installer to the authentication related services.
/// </summary>
public sealed class AuthServiceInstaller : IServiceInstaller
{
	public void InstallServices(IServiceCollection services, IConfiguration configuration,
	                                          IWebHostEnvironment environment)
	{
		services.AddScoped<IPasswordHasher, PasswordHasher>();
		services.AddScoped<ISecurityStampProvider, SecurityStampProvider>();
		services.AddScoped<IEmailLookupNormalizer, EmailLookupNormalizer>();
		services.AddScoped<IPhoneLookupNormalizer, PhoneLookupNormalizer>();
	}
}