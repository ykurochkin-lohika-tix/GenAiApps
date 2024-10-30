using Microsoft.Extensions.DependencyInjection;
using AskGenAi.Application;
using AskGenAi.Common;
using AskGenAi.Core.Interfaces;
using AskGenAi.Infrastructure;
using AskGenAi.Presentation;

var serviceProvider = new ServiceCollection()
    .AddConfiguration()
    .AddApplicationServices()
    .AddCommonServices()
    .AddInfrastructureServices()
    .BuildServiceProvider();

var classNormalizerService = serviceProvider.GetRequiredService<IClassNormalizerService>();
//await classNormalizerService.NormalizeQuestionAsync();

var responseAiGenerator = serviceProvider.GetRequiredService<IResponseAiGenerator>();
await responseAiGenerator.RunAsync();