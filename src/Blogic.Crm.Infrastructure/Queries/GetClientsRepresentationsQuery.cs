using Blogic.Crm.Domain.Data.Dtos;
using Blogic.Crm.Infrastructure.Filtering;
using Blogic.Crm.Infrastructure.Pagination;
using Blogic.Crm.Infrastructure.Searching;
using Blogic.Crm.Infrastructure.Sorting;
using Microsoft.EntityFrameworkCore;

namespace Blogic.Crm.Infrastructure.Queries;

/// <summary>
///     Gets the client data set as the data transfer object for the client.
/// </summary>
/// <param name="QueryString"></param>
public sealed record GetClientsRepresentationsQuery
	(ClientQueryString QueryString) : IQuery<PaginatedList<ClientRepresentation>>;

/// <summary>
///     Handles the <see cref="GetClientsRepresentationsQuery" /> query.
/// </summary>
public sealed class
	GetClientsRepresentationsQueryHandler : IQueryHandler<GetClientsRepresentationsQuery,
		PaginatedList<ClientRepresentation>>
{
	private readonly DataContext _dataContext;

	public GetClientsRepresentationsQueryHandler(DataContext dataContext)
	{
		_dataContext = dataContext;
	}

	public async Task<PaginatedList<ClientRepresentation>> Handle(GetClientsRepresentationsQuery request,
	                                                              CancellationToken cancellationToken)
	{
		// Filter the client data set using query string parameters and search for the term matches.
		var clientEntities = _dataContext.Clients.AsNoTracking()
		                                 .Filter(request.QueryString)
		                                 .Search(request.QueryString);

		// Keep track of the searches and filtered data set size for the pagination functionality.
		var totalClientEntities = clientEntities.Count();

		// Paginate data set using query string parameters.
		var orderedClientEntities = await clientEntities
		                                  .Sort(request.QueryString)
		                                  .Skip((request.QueryString.PageNumber - 1) * request.QueryString.PageSize)
		                                  .Take(request.QueryString.PageSize)
		                                  .ToListAsync(cancellationToken);

		// Map client data to their display form for the client.
		var clientRepresentations = orderedClientEntities.Adapt<List<ClientRepresentation>>();

		// Return paginated client data set in representative state.
		return new PaginatedList<ClientRepresentation>(
			clientRepresentations, totalClientEntities, request.QueryString.PageNumber,
			request.QueryString.PageSize);
	}
}