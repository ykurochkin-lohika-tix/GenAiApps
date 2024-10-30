using Microsoft.EntityFrameworkCore;
using AskGenAi.Core.Entities;

namespace AskGenAi.Infrastructure.ApplicationDbContext;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .Property(u => u.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<User>()
            .Property(u => u.Name)
            .HasMaxLength(100);
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .HasMaxLength(100);

        // You can add custom configurations here (e.g., relationships, constraints).
        // However, for a strict clean architecture, keep all database-specific configurations in AskGenAi.Infrastructure by using the Fluent API within OnModelCreating
    }
}