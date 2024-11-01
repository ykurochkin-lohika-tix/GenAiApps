using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AskGenAi.Core.Entities;
using AskGenAi.Core.Interfaces;
using AskGenAi.Infrastructure.ApplicationDbContext;
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

        services.AddScoped<IChatModelManager, AzureOpenAiChatCompletion>();

        services.AddSingleton<IFilePath, FilePath>();
        services.AddSingleton<IFileSystem, FileSystem.FileSystem>();

        services.AddScoped(typeof(IOnPremisesRepository<>), typeof(FileRepository<>));
        //services.AddScoped(typeof(IOnPremisesRepository<>), typeof(InMemoryRepository<>));

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration["SqlDatabase:ConnectionStrings:DefaultConnection"]));

        services.AddScoped<IRepository<User>, Repository<User>>();
        services.AddScoped<IRepository<Discipline>, Repository<Discipline>>();
        services.AddScoped<IRepository<Question>, Repository<Question>>();
        services.AddScoped<IRepository<Response>, Repository<Response>>();

        return services;
    }
}