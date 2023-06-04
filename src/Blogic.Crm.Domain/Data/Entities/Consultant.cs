using System.ComponentModel.DataAnnotations.Schema;

namespace Blogic.Crm.Domain.Data.Entities;

/// <summary>
/// Persistence data model for consultant.
/// </summary>
[Table("consultants")]
public sealed class Consultant : User
{
}