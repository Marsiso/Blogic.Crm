using System.ComponentModel.DataAnnotations.Schema;

namespace Blogic.Crm.Domain.Data.Entities;

[Table("consultants")]
public sealed class Consultant : User
{
}