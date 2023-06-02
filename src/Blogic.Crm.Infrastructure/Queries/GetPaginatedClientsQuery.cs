using Blogic.Crm.Domain.Data.Dtos;
using Blogic.Crm.Domain.Data.Entities;
using Blogic.Crm.Infrastructure.Data;
using Blogic.Crm.Infrastructure.Pagination;
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
			: _dataContext.Clients.AsNoTracking();

		if (StringExtensions.IsNotNullOrEmpty(request.QueryStringParameters.SearchString))
		{
		}

		var totalClientEntities = clientEntities.Count();
		
		
		var orderedClientEntities= await clientEntities
		                                 .OrderClients(request.QueryStringParameters.SortOrder)
		                                 .Skip((request.QueryStringParameters.PageNumber - 1) * request.QueryStringParameters.PageSize)
		                                 .Take(request.QueryStringParameters.PageSize)
		                                 .ToListAsync(cancellationToken);

		var clientRepresentations = orderedClientEntities.Adapt<List<ClientRepresentation>>();
		
		return new PaginatedList<ClientRepresentation>(
			clientRepresentations, totalClientEntities, request.QueryStringParameters.PageNumber,
			request.QueryStringParameters.PageSize);
	}
}