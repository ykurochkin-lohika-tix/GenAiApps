using AskGenAi.Application.Services;
using AskGenAi.Application.UseCases;
using AskGenAi.Core.Entities;
using AskGenAi.Core.Interfaces;
using AskGenAi.Infrastructure.AIServices;
using AskGenAi.Infrastructure.FileSystem;
using AskGenAi.Infrastructure.Persistence;

IFilePath filePath = new FilePath();
IClassNormalizerService classNormalizerService =
    new ClassNormalizerService(new FileRepository<Question>(filePath.GetLocalQuestionsFullPath()), filePath);
//await classNormalizerService.NormalizeQuestionsAsync();

IResponseAiGenerator responseAiGenerator = new ResponseAiGenerator(
    new AzureOpenAiChatCompletion(),
    new HistoryBuilder(),
    new FileRepository<Discipline>(filePath.GetLocalDisciplinesFullPath()),
    new FileRepository<Question>(filePath.GetLocalQuestionsFullPath()),
    new FileRepository<Response>(filePath.GetLocalResponsesFullPath())
);

await responseAiGenerator.RunAsync();