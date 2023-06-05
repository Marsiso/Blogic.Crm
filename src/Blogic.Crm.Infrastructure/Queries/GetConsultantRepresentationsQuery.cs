using Blogic.Crm.Domain.Data.Dtos;
using Blogic.Crm.Infrastructure.Filtering;
using Blogic.Crm.Infrastructure.Pagination;
using Blogic.Crm.Infrastructure.Searching;
using Blogic.Crm.Infrastructure.Sorting;
using Microsoft.EntityFrameworkCore;

namespace Blogic.Crm.Infrastructure.Queries;

public sealed record GetConsultantRepresentationsQuery
	(ConsultantQueryString QueryString) : IQuery<PaginatedList<ConsultantRepresentation>>;

public sealed class
	GetConsultantRepresentationsQueryHandler : IQueryHandler<GetConsultantRepresentationsQuery,
		PaginatedList<ConsultantRepresentation>>
{
	private readonly DataContext _dataContext;

	public GetConsultantRepresentationsQueryHandler(DataContext dataContext)
	{
		_dataContext = dataContext;
	}

	public async Task<PaginatedList<ConsultantRepresentation>> Handle(GetConsultantRepresentationsQuery request,
	                                                                  CancellationToken cancellationToken)
	{
		// Filter the consultant data set using query string parameters and search for the term matches.
		var consultantEntities = _dataContext.Consultants.AsNoTracking()
		                                     .Filter(request.QueryString)
		                                     .Search(request.QueryString);

		// Keep track of the searches and filtered data set size for the pagination functionality.
		var totalConsultantEntities = consultantEntities.Count();

		// Paginate data set using query string parameters.
		var orderedConsultantEntities = await consultantEntities
		                                      .Sort(request.QueryString)
		                                      .Skip((request.QueryString.PageNumber - 1) * request.QueryString.PageSize)
		                                      .Take(request.QueryString.PageSize)
		                                      .ToListAsync(cancellationToken);

		// Map consultant data to their display form for the client.
		var consultantRepresentations = orderedConsultantEntities.Adapt<List<ConsultantRepresentation>>();

		// Return paginated consultant data set in representative state.
		return new PaginatedList<ConsultantRepresentation>(
			consultantRepresentations, totalConsultantEntities, request.QueryString.PageNumber,
			request.QueryString.PageSize);
	}
}