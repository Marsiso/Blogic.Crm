namespace Blogic.Crm.Domain.Data.Dtos;

public sealed record ClientRepresentation(long Id, string GivenName, string FamilyName, string Email,
                                          string NormalizedEmail, bool IsEmailConfirmed, string Phone,
                                          bool IsPhoneConfirmed, DateTime DateBorn, string BirthNumber);