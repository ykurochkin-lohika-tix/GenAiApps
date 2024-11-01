using AskGenAi.Core.Interfaces;
using AskGenAi.Core.Models;

namespace AskGenAi.Infrastructure.Persistence;

// Represents a repository that stores entities of type T
// This implementation stores entities in memory
public class InMemoryRepository<T> : IOnPremisesRepository<T> where T : IEntity
{
    private readonly List<T> _entities = [];

    // </inheritdoc>
    public Task<IEnumerable<T>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<T>>(_entities);
    }

    // </inheritdoc>
    public Task<T?> GetByIdAsync(Guid id)
    {
        var entity = _entities.SingleOrDefault(e => e.Id == id);

        return Task.FromResult(entity);
    }

    // </inheritdoc>
    public Task AddAsync(T entity)
    {
        _entities.Add(entity);

        return Task.CompletedTask;
    }

    // </inheritdoc>
    public Task UpdateAsync(T entity)
    {
        var existingEntity = _entities.SingleOrDefault(e => e.Id == entity.Id);
        if (existingEntity != null)
        {
            _entities.Remove(existingEntity);
            _entities.Add(entity);
        }

        return Task.CompletedTask;
    }

    // </inheritdoc>
    public Task DeleteAsync(Guid id)
    {
        var entity = _entities.SingleOrDefault(e => e.Id == id);
        if (entity != null)
        {
            _entities.Remove(entity);
        }

        return Task.CompletedTask;
    }

    // </inheritdoc>
    public Task SaveAsync()
    {
        return Task.CompletedTask;
    }
}