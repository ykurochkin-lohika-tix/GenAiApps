namespace AskGenAi.Core.Entities;

public class Response : IEntityRoot
{
    public Guid Id { get; set; }

    public string? Context { get; set; }

    public Guid QuestionId { get; set; }

    public virtual Question? Question { get; set; }
}