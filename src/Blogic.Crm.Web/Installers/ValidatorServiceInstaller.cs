using Blogic.Crm.Infrastructure.Validators;
using FluentValidation;

namespace Blogic.Crm.Web.Installers;

/// <summary>
///     Service Installer Extension Features.
/// </summary>
public sealed class ValidatorServiceInstaller : IServiceInstaller
{
	/// <summary>
	///		Registers all services in the assembly.
	/// </summary>
	public void InstallServices(IServiceCollection services, IConfiguration configuration,
	                            IWebHostEnvironment environment)
	{
		services.AddValidatorsFromAssembly(typeof(CreateClientCommandValidator).Assembly);
	}
}