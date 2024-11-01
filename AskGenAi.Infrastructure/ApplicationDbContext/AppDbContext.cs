using Microsoft.EntityFrameworkCore;
using AskGenAi.Core.Entities;
using AskGenAi.Core.Interfaces;

namespace AskGenAi.Infrastructure.ApplicationDbContext;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<User> Users { get; set; }
    public DbSet<Discipline> Disciplines { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Response> Responses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(u => u.Id).ValueGeneratedOnAdd();
            entity.Property(u => u.Name).HasMaxLength(100);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Email).HasMaxLength(100);
        });

        // Discipline
        modelBuilder.Entity<Discipline>(entity =>
        {
            entity.Property(d => d.Id).ValueGeneratedOnAdd();
            entity.Property(d => d.Type).HasConversion<int>();
            entity.Property(d => d.Title).HasMaxLength(150);
            entity.Property(d => d.Subtitle).HasMaxLength(150);
            entity.Property(d => d.Scope).HasMaxLength(150);
            entity.HasMany(d => d.Questions)
                .WithOne(q => q.Discipline)
                .HasForeignKey(q => q.DisciplineId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Question
        modelBuilder.Entity<Question>(entity =>
        {
            entity.Property(q => q.Id).ValueGeneratedOnAdd();
            entity.HasMany(q => q.Responses)
                .WithOne(r => r.Question)
                .HasForeignKey(r => r.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Response
        modelBuilder.Entity<Response>(entity =>
        {
            entity.Property(r => r.Id).ValueGeneratedOnAdd();
            entity.HasOne(r => r.Question)
                .WithMany(q => q.Responses)
                .HasForeignKey(r => r.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}