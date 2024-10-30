using AskGenAi.Core.Entities;

namespace AskGenAi.Core.Interfaces;

/// <summary>
/// Represents a repository that stores entities of type T
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IOnPremisesRepository<T> where T : IEntity
{
    /// <summary>
    /// Asynchronously retrieves all entities from the repository
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Asynchronously retrieves a single entity by its ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<T?> GetByIdAsync(Guid id);

    /// <summary>
    /// Asynchronously adds a new entity to the repository
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task AddAsync(T entity);

    /// <summary>
    /// Asynchronously updates an existing entity in the repository
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task UpdateAsync(T entity);

    /// <summary>
    /// Asynchronously deletes an entity from the repository by its ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Asynchronously saves any changes made to the repository  
    /// </summary>
    /// <returns></returns>
    Task SaveAsync();
}