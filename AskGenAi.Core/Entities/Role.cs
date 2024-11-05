namespace AskGenAi.Core.Entities;

public class Role : IEntityRoot
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}