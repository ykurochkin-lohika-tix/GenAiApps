using AskGenAi.Application.Services;
using AskGenAi.Core.Entities;
using AskGenAi.Core.Interfaces;

namespace AskGenAi.Application.UseCases;

// </inheritdoc>
public class ResponseAiGenerator(
    IChatModelManager chatModelManager,
    IHistoryBuilder historyBuilder,
    IRepository<Discipline> disciplineRepository,
    IRepository<Question> questionRepository,
    IRepository<Response> responseRepository,
    TimeSpan? delayDuration = null)
    : IResponseAiGenerator
{
    private readonly TimeSpan _delayDuration = delayDuration ?? TimeSpan.FromSeconds(30);
    
    // </inheritdoc>
    public async Task RunAsync()
    {
        // Prepare the chat history. Take all disciplines and all question for it
        var disciplines = await disciplineRepository.GetAllAsync();
        var questions = await questionRepository.GetAllAsync();
        var responses = (await responseRepository.GetAllAsync()).ToArray();

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
                await responseRepository.AddAsync(new Response
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
}