namespace Blogic.Crm.Infrastructure.TypeExtensions;

public static class ClientExtensions
{
    public static bool ClientExists(long id, DataContext dataContext)
    {
        return dataContext.Clients
            .AsNoTracking()
            .Any(c => c.Id == id);
    }
}