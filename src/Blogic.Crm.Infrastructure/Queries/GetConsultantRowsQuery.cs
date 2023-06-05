using Blogic.Crm.Domain.Data.Dtos;
using Blogic.Crm.Infrastructure.Filtering;
using Blogic.Crm.Infrastructure.Pagination;
using Blogic.Crm.Infrastructure.Searching;
using Blogic.Crm.Infrastructure.Sorting;
using Microsoft.EntityFrameworkCore;

namespace Blogic.Crm.Infrastructure.Queries;

/// <summary>
///     Gets the consultant data set ready for the CSV or JSON export.
/// </summary>
/// <param name="QueryString"></param>
public sealed record GetConsultantRowsQuery(ConsultantQueryString QueryString) : IQuery<IEnumerable<ConsultantRow>>;

/// <summary>
///     <see cref="GetConsultantRowsQuery" /> query handler.
/// </summary>
public sealed class GetConsultantRowsQueryHandler : IQueryHandler<GetConsultantRowsQuery, IEnumerable<ConsultantRow>>
{
	private readonly DataContext _dataContext;

	public GetConsultantRowsQueryHandler(DataContext dataContext)
	{
		_dataContext = dataContext;
	}

	public async Task<IEnumerable<ConsultantRow>> Handle(GetConsultantRowsQuery request,
	                                                     CancellationToken cancellationToken)
	{
		// Retrieve the searched through, filtered, sorted and ordered consultant data set.
		var consultantEntities = await _dataContext.Consultants.AsNoTracking()
		                                           .Filter(request.QueryString)
		                                           .Search(request.QueryString)
		                                           .Sort(request.QueryString)
		                                           .ToListAsync(cancellationToken);

		// Return the consultant data set ready for the export.
		return consultantEntities.Adapt<List<ConsultantRow>>();
	}
}