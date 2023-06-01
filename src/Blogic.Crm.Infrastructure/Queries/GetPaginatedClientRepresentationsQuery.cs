using Blogic.Crm.Domain.Data.Dtos;
using Blogic.Crm.Domain.Data.Entities;
using Blogic.Crm.Infrastructure.Data;
using Blogic.Crm.Infrastructure.Paging;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blogic.Crm.Infrastructure.Queries;

public sealed record GetPaginatedClientRepresentationsQuery(int PageNumber, int PageSize, ClientSortOrder SortOrder,
                                                            bool TrackChanges) : IRequest<PaginatedList<ClientRepresentation>>;

public sealed class GetPaginatedClientRepresentationsQueryHandler : IRequestHandler<GetPaginatedClientRepresentationsQuery, PaginatedList<ClientRepresentation>>
{
	public GetPaginatedClientRepresentationsQueryHandler(DataContext dataContext)
	{
		_dataContext = dataContext;
	}

	private readonly DataContext _dataContext;

	public async Task<PaginatedList<ClientRepresentation>> Handle(GetPaginatedClientRepresentationsQuery request,
	                                                              CancellationToken cancellationToken)
	{
		var clientEntities = request.TrackChanges
			? _dataContext.Clients.AsTracking()
			: _dataContext.Clients.AsNoTracking();

		var totalClientEntities = clientEntities.Count();
		
		var orderedClientEntities= await OrderClientsByProperty(clientEntities, request.SortOrder)
		                           .Skip((request.PageNumber - 1) * request.PageSize)
		                           .Take(request.PageSize)
		                           .ToListAsync(cancellationToken);

		var clientRepresentations = orderedClientEntities.Adapt<List<ClientRepresentation>>();
		
		return new PaginatedList<ClientRepresentation>(
			clientRepresentations, totalClientEntities, request.PageNumber,
			request.PageSize);
	}

	public IOrderedQueryable<Client> OrderClientsByProperty(IQueryable<Client> clientEntities, ClientSortOrder sortOrder)
	{
		return sortOrder switch
		{
			ClientSortOrder.Id => clientEntities.OrderBy(c => c.Id),
			ClientSortOrder.IdDesc => clientEntities.OrderByDescending(c => c.Id),
			ClientSortOrder.GivenName => clientEntities.OrderBy(c => c.GivenName),
			ClientSortOrder.GivenNameDesc => clientEntities.OrderByDescending(c => c.GivenName),
			ClientSortOrder.FamilyName => clientEntities.OrderBy(c => c.FamilyName),
			ClientSortOrder.FamilyNameDesc => clientEntities.OrderByDescending(c => c.FamilyName),
			ClientSortOrder.Email => clientEntities.OrderBy(c => c.Email),
			ClientSortOrder.EmailDesc => clientEntities.OrderByDescending(c => c.Email),
			ClientSortOrder.Phone => clientEntities.OrderBy(c => c.Phone),
			ClientSortOrder.PhoneDesc => clientEntities.OrderByDescending(c => c.Phone),
			ClientSortOrder.DateBorn => clientEntities.OrderBy(c => c.DateBorn),
			ClientSortOrder.DateBornDesc => clientEntities.OrderByDescending(c => c.DateBorn),
			ClientSortOrder.IsEmailConfirmed => clientEntities.OrderBy(c => c.IsEmailConfirmed),
			ClientSortOrder.IsEmailConfirmedDesc => clientEntities.OrderByDescending(c => c.IsEmailConfirmed),
			ClientSortOrder.IsPhoneConfirmed => clientEntities.OrderBy(c => c.IsPhoneConfirmed),
			ClientSortOrder.IsPhoneConfirmedDesc => clientEntities.OrderByDescending(c => c.IsPhoneConfirmed),
			ClientSortOrder.BirthNumber => clientEntities.OrderBy(c => c.BirthNumber),
			ClientSortOrder.BirthNumberDesc => clientEntities.OrderByDescending(c => c.BirthNumber),
			_ => clientEntities.OrderByDescending(c => c.FamilyName)
		};
	}
}