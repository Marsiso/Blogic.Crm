#nullable disable

using CsvHelper.Configuration.Attributes;

namespace Blogic.Crm.Domain.Data.Dtos;

/// <summary>
///     Data transfer object used to export clients.
/// </summary>
public sealed class ClientToExport
{
    [Name("Identifier")] [Index(1)] public long Id { get; set; }

    [Name("Given name")] [Index(2)] public string GivenName { get; set; }

    [Name("Family name")] [Index(3)] public string FamilyName { set; get; }

    [Name("Email address")] [Index(4)] public string Email { get; set; }

    [Name("Normalized email address")]
    [Index(5)]
    public string NormalizedEmail { get; set; }

    [Name("Cconfirmed")] [Index(6)] public bool IsEmailConfirmed { get; set; }

    [Name("Phone number")] [Index(7)] public string Phone { get; set; }

    [Name("Confirmed")] [Index(8)] public bool IsPhoneConfirmed { get; set; }

    [Name("Date born")]
    [Index(9)]
    [Format("dd/MM/yyyy")]
    public DateTime DateBorn { get; set; }

    [Name("Birth number")] [Index(10)] public string BirthNumber { get; set; }
}