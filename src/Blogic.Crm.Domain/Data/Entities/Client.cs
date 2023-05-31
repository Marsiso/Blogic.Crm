using System.ComponentModel.DataAnnotations.Schema;

namespace Blogic.Crm.Domain.Data.Entities;

[Table("clients")]
public sealed class Client : User
{
}