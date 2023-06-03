using MediatR;

namespace Blogic.Crm.Infrastructure.Commands;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
	where TCommand : ICommand<TResponse>
{
}