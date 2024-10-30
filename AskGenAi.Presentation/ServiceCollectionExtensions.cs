using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AskGenAi.Presentation;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfiguration(this IServiceCollection services)
    {
        // Setup configuration to read from appsettings.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .Build();

        services.AddSingleton<IConfiguration>(configuration);

        return services;
    }
}