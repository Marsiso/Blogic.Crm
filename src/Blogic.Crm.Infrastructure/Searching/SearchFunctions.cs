using Blogic.Crm.Infrastructure.Pagination;
using static Blogic.Crm.Infrastructure.TypeExtensions.StringExtensions;

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
		if (IsNotNullOrEmpty(queryString.Phone))
		{
			clients = clients.Where(c => c.Phone.Contains(queryString.Phone));
		}
		
		if (IsNotNullOrEmpty(queryString.Email))
		{
			clients = clients.Where(c => c.Email.Contains(queryString.Email));
		}
		
		if (IsNotNullOrEmpty(queryString.GivenName))
		{
			clients = clients.Where(c => c.GivenName.Contains(queryString.GivenName));
		}
		
		if (IsNotNullOrEmpty(queryString.FamilyName))
		{
			clients = clients.Where(c => c.FamilyName.Contains(queryString.FamilyName));
		}

		return clients;
	}

	/// <summary>
	///     Searches the <see cref="Consultant" /> data set for the matches using the provided search string.
	/// </summary>
	/// <param name="contracts">The <see cref="Consultant" /> data set to be searched through.</param>
	/// <param name="queryString">Query string to be used to search for search term matches.</param>
	/// <returns>The filtered <see cref="Consultant" /> data set.</returns>
	public static IQueryable<Consultant> Search(this IQueryable<Consultant> contracts,
	                                            ConsultantQueryString queryString)
	{
		if (IsNotNullOrEmpty(queryString.Phone))
		{
			contracts = contracts.Where(c => c.Phone.Contains(queryString.Phone));
		}
		
		if (IsNotNullOrEmpty(queryString.Email))
		{
			contracts = contracts.Where(c => c.Email.Contains(queryString.Email));
		}
		
		if (IsNotNullOrEmpty(queryString.GivenName))
		{
			contracts = contracts.Where(c => c.GivenName.Contains(queryString.GivenName));
		}
		
		if (IsNotNullOrEmpty(queryString.FamilyName))
		{
			contracts = contracts.Where(c => c.FamilyName.Contains(queryString.FamilyName));
		}

		return contracts;
	}

	/// <summary>
	///     Searches the <see cref="Contract" /> data set for the matches using the provided search string.
	/// </summary>
	/// <param name="contracts">The <see cref="Contract" /> data set to be searched through.</param>
	/// <param name="queryString">Query string to be used to search for search term matches.</param>
	/// <returns>The filtered <see cref="Contract" /> data set.</returns>
	public static IQueryable<Contract> Search(this IQueryable<Contract> contracts,
	                                          ContractQueryString queryString)
	{
		if (IsNotNullOrEmpty(queryString.RegistrationNumber))
		{
			contracts = contracts.Where(c => c.RegistrationNumber.Contains(queryString.RegistrationNumber));
		}

		return IsNotNullOrEmpty(queryString.Institution)
			? contracts.Where(c => c.Institution.Contains(queryString.Institution))
			: contracts;
	}
}