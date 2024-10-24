namespace AskGenAi.Core.Interfaces;

public interface IHistoryBuilder
{
    /// <summary>
    /// Builds the question history
    /// </summary>
    /// <param name="personality"></param>
    /// <param name="technologies"></param>
    /// <param name="discipline"></param>
    /// <param name="subDiscipline"></param>
    /// <returns></returns>
    string BuildQuestionHistory(string? personality, string? technologies, string? discipline, string? subDiscipline);
}
