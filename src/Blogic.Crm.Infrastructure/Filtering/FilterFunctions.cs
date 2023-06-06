using Blogic.Crm.Infrastructure.Pagination;

namespace Blogic.Crm.Infrastructure.Filtering;

/// <summary>
///     Provides LINQ extensions related to the data set filtering using the query string parameters.
/// </summary>
public static class FilterFunctions
{
	/// <summary>
	///     Filters the <see cref="Client" /> data set using the <see cref="ClientQueryString" /> query string
	///     parameters.
	/// </summary>
	/// <param name="clients">The <see cref="Client" /> data set to be filtered.</param>
	/// <param name="queryString">Provided query string used to filter the <see cref="Client" /> data set.</param>
	/// <returns>The filtered <see cref="Client" /> data set.</returns>
	public static IQueryable<Client> Filter(this IQueryable<Client> clients,
	                                        ClientQueryString queryString)
	{
		return clients.Where(c => c.DateBorn > queryString.MinDateBorn &&
		                          c.DateBorn < queryString.MaxDateBorn &&
		                          c.IsEmailConfirmed == queryString.IsEmailConfirmed &&
		                          c.IsPhoneConfirmed == queryString.IsPhoneConfirmed &&
		                          c.DateBorn >= queryString.MinDateBorn &&
		                          c.DateBorn <= queryString.MaxDateBorn &&
		                          c.Id >= queryString.MinId &&
		                          c.Id <= queryString.MaxId);
	}

	/// <summary>
	///     Filters the <see cref="Consultant" /> data set using the <see cref="ConsultantQueryString" /> query string
	///     parameters.
	/// </summary>
	/// <param name="consultants">The <see cref="Consultant" /> data set to be filtered.</param>
	/// <param name="queryString">Provided query string used to filter the <see cref="Consultant" /> data set.</param>
	/// <returns>The filtered <see cref="Consultant" /> data set.</returns>
	public static IQueryable<Consultant> Filter(this IQueryable<Consultant> consultants,
	                                            ConsultantQueryString queryString)
	{
		return consultants;
	}

	/// <summary>
	///     Filters the <see cref="Contract" /> data set using the <see cref="ContractQueryString" /> query string
	///     parameters.
	/// </summary>
	/// <param name="contracts">The <see cref="Contract" /> data set to be filtered.</param>
	/// <param name="queryString">Provided query string used to filter the <see cref="Contract" /> data set.</param>
	/// <returns>The filtered <see cref="Contract" /> data set.</returns>
	public static IQueryable<Contract> Filter(this IQueryable<Contract> contracts,
	                                          ContractQueryString queryString)
	{
		return contracts.Where(c => c.DateConcluded >= queryString.MinDateConcluded &&
		                            c.DateConcluded <= queryString.MaxDateConcluded &&
		                            c.DateValid >= queryString.MinDateValid &&
		                            c.DateValid <= queryString.MaxDateValid &&
		                            c.DateValid >= queryString.MinDateExpired &&
		                            c.DateValid <= queryString.MaxDateExpired &&
		                            c.ClientId >= queryString.MinClientId &&
		                            c.ClientId <= queryString.MaxClientId &&
		                            c.ManagerId >= queryString.MinManagerId &&
		                            c.ManagerId <= queryString.MaxManagerId);
	}
}