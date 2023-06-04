namespace Blogic.Crm.Infrastructure.Sorting;

/// <summary>
/// Client's properties that are viable for the client data set sorting options.
/// </summary>
public enum ClientsSortOrder
{
	GivenName,
	GivenNameDesc,
	FamilyName,
	FamilyNameDesc,
	Id,
	IdDesc,
	Email,
	EmailDesc,
	Phone,
	PhoneDesc,
	DateBorn,
	DateBornDesc,
	IsEmailConfirmed,
	IsEmailConfirmedDesc,
	IsPhoneConfirmed,
	IsPhoneConfirmedDesc,
	BirthNumber,
	BirthNumberDesc
}