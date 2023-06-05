#nullable disable

using Microsoft.AspNetCore.Mvc;

namespace Blogic.Crm.Domain.Data.Dtos;

public sealed class ContractInput
{
	public string RegistrationNumber { get; set; } = default!;
	public string Institution { get; set; } = default!;
	public DateTime DateConcluded { get; set; }
	public DateTime DateExpired { get; set; }
	public DateTime DateValid { get; set; }
	public long ClientId { get; set; }
	public long ManagerId { get; set; }
}