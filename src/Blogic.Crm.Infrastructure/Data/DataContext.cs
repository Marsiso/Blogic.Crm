using Blogic.Crm.Domain.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blogic.Crm.Infrastructure.Data;

public sealed class DataContext : DbContext
{
	public DataContext(DbContextOptions<DataContext> options) : base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema(Schema);
		base.OnModelCreating(modelBuilder);
	}

	public const string Schema = "dbo";
	public DbSet<Client> Clients { get; set; } = default!;
	public DbSet<Consultant> Consultants { get; set; } = default!;
	public DbSet<Contract> Contracts { get; set; } = default!;
	public DbSet<ContractConsultant> ContractConsultants { get; set; } = default!;
}