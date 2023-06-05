using Microsoft.EntityFrameworkCore;

namespace Blogic.Crm.Infrastructure.Persistence;

public sealed class DataContext : DbContext
{
	public const string Schema = "dbo";

	public DataContext(DbContextOptions<DataContext> options) : base(options) { }

	public DbSet<Client> Clients { get; set; } = default!;
	public DbSet<Consultant> Consultants { get; set; } = default!;
	public DbSet<Contract> Contracts { get; set; } = default!;
	public DbSet<ContractConsultant> ContractConsultants { get; set; } = default!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema(Schema);
		base.OnModelCreating(modelBuilder);
	}
}