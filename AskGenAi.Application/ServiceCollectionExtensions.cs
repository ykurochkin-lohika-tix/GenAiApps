using AskGenAi.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using AskGenAi.Application.UseCases;
using AskGenAi.Core.Interfaces;

namespace AskGenAi.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IHistoryBuilder, HistoryBuilder>();

        services.AddScoped<IClassNormalizerService, ClassNormalizerService>();
        services.AddScoped<IResponseAiGenerator, ResponseAiGenerator>();

        return services;
    }
}