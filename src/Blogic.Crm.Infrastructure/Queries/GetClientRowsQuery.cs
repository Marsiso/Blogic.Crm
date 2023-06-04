using Blogic.Crm.Domain.Data.Dtos;
using Blogic.Crm.Domain.Data.Entities;
using Blogic.Crm.Infrastructure.Filtering;
using Blogic.Crm.Infrastructure.Pagination;
using Blogic.Crm.Infrastructure.Persistence;
using Blogic.Crm.Infrastructure.Searching;
using Blogic.Crm.Infrastructure.Sorting;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Blogic.Crm.Infrastructure.Queries;

public record GetClientRowsQuery(ClientQueryString QueryString) : IQuery<IEnumerable<ClientRow>>;

public sealed class GetClientRowsQueryHandler : IQueryHandler<GetClientRowsQuery, IEnumerable<ClientRow>>
{
	public GetClientRowsQueryHandler(DataContext dataContext)
	{
		_dataContext = dataContext;
	}

	private readonly DataContext _dataContext;

	public async Task<IEnumerable<ClientRow>> Handle(GetClientRowsQuery request, CancellationToken cancellationToken)
	{
		List<Client> clientEntities = await _dataContext.Clients.AsNoTracking()
		                                               .FilterClients(request.QueryString)
		                                               .SearchClients(request.QueryString)
		                                               .OrderClients(request.QueryString)
		                                               .ToListAsync(cancellationToken);

		return clientEntities.Adapt<List<ClientRow>>();
	}
}