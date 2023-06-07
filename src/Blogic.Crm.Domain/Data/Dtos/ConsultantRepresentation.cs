namespace Blogic.Crm.Domain.Data.Dtos;

/// <summary>
///     Data transfer object used to get client.
/// </summary>
public sealed record ConsultantRepresentation(long Id, string GivenName, string FamilyName, string Email,
    string NormalizedEmail, bool IsEmailConfirmed, string Phone,
    bool IsPhoneConfirmed, DateTime DateBorn, string BirthNumber);