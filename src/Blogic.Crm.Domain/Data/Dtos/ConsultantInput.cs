#nullable disable

namespace Blogic.Crm.Domain.Data.Dtos;

/// <summary>
///     Data transfer object used to create or update consultant.
/// </summary>
public sealed class ConsultantInput
{
    public ConsultantInput()
    {
    }

    public ConsultantInput(string givenName, string familyName, string email, string phone, string birthNumber,
        string password)
    {
        GivenName = givenName;
        FamilyName = familyName;
        Email = email;
        Phone = phone;
        BirthNumber = birthNumber;
        Password = password;
    }

    public ConsultantInput(DateTime dateBorn) : this()
    {
        DateBorn = dateBorn;
    }

    public string GivenName { get; set; }
    public string FamilyName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string BirthNumber { get; set; }
    public string Password { get; set; }
    public DateTime DateBorn { get; set; }
}