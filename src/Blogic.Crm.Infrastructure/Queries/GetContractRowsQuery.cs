using Blogic.Crm.Domain.Data.Dtos;
using Blogic.Crm.Infrastructure.Filtering;
using Blogic.Crm.Infrastructure.Pagination;
using Blogic.Crm.Infrastructure.Searching;
using Blogic.Crm.Infrastructure.Sorting;
using Microsoft.EntityFrameworkCore;

namespace Blogic.Crm.Infrastructure.Queries;

public record GetContractRowsQuery(ContractQueryString QueryString) : IQuery<IEnumerable<ContractRow>>;

public sealed class GetContractRowsQueryHandler : IQueryHandler<GetContractRowsQuery, IEnumerable<ContractRow>>
{
	public GetContractRowsQueryHandler(DataContext dataContext)
	{
		_dataContext = dataContext;
	}

	private readonly DataContext _dataContext;
	
	public async Task<IEnumerable<ContractRow>> Handle(GetContractRowsQuery request, CancellationToken cancellationToken)
	{
		// Retrieve the searched through, filtered, sorted and ordered consultant data set.
		var contractEntities = await _dataContext.Contracts.AsNoTracking()
		                                           .Filter(request.QueryString)
		                                           .Search(request.QueryString)
		                                           .Sort(request.QueryString)
		                                           .ToListAsync(cancellationToken);

		// Return the consultant data set ready for the export.
		return contractEntities.Adapt<List<ContractRow>>();
	}
}