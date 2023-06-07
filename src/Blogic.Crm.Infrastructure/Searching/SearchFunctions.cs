namespace Blogic.Crm.Infrastructure.Searching;

/// <summary>
///     Extension function for parameterized search of records in the database.
/// </summary>
public static class SearchFunctions
{
    /// <summary>
    ///     Function for parameterized search of clients in the database.
    /// </summary>
    public static IQueryable<Client> Search(this IQueryable<Client> clients,
        ClientQueryString queryString)
    {
        if (IsNotNullOrEmpty(queryString.Phone)) clients = clients.Where(c => c.Phone.Contains(queryString.Phone));

        if (IsNotNullOrEmpty(queryString.Email)) clients = clients.Where(c => c.Email.Contains(queryString.Email));

        if (IsNotNullOrEmpty(queryString.GivenName))
            clients = clients.Where(c => c.GivenName.Contains(queryString.GivenName));

        if (IsNotNullOrEmpty(queryString.FamilyName))
            clients = clients.Where(c => c.FamilyName.Contains(queryString.FamilyName));

        return clients;
    }

    /// <summary>
    ///     Function for parameterized search of consultants in the database.
    /// </summary>
    public static IQueryable<Consultant> Search(this IQueryable<Consultant> contracts,
        ConsultantQueryString queryString)
    {
        if (IsNotNullOrEmpty(queryString.Phone)) contracts = contracts.Where(c => c.Phone.Contains(queryString.Phone));

        if (IsNotNullOrEmpty(queryString.Email)) contracts = contracts.Where(c => c.Email.Contains(queryString.Email));

        if (IsNotNullOrEmpty(queryString.GivenName))
            contracts = contracts.Where(c => c.GivenName.Contains(queryString.GivenName));

        if (IsNotNullOrEmpty(queryString.FamilyName))
            contracts = contracts.Where(c => c.FamilyName.Contains(queryString.FamilyName));

        return contracts;
    }

    /// <summary>
    ///     Function for parameterized search of contracts in the database.
    /// </summary>
    public static IQueryable<Contract> Search(this IQueryable<Contract> contracts,
        ContractQueryString queryString)
    {
        if (IsNotNullOrEmpty(queryString.RegistrationNumber))
            contracts = contracts.Where(c => c.RegistrationNumber.Contains(queryString.RegistrationNumber));

        return IsNotNullOrEmpty(queryString.Institution)
            ? contracts.Where(c => c.Institution.Contains(queryString.Institution))
            : contracts;
    }
}