using AskGenAi.Core.Entities;
using AskGenAi.Core.Interfaces;
using AskGenAi.Core.Models;

namespace AskGenAi.Application.UseCases;

public class DataTransferExecutor(
    IDataTransfer<DisciplineOnPremises, Discipline> disciplineDataTransfer,
    IDataTransfer<QuestionOnPremises, Question> questionDataTransfer,
    IDataTransfer<ResponseOnPremises, Response> responseDataTransfer
) : IDataTransferExecutor
{
    public async Task ExecuteAsync()
    {
        await disciplineDataTransfer.TransferLocalToCloudAsync();

        await QuestionsTransferLocalToCloudAsync();

        await responseDataTransfer.TransferLocalToCloudAsync();
    }

    private async Task QuestionsTransferLocalToCloudAsync()
    {
        var disciplinesOnProm = (await disciplineDataTransfer.GetSourceRepository().GetAllAsync()).ToArray();

        var questionsOnProm = (await questionDataTransfer.GetSourceRepository().GetAllAsync()).ToArray();
        var questions = (await questionDataTransfer.GetDestinationEntitiesAsync()).ToArray();

        foreach (var questionOnProm in questionsOnProm)
        {
            var disciplineOnProm = Array.Find(disciplinesOnProm, d => d.Type == questionOnProm.DisciplineType);

            var question = Array.Find(questions, q => q.Id == questionOnProm.Id);

            if (question == null || disciplineOnProm == null)
            {
                continue;
            }

            question.DisciplineId = disciplineOnProm.Id;
        }

        await questionDataTransfer.SaveChangesDestinationAsync(questions);
    }
}