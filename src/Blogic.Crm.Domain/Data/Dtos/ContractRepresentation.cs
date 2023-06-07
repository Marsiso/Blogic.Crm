#nullable disable

namespace Blogic.Crm.Domain.Data.Dtos;

/// <summary>
///     Data transfer object used to get contract.
/// </summary>
public sealed record ContractRepresentation(long Id, string RegistrationNumber, string Institution,
    DateTime DateConcluded, DateTime DateExpired, DateTime DateValid,
    long ClientId, long ManagerId);