using System.Diagnostics;
using Blogic.Crm.Domain.Data.Entities;
using Blogic.Crm.Infrastructure.Pagination;

namespace Blogic.Crm.Infrastructure.Filtering;

/// <summary>
/// Provides LINQ extensions related to the data set filtering using the query string parameters.
/// </summary>
public static class FilterFunctions
{
	/// <summary>
	/// Filters the <see cref="Client"/> data set using the <see cref="ClientQueryString"/> query string
	/// parameters.
	/// </summary>
	/// <param name="clients">The <see cref="Client"/> data set to be filtered.</param>
	/// <param name="queryString">Provided query string used to filter the <see cref="Client"/> data set.</param>
	/// <returns>The filtered <see cref="Client"/> data set.</returns>
	public static IQueryable<Client> FilterClients(this IQueryable<Client> clients,
	                                               ClientQueryString queryString)
	{
		Debug.Assert(queryString != null);
		return clients.Where(c => c.DateBorn > queryString.MinDateBorn &&
		                          c.DateBorn < queryString.MaxDateBorn);
	}
}