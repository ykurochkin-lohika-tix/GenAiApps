namespace AskGenAi.Core.Interfaces;

public interface IReportGenerator
{
    /// <summary>
    /// Generates a docx report for the given discipline ids. Local file version
    /// </summary>
    /// <param name="disciplineIds"></param>
    /// <param name="outputPath"></param>
    /// <returns></returns>
    Task GenerateDocxReportAsync(IEnumerable<Guid> disciplineIds, string outputPath);

    /// <summary>
    /// Generates a docx report for the given discipline ids. Web version
    /// </summary>
    /// <param name="disciplineIds"></param>
    /// <returns></returns>
    Task<Stream> GenerateDocxReportAsync(IEnumerable<Guid> disciplineIds);

    /// <summary>
    /// Generates a txt report for the given discipline ids
    /// </summary>
    /// <param name="disciplineIds"></param>
    /// <param name="outputPath"></param>
    /// <returns></returns>
    Task GenerateTextFilesReportAsync(IEnumerable<Guid> disciplineIds, string outputPath);

    /// <summary>
    /// Generates a txt report for the given discipline ids. Web version
    /// </summary>
    /// <param name="disciplineIds"></param>
    /// <returns></returns>
    Task<Stream> GenerateTextFilesReportAsync(IEnumerable<Guid> disciplineIds);
}