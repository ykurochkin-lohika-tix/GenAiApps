namespace AskGenAi.Core.Interfaces;

/// <summary>
/// Represents a service that normalizes classes by getting raw data from file adding Ids to them and storing them in the new file
/// </summary>
public interface IClassNormalizerService
{
    /// <summary>
    /// Asynchronously normalizes disciplines by adding Ids to them and storing them in the new file
    /// </summary>
    /// <returns></returns>
    Task NormalizeDisciplineAsync();

    /// <summary>
    /// Asynchronously normalizes disciplines by adding Ids to them and storing them in the new file
    /// </summary>
    /// <returns></returns>
    Task NormalizeQuestionAsync();

    /// <summary>
    /// Asynchronously normalizes all questions by adding Ids to them and storing them in the single new file
    /// </summary>
    /// <returns></returns>
    Task NormalizeQuestionsAsync();
}