using AskGenAi.Core.Aggregators;
using AskGenAi.Core.Models;

namespace AskGenAi.Core.Interfaces;

public interface IJsonFileSerializer<T> where T : IEntity
{
    /// <summary>
    /// Deserializes the JSON file (async)
    /// </summary>
    /// <param name="normalizeFilePath"></param>
    /// <returns></returns>
    Task<Root<T>?> DeserializeAsync(string normalizeFilePath);

    /// <summary>
    /// Serializes the normalized data to a JSON file (async)
    /// </summary>
    /// <param name="normalized"></param>
    /// <param name="normalizedFilePath"></param>
    /// <returns></returns>
    Task<string> SerializeAsync(Root<T> normalized, string normalizedFilePath);

    /// <summary>
    /// Deserializes the JSON file
    /// </summary>
    /// <param name="normalizeFilePath"></param>
    /// <returns></returns>
    Root<T>? Deserialize(string normalizeFilePath);

    /// <summary>
    /// Serializes the normalized data to a JSON file
    /// </summary>
    /// <param name="normalized"></param>
    /// <param name="normalizedFilePath"></param>
    /// <returns></returns>
    string Serialize(Root<T> normalized, string normalizedFilePath);
}