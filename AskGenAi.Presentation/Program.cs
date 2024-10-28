using Microsoft.Extensions.DependencyInjection;

using AskGenAi.Application;
using AskGenAi.Common;
using AskGenAi.Core.Interfaces;
using AskGenAi.Infrastructure;

var serviceProvider = new ServiceCollection()
    .AddApplicationServices()
    .AddCommonServices()
    .AddInfrastructureServices()
    .BuildServiceProvider();

var classNormalizerService = serviceProvider.GetRequiredService<IClassNormalizerService>();
//await classNormalizerService.NormalizeQuestionAsync();

var responseAiGenerator = serviceProvider.GetRequiredService<IResponseAiGenerator>();
await responseAiGenerator.RunAsync();