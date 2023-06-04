using MediatR;

namespace Blogic.Crm.Infrastructure.Commands;

/// <summary>
/// Abstraction for the <see cref="IRequest"/> that represents Command operations in the CQRS pattern.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface ICommand<out TResponse> : IRequest<TResponse>
{
}