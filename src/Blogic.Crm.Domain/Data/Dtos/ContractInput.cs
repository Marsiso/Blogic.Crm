#nullable disable
namespace Blogic.Crm.Domain.Data.Dtos;

/// <summary>
///     Data transfer object used to create or update contract.
/// </summary>
public sealed class ContractInput
{
    public ContractInput()
    {
    }

    public ContractInput(DateTime dateConcluded, DateTime dateValid, DateTime dateExpired)
    {
        DateConcluded = dateConcluded;
        DateValid = dateValid;
        DateExpired = dateExpired;
    }

    public string RegistrationNumber { get; set; } = default!;
    public string Institution { get; set; } = default!;
    public DateTime DateConcluded { get; set; }
    public DateTime DateExpired { get; set; }
    public DateTime DateValid { get; set; }
    public long ClientId { get; set; }
    public long ManagerId { get; set; }
    public string ConsultantIds { get; set; }
}