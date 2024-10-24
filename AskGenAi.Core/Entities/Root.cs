using System.Text.Json.Serialization;

namespace AskGenAi.Core.Entities;

public class Root<T> where T : IEntity
{
    [JsonPropertyName("version")]
    public string Version { get; set; } = default!;

    [JsonPropertyName("data")]
    public List<T> Data { get; set; } = [];
}