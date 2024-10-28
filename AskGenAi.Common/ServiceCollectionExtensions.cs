using AskGenAi.Common.Services;
using AskGenAi.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AskGenAi.Common;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommonServices(this IServiceCollection services)
    {
        services.AddTransient(typeof(IJsonFileSerializer<>), typeof(JsonFileSerializer<>));

        return services;
    }
}