using MediatR;

namespace Blogic.Crm.Infrastructure.Queries;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}