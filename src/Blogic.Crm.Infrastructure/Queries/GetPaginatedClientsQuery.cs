using Blogic.Crm.Domain.Data.Dtos;
using Blogic.Crm.Domain.Data.Entities;
using Blogic.Crm.Infrastructure.Filtering;
using Blogic.Crm.Infrastructure.Pagination;
using Blogic.Crm.Infrastructure.Persistence;
using Blogic.Crm.Infrastructure.Searching;
using Blogic.Crm.Infrastructure.Sorting;
using Blogic.Crm.Infrastructure.TypeExtensions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blogic.Crm.Infrastructure.Queries;

public sealed record GetPaginatedClientsQuery(ClientQueryStringParameters QueryStringParameters, bool TrackChanges) : IRequest<PaginatedList<ClientRepresentation>>;

public sealed class GetPaginatedClientsQueryHandler : IRequestHandler<GetPaginatedClientsQuery, PaginatedList<ClientRepresentation>>
{
	public GetPaginatedClientsQueryHandler(DataContext dataContext)
	{
		_dataContext = dataContext;
	}

	private readonly DataContext _dataContext;

	public async Task<PaginatedList<ClientRepresentation>> Handle(GetPaginatedClientsQuery request,
	                                                              CancellationToken cancellationToken)
	{
		var clientEntities = request.TrackChanges
			? _dataContext.Clients.AsTracking()
			              .FilterClients(request.QueryStringParameters)
			              .SearchClients(request.QueryStringParameters)
			: _dataContext.Clients.AsNoTracking()
			              .FilterClients(request.QueryStringParameters)
			              .SearchClients(request.QueryStringParameters);

		var totalClientEntities = clientEntities.Count();
		var orderedClientEntities= await clientEntities
		                                 .OrderClients(request.QueryStringParameters)
		                                 .Skip((request.QueryStringParameters.PageNumber - 1) * request.QueryStringParameters.PageSize)
		                                 .Take(request.QueryStringParameters.PageSize)
		                                 .ToListAsync(cancellationToken);

		var clientRepresentations = orderedClientEntities.Adapt<List<ClientRepresentation>>();
		
		return new PaginatedList<ClientRepresentation>(
			clientRepresentations, totalClientEntities, request.QueryStringParameters.PageNumber,
			request.QueryStringParameters.PageSize);
	}
}