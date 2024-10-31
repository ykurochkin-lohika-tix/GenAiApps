using AskGenAi.Infrastructure.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AskGenAi.xTests.Utils;

internal static class EfCoreContextHelper
{
    public static IServiceCollection AddDbContextInMemory(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseInMemoryDatabase("InMemoryDbForTesting" + Guid.NewGuid());
        });

        return services;
    }

    /// <summary>
    ///     Prepare an in-memory db <see cref="AppDbContext" /> with the name <paramref name="dbName" />
    /// </summary>
    /// <param name="dbName"></param>
    /// <returns></returns>
    public static AppDbContext GetAppDbContextInMemory(string? dbName = null)
    {
        dbName ??= Guid.NewGuid().ToString();

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder
            .UseInMemoryDatabase(dbName)
            .ConfigureWarnings(builder => builder.Ignore(InMemoryEventId.TransactionIgnoredWarning));
        return new AppDbContext(optionsBuilder.Options);
    }
}