using Blogic.Crm.Domain.Data.Dtos;
using Blogic.Crm.Infrastructure.Pagination;
using Blogic.Crm.Infrastructure.Searching;
using Blogic.Crm.Infrastructure.Sorting;
using Microsoft.EntityFrameworkCore;

namespace Blogic.Crm.Infrastructure.Queries;

public sealed record GetContractRepresentationsQuery(ContractQueryString QueryString) : IQuery<PaginatedList<ContractRepresentation>>;

public sealed class GetContractRepresentationsQueryHandler : IQueryHandler<GetContractRepresentationsQuery, PaginatedList<ContractRepresentation>>
{
	public GetContractRepresentationsQueryHandler(DataContext dataContext)
	{
		_dataContext = dataContext;
	}

	private readonly DataContext _dataContext;

	public async Task<PaginatedList<ContractRepresentation>> Handle(GetContractRepresentationsQuery request, CancellationToken cancellationToken)
	{
		IQueryable<Contract> contractEntities = _dataContext.Contracts
		                                                    .AsNoTracking()
		                                                    .Search(request.QueryString);
		
		var totalItems = contractEntities.Count();
		List<Contract> orderedContracts = await contractEntities
		                                 .Sort(request.QueryString)
		                                 .Skip((request.QueryString.PageNumber - 1) * request.QueryString.PageSize)
		                                 .Take(request.QueryString.PageSize)
		                                 .ToListAsync(cancellationToken);

		List<ContractRepresentation> contractRepresentations = orderedContracts.Adapt<List<ContractRepresentation>>();
		return new PaginatedList<ContractRepresentation>(contractRepresentations, totalItems, request.QueryString.PageNumber, request.QueryString.PageSize);
	}
}