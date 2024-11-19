using Microsoft.Extensions.DependencyInjection;
using AskGenAi.Application;
using AskGenAi.Common;
using AskGenAi.Core.Entities;
using AskGenAi.Core.Interfaces;
using AskGenAi.Infrastructure;
using AskGenAi.Presentation;
using Microsoft.Extensions.Logging;

var serviceProvider = new ServiceCollection()

    .AddLogging(configure =>
    {
        configure.ClearProviders();
        configure.AddConsole();
        configure.SetMinimumLevel(LogLevel.Information); // Set the minimum log level
    })
    .AddConfiguration()
    .AddApplicationServices()
    .AddCommonServices()
    .AddInfrastructureServices()
    .BuildServiceProvider();

var classNormalizerService = serviceProvider.GetRequiredService<IClassNormalizerService>();
//await classNormalizerService.NormalizeQuestionAsync();

var responseAiGenerator = serviceProvider.GetRequiredService<IResponseAiGenerator>();
//await responseAiGenerator.RunForAllWithoutResponseAsync();

var repo = serviceProvider.GetRequiredService<IRepository<User>>();
//var projected = await repo.GetAllProjectedAsync(u => new { u.Email, u.Name });

var executor = serviceProvider.GetRequiredService<IDataTransferExecutor>();
//await executor.ExecuteAsync();

var randomizer = serviceProvider.GetRequiredService<IRandomizer>();
//var key = randomizer.GenerateKey();

var reportGeneratorHandler = serviceProvider.GetRequiredService<IReportGeneratorHandler>();
//await reportGeneratorHandler.GenerateAllTextFilesReportSeparateAsync("md");
//await reportGeneratorHandler.GenerateDocxReportAsync(Guid.Parse("e7789b51-02b5-44ad-a67c-958afb7a6212"));
//Console.ReadLine();