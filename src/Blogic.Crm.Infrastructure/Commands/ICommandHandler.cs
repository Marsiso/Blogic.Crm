namespace Blogic.Crm.Infrastructure.Commands;

/// <summary>
///     Abstraction for the CQRS design pattern command type request handler.
/// </summary>
public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
}