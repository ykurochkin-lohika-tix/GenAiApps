using Microsoft.Extensions.DependencyInjection;
using AskGenAi.Application.UseCases;
using AskGenAi.Application.Services;
using AskGenAi.Application.Mapping;
using AskGenAi.Core.Interfaces;
using AskGenAi.Core.Entities;
using AskGenAi.Core.Models;

namespace AskGenAi.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));

        services
            .AddScoped<IDataTransfer<DisciplineOnPremises, Discipline>,
                DataTransfer<DisciplineOnPremises, Discipline>>();
        services.AddScoped<IDataTransfer<QuestionOnPremises, Question>, DataTransfer<QuestionOnPremises, Question>>();
        services.AddScoped<IDataTransfer<ResponseOnPremises, Response>, DataTransfer<ResponseOnPremises, Response>>();

        services.AddScoped<IHistoryBuilder, HistoryBuilder>();
        services.AddSingleton<IRandomizer, Randomizer>();

        services.AddScoped<IClassNormalizerService, ClassNormalizerService>();
        services.AddScoped<IResponseAiGenerator, ResponseAiGenerator>();
        //services.AddScoped<IResponseAiGenerator, LocalResponseAiGenerator>();
        services.AddScoped<IDataTransferExecutor, DataTransferExecutor>();
        services.AddScoped<IReportGeneratorHandler, ReportGeneratorHandler>();

        return services;
    }
}