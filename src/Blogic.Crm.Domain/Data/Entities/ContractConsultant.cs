using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Blogic.Crm.Domain.Data.Entities;

/// <summary>
/// Persistence data model for contract and consultant relationship.
/// </summary>
[Table("contract_consultants")]
[PrimaryKey(nameof(ContractId), nameof(ConsultantId))]
public sealed class ContractConsultant
{
	[Column] [Required] public long ContractId { get; set; }
	[Column] [Required] public long ConsultantId { get; set; }

	[NotMapped]
	[ForeignKey(nameof(ContractId))]
	[DeleteBehavior(DeleteBehavior.Cascade)]
	public Contract Contract { get; set; } = default!;
	
	[NotMapped]
	[ForeignKey(nameof(ConsultantId))]
	[DeleteBehavior(DeleteBehavior.Cascade)]
	public Contract Consultant { get; set; } = default!;

	[NotMapped] public ICollection<Contract> ManagedContracts { get; set; } = default!;

	[NotMapped] public ICollection<ContractConsultant> ConsultedContracts { get; set; } = default!;
}