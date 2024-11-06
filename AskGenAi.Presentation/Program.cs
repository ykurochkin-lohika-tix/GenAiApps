using Microsoft.Extensions.DependencyInjection;
using AskGenAi.Application;
using AskGenAi.Common;
using AskGenAi.Core.Entities;
using AskGenAi.Core.Interfaces;
using AskGenAi.Infrastructure;
using AskGenAi.Presentation;

var serviceProvider = new ServiceCollection()
    .AddLogging()
    .AddConfiguration()
    .AddApplicationServices()
    .AddCommonServices()
    .AddInfrastructureServices()
    .BuildServiceProvider();

var classNormalizerService = serviceProvider.GetRequiredService<IClassNormalizerService>();
//await classNormalizerService.NormalizeQuestionAsync();

var responseAiGenerator = serviceProvider.GetRequiredService<IResponseAiGenerator>();
await responseAiGenerator.RunForAllWithoutResponseAsync();

var repo = serviceProvider.GetRequiredService<IRepository<User>>();
//var projected = await repo.GetProjectedAsync(u => new { u.Email, u.Name });

var executor = serviceProvider.GetRequiredService<IDataTransferExecutor>();
//await executor.ExecuteAsync();

var randomizer = serviceProvider.GetRequiredService<IRandomizer>();
//var key = randomizer.GenerateKey();
