using AskGenAi.Core.Entities;
using AskGenAi.Core.Interfaces;
using AskGenAi.Infrastructure.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AskGenAi.Infrastructure.Persistence;

public class Repository<T>(AppDbContext context) : IRepository<T> where T : class, IEntityRoot
{
    protected readonly DbSet<T> DbSet = context.Set<T>();

    public IUnitOfWork UnitOfWork => context;

    public void Add(T entity)
    {
        DbSet.Add(entity);
    }

    public async ValueTask AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
    }

    public async Task AddRangeAsync(CancellationToken cancellationToken = default, params T[] entities)
    {
        await DbSet.AddRangeAsync(entities, cancellationToken);
    }

    public void Update(T entity)
    {
        DbSet.Update(entity);
    }

    public void Remove(T entity)
    {
        DbSet.Remove(entity);
    }

    public async Task RemoveByIdAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            DbSet.Remove(entity);
        }
    }

    public async Task<T?> GetByIdAsync(params object?[]? keyValues)
    {
        return await DbSet.FindAsync(keyValues);
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        return await DbSet.CountAsync(predicate, cancellationToken);
    }

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        return await DbSet.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate,
        CancellationToken cancellationToken = default)
    {
        return predicate is null
            ? await DbSet.ToListAsync(cancellationToken)
            : await DbSet.Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAllNoTrackAsync(Expression<Func<T, bool>>? predicate,
        CancellationToken cancellationToken = default)
    {
        return predicate is null
            ? await DbSet.AsNoTracking().ToListAsync(cancellationToken)
            : await DbSet.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TResult>> GetAllProjectedAsync<TResult>(
        Expression<Func<T, TResult>> selector,
        CancellationToken cancellationToken = default)
    {
        return await DbSet.Select(selector).ToListAsync(cancellationToken);
    }
}