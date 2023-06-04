using Blogic.Crm.Infrastructure.Validators;
using FluentValidation;

namespace Blogic.Crm.Web.Installers;

/// <summary>
/// Installer for the validation related services.
/// </summary>
public sealed class ValidatorServiceInstaller : IServiceInstaller
{
	public void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
	{
		services.AddValidatorsFromAssembly(typeof(CreateClientCommandValidator).Assembly);
	}
}