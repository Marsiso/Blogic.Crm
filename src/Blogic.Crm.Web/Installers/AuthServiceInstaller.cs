using Blogic.Crm.Infrastructure.Authentication;

namespace Blogic.Crm.Web.Installers;

/// <summary>
///     Installer of services related to user account management, authentication and authorization.
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