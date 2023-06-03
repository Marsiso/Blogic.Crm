using System.Diagnostics;
using Blogic.Crm.Domain.Data.Entities;
using Blogic.Crm.Infrastructure.Pagination;

namespace Blogic.Crm.Infrastructure.Filtering;

public static class FilterFunctions
{
	public static IQueryable<Client> FilterClients(this IQueryable<Client> clients,
	                                               ClientQueryStringParameters queryStringParameters)
	{
		Debug.Assert(queryStringParameters != null);
		return clients.Where(c => c.DateBorn > queryStringParameters.MinDateBorn &&
		                          c.DateBorn < queryStringParameters.MaxDateBorn);
	}
}