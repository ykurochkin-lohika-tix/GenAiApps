using Microsoft.Extensions.DependencyInjection;
using AskGenAi.Infrastructure.AIServices;
using AskGenAi.Infrastructure.FileSystem;
using AskGenAi.Infrastructure.Persistence;
using AskGenAi.Core.Interfaces;

namespace AskGenAi.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IChatModelManager, AzureOpenAiChatCompletion>();

        services.AddSingleton<IFilePath, FilePath>();
        services.AddSingleton<IFileSystem, FileSystem.FileSystem>();

        services.AddScoped(typeof(IRepository<>), typeof(FileRepository<>));
        //services.AddScoped(typeof(IRepository<>), typeof(InMemoryRepository<>));

        return services;
    }
}