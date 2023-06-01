using System.Linq.Expressions;
using Blogic.Crm.Domain.Data.Entities;

namespace Blogic.Crm.Infrastructure.Data;

public interface IRepository<TEntity> : IDisposable where TEntity : Entity
{
	IQueryable<TEntity> Entities { get; }
	IQueryable<TEntity> FindAll(bool trackChanges);
	IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges);
	Task<TEntity?> FindById(long entityId, bool trackChanges);
	void Create(TEntity entity);
	Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
	void Update(TEntity entity);
	void Delete(TEntity entity);
	void SaveChanges();
	Task SaveChangesAsync(CancellationToken cancellationToken = default);
}