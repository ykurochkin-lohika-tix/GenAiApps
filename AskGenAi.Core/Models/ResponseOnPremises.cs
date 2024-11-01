using System.Text.Json.Serialization;

namespace AskGenAi.Core.Models;

public class ResponseOnPremises : IEntity
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("questionId")]
    public Guid QuestionId { get; set; }

    [JsonPropertyName("disciplineType")]
    public int DisciplineType { get; set; }

    [JsonPropertyName("context")]
    public string? Context { get; set; }
}