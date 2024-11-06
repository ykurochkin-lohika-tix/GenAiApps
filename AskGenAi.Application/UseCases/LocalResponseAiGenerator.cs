using AskGenAi.Application.Services;
using AskGenAi.Core.Enums;
using AskGenAi.Core.Interfaces;
using AskGenAi.Core.Models;

namespace AskGenAi.Application.UseCases;

// </inheritdoc>
public class LocalResponseAiGenerator(
    IChatModelManager chatModelManager,
    IHistoryBuilder historyBuilder,
    IOnPremisesRepository<DisciplineOnPremises> disciplineOnPremisesRepository,
    IOnPremisesRepository<QuestionOnPremises> questionOnPremisesRepository,
    IOnPremisesRepository<ResponseOnPremises> responseOnPremisesRepository,
    TimeSpan? delayDuration = null)
    : IResponseAiGenerator
{
    private readonly TimeSpan _delayDuration = delayDuration ?? TimeSpan.FromSeconds(30);

    // </inheritdoc>
    public async Task RunForAllAsync()
    {
        // Prepare the chat history. Take all disciplines and all question for it
        var disciplines = await disciplineOnPremisesRepository.GetAllAsync();
        var questions = await questionOnPremisesRepository.GetAllAsync();
        var responses = (await responseOnPremisesRepository.GetAllAsync()).ToArray();

        var group = disciplines.GroupJoin(questions, discipline => discipline.Type, question => question.DisciplineType,
            (discipline, enumerable) => new { discipline, enumerable });

        // Launch the cycle of questions and clear the history after each discipline change generating new history.
        foreach (var item in group)
        {
            var discipline = item.discipline;
            var questionsForDiscipline = item.enumerable;

            // Service to get history template with certain discipline
            var historySystemMessage =
                historyBuilder.BuildQuestionHistory(PersonalityHelper.GetPersonality((DisciplineType)discipline.Type),
                    discipline.Scope, discipline.Title,
                    discipline.Subtitle);

            chatModelManager.AddSystemMessage(historySystemMessage);

            foreach (var question in questionsForDiscipline)
            {
                // If the response to the question already exists, skip it
                if (Array.Exists(responses, r => r.QuestionId == question.Id))
                {
                    continue;
                }

                // Add the question to the chat history and get the response
                chatModelManager.AddUserMessage(question.Context ?? string.Empty);
                var result = await chatModelManager.GetChatMessageContentAsync();

                // Save the response to the question
                await responseOnPremisesRepository.AddAsync(new ResponseOnPremises
                {
                    Id = Guid.NewGuid(),
                    DisciplineType = discipline.Type,
                    Context = result,
                    QuestionId = question.Id
                });

                // make calls to the chat completion service to get the response with some delay 40 sec between each question
                await Task.Delay(_delayDuration);
            }
        }
    }

    // </inheritdoc>
    public Task RunForAllWithoutResponseAsync()
    {
        return RunForAllAsync();
    }
}