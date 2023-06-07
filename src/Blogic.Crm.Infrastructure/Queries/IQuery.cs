namespace Blogic.Crm.Infrastructure.Queries;

/// <summary>
///     Abstraction for CQRS design pattern query type requirements.
/// </summary>
public interface IQuery<out TResponse> : IRequest<TResponse>
{
}