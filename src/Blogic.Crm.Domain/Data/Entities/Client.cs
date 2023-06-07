using System.ComponentModel.DataAnnotations.Schema;

namespace Blogic.Crm.Domain.Data.Entities;

/// <summary>
///     Persistence data model for client.
/// </summary>
[Table("clients")]
public sealed class Client : User, ICloneable
{
    public object Clone()
    {
        return MemberwiseClone();
    }
}