namespace Blogic.Crm.Infrastructure.TypeExtensions;

public static class ContractExtensions
{
    public static bool RegistrationNumberNotTaken(Entity entity, string registrationNumber, DataContext dataContext)
    {
        return !dataContext.Contracts
            .AsNoTracking()
            .Any(c => c.Id != entity.Id && c.RegistrationNumber == registrationNumber);
    }
}