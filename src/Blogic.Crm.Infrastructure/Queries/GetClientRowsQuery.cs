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

/// <summary>
/// Gets the client data set ready for the CSV or JSON export.
/// </summary>
/// <param name="QueryString"></param>
public record GetClientRowsQuery(ClientQueryString QueryString) : IQuery<IEnumerable<ClientRow>>;

/// <summary>
/// <see cref="GetClientRowsQuery"/> query handler.
/// </summary>
public sealed class GetClientRowsQueryHandler : IQueryHandler<GetClientRowsQuery, IEnumerable<ClientRow>>
{
	public GetClientRowsQueryHandler(DataContext dataContext)
	{
		_dataContext = dataContext;
	}

	private readonly DataContext _dataContext;

	public async Task<IEnumerable<ClientRow>> Handle(GetClientRowsQuery request, CancellationToken cancellationToken)
	{
		// Retrieve the searched through, filtered, sorted and ordered client data set.
		List<Client> clientEntities = await _dataContext.Clients.AsNoTracking()
		                                               .FilterClients(request.QueryString)
		                                               .SearchClients(request.QueryString)
		                                               .OrderClients(request.QueryString)
		                                               .ToListAsync(cancellationToken);

		// Return the client data set ready for the export.
		return clientEntities.Adapt<List<ClientRow>>();
	}
}