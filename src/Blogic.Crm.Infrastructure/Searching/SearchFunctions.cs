using System.Diagnostics;
using Blogic.Crm.Domain.Data.Entities;
using Blogic.Crm.Infrastructure.Pagination;

namespace Blogic.Crm.Infrastructure.Searching;

public static class SearchFunctions
{
	public static IQueryable<Client> SearchClients(this IQueryable<Client> clients,
	                                               ClientQueryStringParameters queryStringParameters)
	{
		Debug.Assert(queryStringParameters != null);
		if (string.IsNullOrEmpty(queryStringParameters.SearchString))
		{
			return clients;
		}

		string[] searchTerms = queryStringParameters.SearchString.Split(' ');
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