using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Blogic.Crm.Domain.Data.Entities;

/// <summary>
///     Persistence data model for user.
/// </summary>
[Index(nameof(NormalizedEmail), IsUnique = true)]
[Index(nameof(Phone), IsUnique = true)]
[Index(nameof(BirthNumber), IsUnique = true)]
public class User : Entity
{
	public const int GivenNameMaximumLength = 256;
	public const int FamilyNameMaximumLength = 256;
	public const int EmailMaximumLength = 256;
	public const int PhoneMaximumLength = 64;
	public const int BirthNumberMaximumLength = 64;
	public const int SecurityStampMaximumLength = 64;
	public const int AgeMinimumValue = 18;
	public const int RequiredSpecialCharacters = 1;
	public const int RequiredDigitCharacters = 1;
	public const int RequiredLowerCaseCharacters = 1;
	public const int RequiredUpperCaseCharacters = 1;
	public static DateTime MinimalDateBorn = new DateTime(1800,1,1);

	[Column("given_name")]
	[Required]
	[Unicode]
	[MaxLength(GivenNameMaximumLength)]
	public string GivenName { get; set; } = default!;

	[Column("family_name")]
	[Required]
	[Unicode]
	[MaxLength(FamilyNameMaximumLength)]
	public string FamilyName { get; set; } = default!;

	[Column("email")]
	[Required]
	[MaxLength(EmailMaximumLength)]
	public string Email { get; set; } = default!;

	[Column("normalized_email")]
	[Required]
	[MaxLength(EmailMaximumLength)]
	public string NormalizedEmail { get; set; } = default!;

	[Column("is_email_confirmed")]
	[Required]
	public bool IsEmailConfirmed { get; set; } = true;

	[Column("phone")]
	[Required]
	[MaxLength(PhoneMaximumLength)]
	public string Phone { get; set; } = default!;

	[Column("is_phone_confirmed")]
	[Required]
	public bool IsPhoneConfirmed { get; set; } = true;

	[Column("date_born")] [Required] public DateTime DateBorn { get; set; }

	[Column("birth_number")]
	[Required]
	[MaxLength(BirthNumberMaximumLength)]
	public string BirthNumber { get; set; } = default!;

	[Column("password_hash")] [Required] public string PasswordHash { get; set; } = default!;

	[Column("concurrency_stamp")]
	[Required]
	[Timestamp]
	[ConcurrencyCheck]
	public byte[] ConcurrencyStamp { get; set; } = default!;

	[Column("security_stamp")]
	[Required]
	[MaxLength(SecurityStampMaximumLength)]
	public string SecurityStamp { get; set; } = default!;
}