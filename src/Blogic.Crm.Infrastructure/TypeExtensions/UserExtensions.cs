namespace Blogic.Crm.Infrastructure.TypeExtensions;

public static class UserExtensions
{
    public static bool EmailNotTaken(string email, DataContext dataContext, IEmailLookupNormalizer normalizer)
    {
        var normalizedEmail = normalizer.Normalize(email)!;
        return !dataContext.Clients.AsNoTracking().Any(c => c.NormalizedEmail == normalizedEmail);
    }
    
    public static bool EmailNotTaken(Entity entity, string email, DataContext dataContext, IEmailLookupNormalizer normalizer)
    {
        var normalizedEmail = normalizer.Normalize(email)!;
        return !dataContext.Clients.AsNoTracking().Any(c => c.NormalizedEmail == normalizedEmail && c.Id != entity.Id);
    }

    public static bool PhoneNotTaken(string phone, DataContext dataContext, IPhoneLookupNormalizer normalizer)
    {
        var normalizedPhone = normalizer.Normalize(phone)!;
        return !dataContext.Clients.AsNoTracking().Any(c => c.Phone == normalizedPhone);
    }
    
    
    public static bool PhoneNotTaken(Entity entity, string phone, DataContext dataContext, IPhoneLookupNormalizer normalizer)
    {
        var normalizedPhone = normalizer.Normalize(phone)!;
        return !dataContext.Clients.AsNoTracking().Any(c => c.Phone == normalizedPhone && c.Id != entity.Id);
    }

    public static bool BirthNumberNotTaken(string birthNumber, DataContext dataContext)
    {
        return !dataContext.Clients.AsNoTracking().Any(c => c.BirthNumber == birthNumber);
    }
    
    public static bool BirthNumberNotTaken(Entity entity, string birthNumber, DataContext dataContext)
    {
        return !dataContext.Clients.AsNoTracking().Any(c => c.BirthNumber == birthNumber && c.Id != entity.Id);
    }
}