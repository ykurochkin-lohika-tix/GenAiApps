using AskGenAi.Application.Services;
using AskGenAi.Core.Entities;
using AskGenAi.Core.Interfaces;

namespace AskGenAi.Application.UseCases;

// </inheritdoc>
public class ResponseAiGenerator(
    IChatModelManager chatModelManager,
    IHistoryBuilder historyBuilder,
    IRepository<Discipline> disciplineRepository,
    IRepository<Response> responseRepository,
    IRepository<Question> questionRepository,
    TimeSpan? delayDuration = null)
    : IResponseAiGenerator
{
    private readonly TimeSpan _delayDuration = delayDuration ?? TimeSpan.FromSeconds(30);

    // </inheritdoc>
    public async Task RunForAllAsync()
    {
        // Prepare the chat history. Take all disciplines and all questions for it
        var disciplines = await disciplineRepository.GetAllAsync(null);
        var allResponses = (await responseRepository.GetAllAsync(null)).ToDictionary(r => r.QuestionId);

        foreach (var discipline in disciplines)
        {
            var questionsForDiscipline = discipline.Questions;

            // Service to get history template with certain discipline
            var historySystemMessage = historyBuilder.BuildQuestionHistory(
                PersonalityHelper.GetPersonality(discipline.Type),
                discipline.Scope, discipline.Title, discipline.Subtitle);

            chatModelManager.AddSystemMessage(historySystemMessage);

            var newDisciplineResponses = new List<Response>();

            foreach (var question in questionsForDiscipline)
            {
                if (allResponses.ContainsKey(question.Id))
                {
                    continue;
                }

                chatModelManager.AddUserMessage(question.Context ?? string.Empty);
                var result = await chatModelManager.GetChatMessageContentAsync();

                // Save the response to the question
                var response = new Response
                {
                    Id = Guid.NewGuid(),
                    QuestionId = question.Id,
                    Context = result
                };
                newDisciplineResponses.Add(response);

                // make calls to the chat completion service to get the response with some delay some sec between each question
                await Task.Delay(_delayDuration);
            }

            // Save the changes to the database
            await responseRepository.AddRangeAsync(CancellationToken.None, [.. newDisciplineResponses]);
            await responseRepository.UnitOfWork.SaveChangesAsync();
        }
    }

    // </inheritdoc>
    public async Task RunForAllWithoutResponseAsync()
    {
        var onlyQuestions = await questionRepository.GetAllAsync(r => r.Responses.Count == 0);

        var groupedOnlyQuestions = onlyQuestions.GroupBy(q => q.DisciplineId);

        foreach (var group in groupedOnlyQuestions)
        {
            var discipline = await disciplineRepository.GetByIdAsync(group.Key);

            // If discipline is null, skip the group
            if (discipline == null)
            {
                continue;
            }

            var historySystemMessage = historyBuilder.BuildQuestionHistory(
                PersonalityHelper.GetPersonality(discipline.Type),
                discipline.Scope, discipline.Title, discipline.Subtitle);

            chatModelManager.AddSystemMessage(historySystemMessage);

            var newDisciplineResponses = new List<Response>();

            foreach (var question in group)
            {
                chatModelManager.AddUserMessage(question.Context ?? string.Empty);
                var result = await chatModelManager.GetChatMessageContentAsync();

                var response = new Response
                {
                    Id = Guid.NewGuid(),
                    QuestionId = question.Id,
                    Context = result
                };
                newDisciplineResponses.Add(response);

                await Task.Delay(_delayDuration);
            }

            await responseRepository.AddRangeAsync(CancellationToken.None, [.. newDisciplineResponses]);
            await responseRepository.UnitOfWork.SaveChangesAsync();
        }
    }
}