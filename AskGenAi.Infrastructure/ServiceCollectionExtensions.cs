using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AskGenAi.Infrastructure.AIServices;
using AskGenAi.Infrastructure.FileSystem;
using AskGenAi.Infrastructure.Persistence;
using AskGenAi.Core.Interfaces;
using AskGenAi.Infrastructure.ApplicationDbContext;

namespace AskGenAi.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // Get the configuration from the service collection
        var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        services.AddScoped<IChatModelManager, AzureOpenAiChatCompletion>();

        services.AddSingleton<IFilePath, FilePath>();
        services.AddSingleton<IFileSystem, FileSystem.FileSystem>();

        services.AddScoped(typeof(IOnPremisesRepository<>), typeof(FileRepository<>));
        //services.AddScoped(typeof(IOnPremisesRepository<>), typeof(InMemoryRepository<>));

        var connectionStrings = configuration["SqlDatabase:ConnectionStrings:DefaultConnection"];
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionStrings));

        return services;
    }
}