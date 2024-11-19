using System.Linq.Expressions;
using AskGenAi.Core.Entities;

namespace AskGenAi.Core.Interfaces;

public interface IRepository<T> where T : class, IEntityRoot
{
    void Add(T entity);
    ValueTask AddAsync(T entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(CancellationToken cancellationToken = default, params T[] entities);
    void Update(T entity);
    void Remove(T entity);
    Task RemoveByIdAsync(Guid id);
    Task<T?> GetByIdAsync(params object?[]? keyValues);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllNoTrackAsync(Expression<Func<T, bool>>? predicate, CancellationToken cancellationToken = default);
    Task<IEnumerable<TResult>> GetAllProjectedAsync<TResult>(Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default);

    IUnitOfWork UnitOfWork { get; }
}