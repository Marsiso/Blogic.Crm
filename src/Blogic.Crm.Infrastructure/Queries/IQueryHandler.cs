namespace Blogic.Crm.Infrastructure.Queries;

/// <summary>
///     Abstraction for the CQRS design pattern query type request handler.
/// </summary>
public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
}