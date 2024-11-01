namespace AskGenAi.Core.Entities;

public class Question : IEntityRoot
{
    public Guid Id { get; set; }

    public string? Context { get; set; }

    public Guid DisciplineId { get; set; }

    public virtual Discipline? Discipline { get; set; }

    public virtual ICollection<Response> Responses { get; set; } = new List<Response>();
}