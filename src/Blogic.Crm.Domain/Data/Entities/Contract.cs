using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Blogic.Crm.Domain.Data.Entities;

[Table("contracts")]
[Index(nameof(RegistrationNumber), IsUnique = true)]
public sealed class Contract : Entity
{
	public const int RegistrationNumberMaximumLength = 128;
	public const int InstitutionMaximumLength = 256;
	
	[Column("registration_number")]
	[Required]
	[MaxLength(RegistrationNumberMaximumLength)]
	public string RegistrationNumber { get; set; } = default!;

	[Column("institution")]
	[Required]
	[Unicode]
	[MaxLength(InstitutionMaximumLength)]
	public string Institution { get; set; } = default!;

	[Column("date_concluded")]
	[Required]
	public DateTime DateConcluded { get; set; }
	
	[Column("date_expired")]
	[Required]
	public DateTime DateExpired { get; set; }
	
	[Column("date_valid")]
	[Required]
	public DateTime DateValid { get; set; }
	
	[Column("client_id")]
	[Required]
	public long ClientId { get; set; }

	[Column("manager_id")] public long? ManagerId { get; set; } = default!;

	[NotMapped]
	[ForeignKey(nameof(ClientId))]
	[DeleteBehavior(DeleteBehavior.Cascade)]
	public Client Client { get; set; } = default!;
	
	[NotMapped]
	[ForeignKey(nameof(ManagerId))]
	[DeleteBehavior(DeleteBehavior.SetNull)]
	public Consultant Manager { get; set; } = default!;

	[NotMapped] public ICollection<ContractConsultant> Consultants { get; set; } = default!;
}