namespace AskGenAi.Core.Interfaces;

public interface IResponseAiGenerator
{
    /// <summary>
    /// Runs the AI generator, that will generate at least one response for all questions.
    /// Purpose is to generate responses for all questions like in first run.
    /// </summary>
    /// <returns></returns>
    Task RunForAllAsync();

    /// <summary>
    /// Runs the AI generator, finds all questions without responses and generates one response for them.
    /// Purpose is find recently added questions and generate responses for them.
    /// </summary>
    /// <returns></returns>
    Task RunForAllWithoutResponseAsync();
}