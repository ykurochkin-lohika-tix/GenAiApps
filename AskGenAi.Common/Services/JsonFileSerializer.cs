using System.Text.Json;
using AskGenAi.Core.Entities;
using AskGenAi.Core.Interfaces;

namespace AskGenAi.Common.Services;

// </inheritdoc>
public class JsonFileSerializer<T> : IJsonFileSerializer<T> where T : IEntity
{
    // </inheritdoc>
    public async Task<Root<T>?> DeserializeAsync(string normalizeFilePath)
    {
        var allText = await File.ReadAllTextAsync(normalizeFilePath);
        var normalize = JsonSerializer.Deserialize<Root<T>>(allText);
        return normalize;
    }

    // </inheritdoc>
    public async Task<string> SerializeAsync(Root<T> normalized, string normalizedFilePath)
    {
        var serialized =
            JsonSerializer.Serialize(normalized, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(normalizedFilePath, serialized);
        return serialized;
    }

    // </inheritdoc>
    public Root<T>? Deserialize(string normalizeFilePath)
    {
        var allText = File.ReadAllText(normalizeFilePath);
        var normalize = JsonSerializer.Deserialize<Root<T>>(allText);
        return normalize;
    }

    // </inheritdoc>
    public string Serialize(Root<T> normalized, string normalizedFilePath)
    {
        var serialized =
            JsonSerializer.Serialize(normalized, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(normalizedFilePath, serialized);
        return serialized;
    }
}