using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AskGenAi.Core.Entities;
using AskGenAi.Core.Interfaces;
using AskGenAi.Infrastructure.AIServices;
using AskGenAi.Infrastructure.FileSystem;
using AskGenAi.Infrastructure.Persistence;

namespace AskGenAi.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // Get the configuration from the service collection
        var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        // Bind AzureOpenAI settings
        services.Configure<AzureOpenAiSettings>(configuration.GetSection("AzureOpenAI"));

        // Add services to the container
        services.AddScoped<IChatModelManager, AzureOpenAiChatCompletion>();
        services.AddSingleton<IFilePath, FilePath>();
        services.AddSingleton<IFileSystem, FileSystem.FileSystem>();
        services.AddScoped<IReportGenerator, ReportGenerator.ReportGenerator>();
        services.AddScoped(typeof(IOnPremisesRepository<>), typeof(FileRepository<>));

        services.AddScoped<IRepository<User>, Repository<User>>();
        services.AddScoped<IRepository<Role>, Repository<Role>>();
        services.AddScoped<IRepository<UserRole>, Repository<UserRole>>();
        services.AddScoped<IRepository<Discipline>, Repository<Discipline>>();
        services.AddScoped<IRepository<Question>, Repository<Question>>();
        services.AddScoped<IRepository<Response>, Repository<Response>>();

        return services;
    }
}