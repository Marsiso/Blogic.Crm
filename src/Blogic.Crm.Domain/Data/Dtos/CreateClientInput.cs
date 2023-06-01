namespace Blogic.Crm.Domain.Data.Dtos;

public sealed record CreateClientInput(string Email, string Password, string GivenName, string FamilyName,
                                       string Phone, DateTime DateBorn, string BirthNumber);