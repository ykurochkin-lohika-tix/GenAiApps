namespace AskGenAi.Core.Interfaces;

public interface IResponseAiGenerator
{
    /// <summary>
    /// Runs the AI generator
    /// </summary>
    /// <returns></returns>
    Task RunAsync();
}