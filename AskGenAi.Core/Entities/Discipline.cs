using AskGenAi.Core.Enums;

namespace AskGenAi.Core.Entities;

public class Discipline : IEntityRoot
{
    public Guid Id { get; set; }

    public DisciplineType Type { get; set; }

    public string? Title { get; set; }

    public string? Subtitle { get; set; }

    public string? Scope { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}