namespace Blogic.Crm.Infrastructure.Commands;

/// <summary>
///     Abstraction for CQRS design pattern command type requirements.
/// </summary>
public interface ICommand<out TResponse> : IRequest<TResponse>
{
}