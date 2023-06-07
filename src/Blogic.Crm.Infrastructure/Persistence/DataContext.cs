#nullable disable

namespace Blogic.Crm.Infrastructure.Persistence;

/// <summary>
///     Database session of the application that allows to perform operations on the database and its entities.
/// </summary>
public sealed class DataContext : DbContext
{
    private const string Schema = "dbo";

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    /// <summary>
    ///     Client records.
    /// </summary>
    public DbSet<Client> Clients { get; set; }

    /// <summary>
    ///     Consultant records.
    /// </summary>
    public DbSet<Consultant> Consultants { get; set; }

    /// <summary>
    ///     Contract records.
    /// </summary>
    public DbSet<Contract> Contracts { get; set; }

    /// <summary>
    ///     Contract consultant records.
    /// </summary>
    public DbSet<ContractConsultant> ContractConsultants { get; set; }

    /// <summary>
    ///     Configures the database when it is created.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        base.OnModelCreating(modelBuilder);
    }
}