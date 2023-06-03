using MediatR;

namespace Blogic.Crm.Infrastructure.Commands;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}