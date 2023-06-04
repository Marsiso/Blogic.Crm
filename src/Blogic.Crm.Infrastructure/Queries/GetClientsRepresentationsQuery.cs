using Blogic.Crm.Domain.Data.Dtos;
using Blogic.Crm.Infrastructure.Filtering;
using Blogic.Crm.Infrastructure.Pagination;
using Blogic.Crm.Infrastructure.Persistence;
using Blogic.Crm.Infrastructure.Searching;
using Blogic.Crm.Infrastructure.Sorting;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Blogic.Crm.Infrastructure.Queries;

public sealed record GetClientsRepresentationsQuery(ClientQueryString QueryString) : IQuery<PaginatedList<ClientRepresentation>>;

public sealed class GetClientsRepresentationsQueryHandler : IQueryHandler<GetClientsRepresentationsQuery, PaginatedList<ClientRepresentation>>
{
	public GetClientsRepresentationsQueryHandler(DataContext dataContext)
	{
		_dataContext = dataContext;
	}

	private readonly DataContext _dataContext;

	public async Task<PaginatedList<ClientRepresentation>> Handle(GetClientsRepresentationsQuery request,
	                                                              CancellationToken cancellationToken)
	{
		var clientEntities = _dataContext.Clients.AsNoTracking()
		                                 .FilterClients(request.QueryString)
		                                 .SearchClients(request.QueryString);

		var totalClientEntities = clientEntities.Count();
		var orderedClientEntities= await clientEntities
		                                 .OrderClients(request.QueryString)
		                                 .Skip((request.QueryString.PageNumber - 1) * request.QueryString.PageSize)
		                                 .Take(request.QueryString.PageSize)
		                                 .ToListAsync(cancellationToken);

		var clientRepresentations = orderedClientEntities.Adapt<List<ClientRepresentation>>();
		
		return new PaginatedList<ClientRepresentation>(
			clientRepresentations, totalClientEntities, request.QueryString.PageNumber,
			request.QueryString.PageSize);
	}
}