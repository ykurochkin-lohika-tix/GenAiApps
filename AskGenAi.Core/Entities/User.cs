namespace AskGenAi.Core.Entities;

public class User : IEntityRoot
{
    public Guid Id { get; set; }
    public string PasswordHash { get; set; } = default!;
    public string Email { get; set; } = default!;
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}