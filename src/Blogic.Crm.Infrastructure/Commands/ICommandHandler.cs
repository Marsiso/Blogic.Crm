namespace Blogic.Crm.Infrastructure.Commands;

/// <summary>
///     Abstraction for the <see cref="ICommand{TResponse}" /> request handler.
/// </summary>
/// <typeparam name="TCommand">Command to be processed by the command handler.</typeparam>
/// <typeparam name="TResponse">Result produced by the command handler.</typeparam>
public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
	where TCommand : ICommand<TResponse> { }