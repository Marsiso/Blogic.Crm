using System.Diagnostics;
using Blogic.Crm.Infrastructure.Pagination;

namespace Blogic.Crm.Infrastructure.Searching;

/// <summary>
///     Provides LINQ extensions related to the data set searching using the query string parameters.
/// </summary>
public static class SearchFunctions
{
	/// <summary>
	///     Searches the <see cref="Client" /> data set for the matches using the provided search string.
	/// </summary>
	/// <param name="clients">The <see cref="Client" /> data set to be searched through.</param>
	/// <param name="queryString">Query string to be used to search for search term matches.</param>
	/// <returns>The filtered <see cref="Client" /> data set.</returns>
	public static IQueryable<Client> Search(this IQueryable<Client> clients,
	                                        ClientQueryString queryString)
	{
		// When the provided search string is empty then do not search for the term occurrences.
		Debug.Assert(queryString != null);
		if (string.IsNullOrEmpty(queryString.SearchString))
		{
			return clients;
		}

		// Search for term matches in the provided client data set.
		var searchTerms = queryString.SearchString.Split(' ');
		return searchTerms.Aggregate(clients, (current, searchTerm) =>
		{
			return current.Where(c => c.BirthNumber.Contains(searchTerm) ||
			                          c.Phone.Contains(searchTerm) ||
			                          c.GivenName.Contains(searchTerm) ||
			                          c.FamilyName.Contains(searchTerm) ||
			                          c.Email.Contains(searchTerm));
		});
	}

	/// <summary>
	///     Searches the <see cref="Consultant" /> data set for the matches using the provided search string.
	/// </summary>
	/// <param name="consultants">The <see cref="Consultant" /> data set to be searched through.</param>
	/// <param name="queryString">Query string to be used to search for search term matches.</param>
	/// <returns>The filtered <see cref="Consultant" /> data set.</returns>
	public static IQueryable<Consultant> Search(this IQueryable<Consultant> consultants,
	                                            ConsultantQueryString queryString)
	{
		// When the provided search string is empty then do not search for the term occurrences.
		Debug.Assert(queryString != null);
		if (string.IsNullOrEmpty(queryString.SearchString))
		{
			return consultants;
		}

		// Search for term matches in the provided consultant data set.
		var searchTerms = queryString.SearchString.Split(' ');
		return searchTerms.Aggregate(consultants, (current, searchTerm) =>
		{
			return current.Where(c => c.BirthNumber.Contains(searchTerm) ||
			                          c.Phone.Contains(searchTerm) ||
			                          c.GivenName.Contains(searchTerm) ||
			                          c.FamilyName.Contains(searchTerm) ||
			                          c.Email.Contains(searchTerm));
		});
	}
}