using Blogic.Crm.Domain.Data.Entities;
using Blogic.Crm.Infrastructure.Pagination;
using static Blogic.Crm.Infrastructure.Sorting.ClientsSortOrder;

namespace Blogic.Crm.Infrastructure.Sorting;

/// <summary>
/// Provides LINQ extensions related to the data set ordering using the query string parameters.
/// </summary>
public static class SortOrderFunctions
{
	/// <summary>
	/// Orders the <see cref="Client"/> data set using the <see cref="ClientQueryString"/> query string parameters.
	/// </summary>
	/// <param name="clients">The <see cref="Client"/> data set to be ordered.</param>
	/// <param name="queryString">Provided query string used to order the <see cref="Client"/> data set.</param>
	/// <returns>The ordered <see cref="Client"/> data set.</returns>
	public static IOrderedQueryable<Client> OrderClients(this IQueryable<Client> clients,
	                                                     ClientQueryString queryString)
	{
		return queryString.SortOrder switch
		{
			Id => clients.OrderBy(c => c.Id),
			IdDesc => clients.OrderByDescending(c => c.Id),
			GivenName => clients.OrderBy(c => c.GivenName),
			GivenNameDesc => clients.OrderByDescending(c => c.GivenName),
			FamilyName => clients.OrderBy(c => c.FamilyName),
			FamilyNameDesc => clients.OrderByDescending(c => c.FamilyName),
			Email => clients.OrderBy(c => c.Email),
			EmailDesc => clients.OrderByDescending(c => c.Email),
			Phone => clients.OrderBy(c => c.Phone),
			PhoneDesc => clients.OrderByDescending(c => c.Phone),
			DateBorn => clients.OrderBy(c => c.DateBorn),
			DateBornDesc => clients.OrderByDescending(c => c.DateBorn),
			IsEmailConfirmed => clients.OrderBy(c => c.IsEmailConfirmed),
			IsEmailConfirmedDesc => clients.OrderByDescending(c => c.IsEmailConfirmed),
			IsPhoneConfirmed => clients.OrderBy(c => c.IsPhoneConfirmed),
			IsPhoneConfirmedDesc => clients.OrderByDescending(c => c.IsPhoneConfirmed),
			BirthNumber => clients.OrderBy(c => c.BirthNumber),
			BirthNumberDesc => clients.OrderByDescending(c => c.BirthNumber),
			_ => clients.OrderByDescending(c => c.FamilyName)
		};
	}
}