using System.Diagnostics;
using Blogic.Crm.Domain.Data.Entities;
using Blogic.Crm.Infrastructure.Pagination;

namespace Blogic.Crm.Infrastructure.Filtering;

public static class FilterFunctions
{
	public static IQueryable<Client> FilterClients(this IQueryable<Client> clients,
	                                               ClientQueryString queryString)
	{
		Debug.Assert(queryString != null);
		return clients.Where(c => c.DateBorn > queryString.MinDateBorn &&
		                          c.DateBorn < queryString.MaxDateBorn);
	}
}