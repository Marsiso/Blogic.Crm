using System.Diagnostics;
using Blogic.Crm.Domain.Data.Entities;
using Blogic.Crm.Infrastructure.Pagination;

namespace Blogic.Crm.Infrastructure.Searching;

public static class SearchFunctions
{
	public static IQueryable<Client> SearchClients(this IQueryable<Client> clients,
	                                               ClientQueryString queryString)
	{
		Debug.Assert(queryString != null);
		if (string.IsNullOrEmpty(queryString.SearchString))
		{
			return clients;
		}

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