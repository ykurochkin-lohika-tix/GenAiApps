using AskGenAi.Common.Services;
using AskGenAi.Core.Entities;
using AskGenAi.Core.Interfaces;

namespace AskGenAi.Infrastructure.Persistence;

// Represents a repository that stores entities of type T
// This implementation stores entities in a JSON file
public class FileRepository<T> : IRepository<T> where T : IEntity
{
    // it is used like composition for the file repository
    private readonly IJsonFileSerializer<T> _jsonFileSerializer = new JsonFileSerializer<T>();

    private readonly string _filePath;
    private readonly List<T> _entities;
    private readonly string _version;
    
    public FileRepository(string filePath)
    {
        _filePath = filePath;
        _entities = new List<T>();
        _version = string.Empty;

        if (!File.Exists(_filePath))
        {
            Console.WriteLine($"File not found: {_filePath}");
            return;
        }

        var root = _jsonFileSerializer.Deserialize(_filePath);
        _entities = root?.Data ?? [];
        _version = root?.Version ?? string.Empty;
    }

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
    public async Task AddAsync(T entity)
    {
        _entities.Add(entity);
        await SaveToFileAsync();
    }

    // </inheritdoc>
    public async Task UpdateAsync(T entity)
    {
        var existingEntity = _entities.SingleOrDefault(e => e.Id == entity.Id);
        if (existingEntity != null)
        {
            _entities.Remove(existingEntity);
            _entities.Add(entity);
            await SaveToFileAsync();
        }
    }

    // </inheritdoc>
    public async Task DeleteAsync(Guid id)
    {
        var entity = _entities.SingleOrDefault(e => e.Id == id);
        if (entity != null)
        {
            _entities.Remove(entity);
            await SaveToFileAsync();
        }
    }

    // </inheritdoc>
    public Task SaveAsync()
    {
        return SaveToFileAsync();
    }

    // Helper method to save the current entities to the JSON file
    private async Task SaveToFileAsync()
    {
        var normalizeEntitiesFull = new Root<T>
        {
            Data = _entities,
            Version = _version
        };

        _ = await _jsonFileSerializer.SerializeAsync(normalizeEntitiesFull, _filePath);
    }
}