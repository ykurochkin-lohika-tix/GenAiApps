using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AskGenAi.Infrastructure.ApplicationDbContext;

// support for design-time tools
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // Build configuration to access the connection string
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        // Set up the DbContext options with SQL Server
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>(); 
        var connectionString = configuration["SqlDatabase:ConnectionStrings:DefaultConnection"];
        optionsBuilder.UseSqlServer(connectionString);

        return new AppDbContext(optionsBuilder.Options);
    }
}