using System.Text.Json.Serialization;

namespace AskGenAi.Core.Entities;

public class Discipline : IEntity
{
    [JsonPropertyName("id")] 
    public Guid Id { get; set; }

    [JsonPropertyName("type")] 
    public int Type { get; set; }

    [JsonPropertyName("title")] 
    public string? Title { get; set; }

    [JsonPropertyName("subtitle")] 
    public string? Subtitle { get; set; }

    [JsonPropertyName("goal")] 
    public string? Goal { get; set; }

    [JsonPropertyName("scope")] 
    public string? Scope { get; set; }
}