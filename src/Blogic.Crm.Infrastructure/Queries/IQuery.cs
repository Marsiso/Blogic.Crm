namespace Blogic.Crm.Infrastructure.Queries;

/// <summary>
///     Abstraction for the <see cref="IRequest" /> that represents Query operations in the CQRS pattern.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface IQuery<out TResponse> : IRequest<TResponse> { }