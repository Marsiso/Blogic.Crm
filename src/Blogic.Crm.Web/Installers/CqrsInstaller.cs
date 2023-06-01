using Blogic.Crm.Infrastructure.Commands;
using Blogic.Crm.Infrastructure.Validations;
using MediatR;

namespace Blogic.Crm.Web.Installers;

public sealed class CqrsInstaller : IServiceInstaller
{
	public void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
	{
		services.AddMediatR(mediatrConfiguration => mediatrConfiguration.RegisterServicesFromAssembly(typeof(CreateClientCommandHandler).Assembly));
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
	}
}