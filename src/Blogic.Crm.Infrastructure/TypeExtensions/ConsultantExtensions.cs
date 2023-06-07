namespace Blogic.Crm.Infrastructure.TypeExtensions;

public static class ConsultantExtensions
{
    public static bool ConsultantExists(Entity entity, DataContext dataContext)
    {
        return dataContext.Consultants
            .AsNoTracking()
            .Any(c => c.Id == entity.Id);
    }

    public static bool ConsultantsExist(string idsAsString, DataContext dataContext)
    {
        var identifiers = ParseNumbersToInt64(idsAsString);
        var consultants = dataContext.Consultants.AsNoTracking();
        return identifiers.All(id => consultants.Any(c => c.Id == id));
    }
}