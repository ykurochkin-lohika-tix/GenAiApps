namespace AskGenAi.Core.Interfaces;

public interface IReportGeneratorHandler
{
    /// <summary>
    /// Generates docx report for all disciplines in one file
    /// </summary>
    /// <returns></returns>
    Task GenerateAllDocxReportAsync();

    /// <summary>
    /// Generates docx report for all disciplines in a stream. Web version.
    /// </summary>
    /// <returns></returns>
    Task<Stream?> GenerateWebAllDocxReportAsync();

    /// <summary>
    /// Generates txt report for all disciplines in one file
    /// </summary>
    /// <returns></returns>
    Task GenerateAllTxtReportAsync();

    /// <summary>
    /// Generates md report for all disciplines in one file
    /// </summary>
    /// <returns></returns>
    Task GenerateAllMdReportAsync();

    /// <summary>
    /// Generates docx report for the given discipline id
    /// </summary>
    /// <param name="disciplineId"></param>
    /// <returns></returns>
    Task GenerateDocxReportAsync(Guid disciplineId);

    /// <summary>
    /// Generates docx in a stream report for the given discipline id. Web version.
    /// </summary>
    /// <param name="disciplineId"></param>
    /// <returns></returns>
    Task<Stream?> GenerateWebDocxReportAsync(Guid disciplineId);

    /// <summary>
    /// Generates md report in a stream for all disciplines in stream. Web version.
    /// </summary>
    /// <returns></returns>
    Task<Stream?> GenerateWebAllMdReportAsync();

    /// <summary>
    /// Generates txt report in a stream for all disciplines in stream. Web version.
    /// </summary>
    /// <returns></returns>
    Task<Stream?> GenerateWebAllTxtReportAsync();

    /// <summary>
    /// Generates txt or md report for the given discipline id
    /// </summary>
    /// <param name="disciplineId"></param>
    /// <param name="fileExtension">txt or md</param>
    /// <returns></returns>
    Task GenerateTextFilesReportAsync(Guid disciplineId, string fileExtension);

    /// <summary>
    /// Generates txt or md report in a stream for the given discipline id. Web version.
    /// </summary>
    /// <param name="disciplineId"></param>
    /// <returns></returns>
    Task<Stream?> GenerateWebTextFilesReportAsync(Guid disciplineId);

    /// <summary>
    /// Generates txt or md report for discipline ids provided in one file
    /// </summary>
    /// <param name="disciplineIds"></param>
    /// <returns></returns>
    Task<Stream> GenerateWebTextFilesReportAsync(IEnumerable<Guid> disciplineIds);

    /// <summary>
    /// Generates txt or md report for all disciplines in one file per discipline
    /// </summary>
    /// <param name="fileExtension">txt or md</param>
    /// <returns></returns>
    Task GenerateAllTextFilesReportSeparateAsync(string fileExtension);
}