#nullable disable

using CsvHelper.Configuration.Attributes;

namespace Blogic.Crm.Domain.Data.Dtos;

/// <summary>
///     Data transfer object used to export contracts.
/// </summary>
public sealed class ContractToExport
{
    [Name("Identifier")] [Index(1)] public long Id { get; set; }

    [Name("Registration number")]
    [Index(2)]
    public string RegistrationNumber { get; set; }

    [Name("Institution")] [Index(3)] public string Institution { get; set; }

    [Name("Conclusion date")] [Index(4)] public DateTime DateConcluded { get; set; }

    [Name("Expiration date")] [Index(5)] public DateTime DateExpired { get; set; }

    [Name("Validity date")] [Index(6)] public DateTime DateValid { get; set; }

    [Name("Client identifier")] [Index(7)] public long ClientId { get; set; }

    [Name("Manager identifier")]
    [Index(8)]
    public long ManagerId { get; set; }
}