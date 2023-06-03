#nullable disable

namespace Blogic.Crm.Domain.Data.Dtos;

public sealed record ClientInput
{
	public ClientInput()
	{
	}
	
	public ClientInput(string givenName, string familyName, string email, string phone, string birthNumber, string password)
	{
		GivenName = givenName;
		FamilyName = familyName;
		Email = email;
		Phone = phone;
		BirthNumber = birthNumber;
		Password = password;
	}

	public string GivenName { get; set; }
	public string FamilyName { get; set; }
	public string Email { get; set; }
	public string Phone { get; set; }
	public string BirthNumber { get; set; }
	public string Password { get; set; }
	public DateTime? DateBorn { get; set; }
}
