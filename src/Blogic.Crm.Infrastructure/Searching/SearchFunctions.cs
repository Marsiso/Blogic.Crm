using System.Diagnostics;
using Blogic.Crm.Domain.Data.Entities;
using Blogic.Crm.Infrastructure.Pagination;

namespace Blogic.Crm.Infrastructure.Searching;

/// <summary>
/// Provides LINQ extensions related to the data set searching using the query string parameters.
/// </summary>
public static class SearchFunctions
{
	/// <summary>
	/// Searches the client data set for the matches using the provided search string.
	/// </summary>
	/// <param name="clients">The client data set to be searched through.</param>
	/// <param name="queryString">Query string to be used to search for search term matches.</param>
	/// <returns>The filtered <see cref="Client"/> data set.</returns>
	public static IQueryable<Client> SearchClients(this IQueryable<Client> clients,
	                                               ClientQueryString queryString)
	{
		// When the provided search string is empty then do not search for the term occurences.
		Debug.Assert(queryString != null);
		if (string.IsNullOrEmpty(queryString.SearchString))
		{
			return clients;
		}

		// Search for term matches in the provided client data set.
		string[] searchTerms = queryString.SearchString.Split(' ');
		return searchTerms.Aggregate(clients, (current, searchTerm) =>
		{
			return current.Where(c => c.BirthNumber.Contains(searchTerm) ||
			                          c.Phone.Contains(searchTerm) ||
			                          c.GivenName.Contains(searchTerm) ||
			                          c.FamilyName.Contains(searchTerm) ||
			                          c.Email.Contains(searchTerm));
		});
	}
}