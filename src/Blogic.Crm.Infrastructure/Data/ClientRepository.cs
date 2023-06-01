using System.Diagnostics;
using Blogic.Crm.Domain.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blogic.Crm.Infrastructure.Data;

public sealed class ClientRepository : Repository<Client>
{
	public ClientRepository(DataContext dataContext) : base(dataContext) { }

	public Task<Client?> FindByEmail(string normalizedEmail, bool trackChanges,
	                               CancellationToken cancellationToken = default)
	{
		Debug.Assert(!string.IsNullOrEmpty(normalizedEmail));
		return FindByCondition(c => c.NormalizedEmail == normalizedEmail, trackChanges)
			.SingleOrDefaultAsync(cancellationToken);
	}
}