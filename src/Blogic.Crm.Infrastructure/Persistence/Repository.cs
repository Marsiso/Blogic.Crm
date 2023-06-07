using System.Diagnostics;
using System.Linq.Expressions;

namespace Blogic.Crm.Infrastructure.Persistence;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
{
    protected readonly DataContext DataContext;

    private bool _disposed;

    public Repository(DataContext dataContext)
    {
        DataContext = dataContext;
        Entities = dataContext.Set<TEntity>();
    }

    public IQueryable<TEntity> Entities { get; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public IQueryable<TEntity> FindAll(bool trackChanges)
    {
        return trackChanges
            ? DataContext
                .Set<TEntity>()
                .AsTracking()
            : DataContext
                .Set<TEntity>()
                .AsNoTracking();
    }

    public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges)
    {
        return trackChanges
            ? DataContext
                .Set<TEntity>()
                .AsTracking()
                .Where(expression)
            : DataContext
                .Set<TEntity>()
                .AsNoTracking()
                .Where(expression);
    }

    public async Task<TEntity?> FindByIdentifier(long entityId, bool trackChanges)
    {
        Debug.Assert(entityId > 0);

        if (trackChanges)
            return await DataContext
                .Set<TEntity>()
                .AsTracking()
                .SingleOrDefaultAsync(entity => entity.Id == entityId);

        return await DataContext
            .Set<TEntity>()
            .AsNoTracking()
            .SingleOrDefaultAsync(entity => entity.Id == entityId);
    }

    public void Create(TEntity entity)
    {
        Debug.Assert(entity is not null);
        DataContext.Set<TEntity>().Add(entity);
    }

    public async Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Debug.Assert(entity is not null);
        await DataContext.Set<TEntity>().AddAsync(entity, cancellationToken);
    }

    public void Update(TEntity entity)
    {
        Debug.Assert(entity is { Id: > 0 });
        DataContext.Set<TEntity>().Update(entity);
    }

    public void Delete(TEntity entity)
    {
        Debug.Assert(entity is { Id: > 0 });
        DataContext.Set<TEntity>().Remove(entity);
    }

    public void SaveChanges()
    {
        DataContext.SaveChanges();
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return DataContext.SaveChangesAsync(cancellationToken);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed is false && disposing) DataContext.Dispose();
        _disposed = true;
    }
}