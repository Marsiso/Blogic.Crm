namespace Blogic.Crm.Domain.Data.Dtos;

/// <summary>
///     Data transfer object used to present clients.
/// </summary>
/// <param name="Id"></param>
/// <param name="GivenName"></param>
/// <param name="FamilyName"></param>
/// <param name="Email"></param>
/// <param name="NormalizedEmail"></param>
/// <param name="IsEmailConfirmed"></param>
/// <param name="Phone"></param>
/// <param name="IsPhoneConfirmed"></param>
/// <param name="DateBorn"></param>
/// <param name="BirthNumber"></param>
public sealed record ConsultantRepresentation(long Id, string GivenName, string FamilyName, string Email,
                                              string NormalizedEmail, bool IsEmailConfirmed, string Phone,
                                              bool IsPhoneConfirmed, DateTime DateBorn, string BirthNumber);