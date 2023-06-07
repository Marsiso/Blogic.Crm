namespace Blogic.Crm.Infrastructure.Sorting;

/// <summary>
///     Extension functions for sorting records in the database.
/// </summary>
public static class SortFunctions
{
    /// <summary>
    ///     Function for sorting client records by the selected property type..
    /// </summary>
    public static IOrderedQueryable<Client> Sort(this IQueryable<Client> clients,
        ClientQueryString queryString)
    {
        return queryString.SortOrder switch
        {
            ClientsSortOrder.Id => clients.OrderBy(c => c.Id),
            ClientsSortOrder.IdDesc => clients.OrderByDescending(c => c.Id),
            ClientsSortOrder.GivenName => clients.OrderBy(c => c.GivenName),
            ClientsSortOrder.GivenNameDesc => clients.OrderByDescending(c => c.GivenName),
            ClientsSortOrder.FamilyName => clients.OrderBy(c => c.FamilyName),
            ClientsSortOrder.FamilyNameDesc => clients.OrderByDescending(c => c.FamilyName),
            ClientsSortOrder.Email => clients.OrderBy(c => c.Email),
            ClientsSortOrder.EmailDesc => clients.OrderByDescending(c => c.Email),
            ClientsSortOrder.Phone => clients.OrderBy(c => c.Phone),
            ClientsSortOrder.PhoneDesc => clients.OrderByDescending(c => c.Phone),
            ClientsSortOrder.DateBorn => clients.OrderBy(c => c.DateBorn),
            ClientsSortOrder.DateBornDesc => clients.OrderByDescending(c => c.DateBorn),
            ClientsSortOrder.IsEmailConfirmed => clients.OrderBy(c => c.IsEmailConfirmed),
            ClientsSortOrder.IsEmailConfirmedDesc => clients.OrderByDescending(c => c.IsEmailConfirmed),
            ClientsSortOrder.IsPhoneConfirmed => clients.OrderBy(c => c.IsPhoneConfirmed),
            ClientsSortOrder.IsPhoneConfirmedDesc => clients.OrderByDescending(c => c.IsPhoneConfirmed),
            ClientsSortOrder.BirthNumber => clients.OrderBy(c => c.BirthNumber),
            ClientsSortOrder.BirthNumberDesc => clients.OrderByDescending(c => c.BirthNumber),
            _ => clients.OrderByDescending(c => c.Id)
        };
    }

    /// <summary>
    ///     Function for sorting consultant records by the selected property type.
    /// </summary>
    public static IOrderedQueryable<Consultant> Sort(this IQueryable<Consultant> consultants,
        ConsultantQueryString queryString)
    {
        return queryString.SortOrder switch
        {
            ConsultantSortOrder.Id => consultants.OrderBy(c => c.Id),
            ConsultantSortOrder.IdDesc => consultants.OrderByDescending(c => c.Id),
            ConsultantSortOrder.GivenName => consultants.OrderBy(c => c.GivenName),
            ConsultantSortOrder.GivenNameDesc => consultants.OrderByDescending(c => c.GivenName),
            ConsultantSortOrder.FamilyName => consultants.OrderBy(c => c.FamilyName),
            ConsultantSortOrder.FamilyNameDesc => consultants.OrderByDescending(c => c.FamilyName),
            ConsultantSortOrder.Email => consultants.OrderBy(c => c.Email),
            ConsultantSortOrder.EmailDesc => consultants.OrderByDescending(c => c.Email),
            ConsultantSortOrder.Phone => consultants.OrderBy(c => c.Phone),
            ConsultantSortOrder.PhoneDesc => consultants.OrderByDescending(c => c.Phone),
            ConsultantSortOrder.DateBorn => consultants.OrderBy(c => c.DateBorn),
            ConsultantSortOrder.DateBornDesc => consultants.OrderByDescending(c => c.DateBorn),
            ConsultantSortOrder.IsEmailConfirmed => consultants.OrderBy(c => c.IsEmailConfirmed),
            ConsultantSortOrder.IsEmailConfirmedDesc => consultants.OrderByDescending(c => c.IsEmailConfirmed),
            ConsultantSortOrder.IsPhoneConfirmed => consultants.OrderBy(c => c.IsPhoneConfirmed),
            ConsultantSortOrder.IsPhoneConfirmedDesc => consultants.OrderByDescending(c => c.IsPhoneConfirmed),
            ConsultantSortOrder.BirthNumber => consultants.OrderBy(c => c.BirthNumber),
            ConsultantSortOrder.BirthNumberDesc => consultants.OrderByDescending(c => c.BirthNumber),
            _ => consultants.OrderByDescending(c => c.Id)
        };
    }

    /// <summary>
    ///     Function for sorting contract records by the selected property type.
    /// </summary>
    public static IOrderedQueryable<Contract> Sort(this IQueryable<Contract> contracts,
        ContractQueryString queryString)
    {
        return queryString.SortOrder switch
        {
            ContractSortOrder.Id => contracts.OrderBy(c => c.Id),
            ContractSortOrder.IdDesc => contracts.OrderByDescending(c => c.Id),
            ContractSortOrder.RegistrationNumber => contracts.OrderBy(c => c.RegistrationNumber),
            ContractSortOrder.RegistrationNumberDesc => contracts.OrderByDescending(c => c.RegistrationNumber),
            ContractSortOrder.Institution => contracts.OrderBy(c => c.Institution),
            ContractSortOrder.InstitutionDesc => contracts.OrderByDescending(c => c.Institution),
            ContractSortOrder.DateConcluded => contracts.OrderBy(c => c.DateConcluded),
            ContractSortOrder.DateConcludedDesc => contracts.OrderByDescending(c => c.DateConcluded),
            ContractSortOrder.DateExpired => contracts.OrderBy(c => c.DateExpired),
            ContractSortOrder.DateExpiredDesc => contracts.OrderByDescending(c => c.DateExpired),
            ContractSortOrder.DateValid => contracts.OrderBy(c => c.DateValid),
            ContractSortOrder.DateValidDesc => contracts.OrderByDescending(c => c.DateValid),
            ContractSortOrder.ClientId => contracts.OrderBy(c => c.ClientId),
            ContractSortOrder.ClientIdDesc => contracts.OrderByDescending(c => c.ClientId),
            ContractSortOrder.ManagerId => contracts.OrderBy(c => c.ManagerId),
            ContractSortOrder.ManagerIdDesc => contracts.OrderByDescending(c => c.ManagerId),
            _ => contracts.OrderBy(c => c.Id)
        };
    }
}