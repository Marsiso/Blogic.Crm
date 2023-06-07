using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blogic.Crm.Domain.Data.Entities;

/// <summary>
///     Persistence model base.
/// </summary>
public class Entity
{
	public Entity()
	{
	}

	public Entity(long id)
	{
		Id = id;
	}

	/// <summary>
	///     Unique identifier used to distinct entities.
	/// </summary>
	[Column("id")]
    [Required]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
}