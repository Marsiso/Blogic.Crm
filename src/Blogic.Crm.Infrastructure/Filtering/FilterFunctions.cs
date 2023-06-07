namespace Blogic.Crm.Infrastructure.Filtering;

/// <summary>
///     Extension functions for filtering records in the database.
/// </summary>
public static class FilterFunctions
{
	/// <summary>
	///     Filters the <see cref="Client" /> data set using the <see cref="ClientQueryString" /> query string
	///     parameters.
	/// </summary>
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
	public static IQueryable<Consultant> Filter(this IQueryable<Consultant> consultants,
        ConsultantQueryString queryString)
    {
	    return consultants.Where(c => c.DateBorn > queryString.MinDateBorn &&
	                                  c.DateBorn < queryString.MaxDateBorn &&
	                                  c.IsEmailConfirmed == queryString.IsEmailConfirmed &&
	                                  c.IsPhoneConfirmed == queryString.IsPhoneConfirmed &&
	                                  c.DateBorn >= queryString.MinDateBorn &&
	                                  c.DateBorn <= queryString.MaxDateBorn &&
	                                  c.Id >= queryString.MinId &&
	                                  c.Id <= queryString.MaxId);
    }

	/// <summary>
	///     Filters the <see cref="Contract" /> data set using the <see cref="ContractQueryString" /> query string
	///     parameters.
	/// </summary>
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