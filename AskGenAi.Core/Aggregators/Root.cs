using System.Text.Json.Serialization;
using AskGenAi.Core.Entities;

namespace AskGenAi.Core.Aggregators;

public class Root<T> where T : IEntity
{
    [JsonPropertyName("version")]
    public string Version { get; set; } = default!;

    [JsonPropertyName("data")]
    public List<T> Data { get; set; } = [];
}