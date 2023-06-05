namespace Blogic.Crm.Infrastructure.Queries;

/// <summary>
///     Abstraction for the <see cref="IQuery{TResponse}" /> request handler.
/// </summary>
/// <typeparam name="TQuery">Query to be processed by the query handler.</typeparam>
/// <typeparam name="TResponse">Result produced by the query handler.</typeparam>
public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
	where TQuery : IQuery<TResponse> { }