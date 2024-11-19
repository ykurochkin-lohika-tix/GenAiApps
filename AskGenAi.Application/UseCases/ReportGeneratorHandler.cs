using AskGenAi.Core.Entities;
using AskGenAi.Core.Interfaces;

namespace AskGenAi.Application.UseCases;

public class ReportGeneratorHandler(
    IReportGenerator reportGenerator,
    IRepository<Discipline> disciplineRepository,
    IFilePath filePath) : IReportGeneratorHandler
{
    // </inheritdoc>
    public async Task GenerateAllDocxReportAsync()
    {
        var disciplineIds = await GetAllDisciplineIdsAsync();
        if (!disciplineIds.Any())
        {
            return;
        }

        var outputPath = filePath.GetFullReportPath("docx");
        await reportGenerator.GenerateDocxReportAsync(disciplineIds, outputPath);
    }

    // </inheritdoc>
    public async Task<Stream?> GenerateWebAllDocxReportAsync()
    {
        var disciplineIds = await GetAllDisciplineIdsAsync();
        if (!disciplineIds.Any())
        {
            return null;
        }

        return await reportGenerator.GenerateDocxReportAsync(disciplineIds);
    }

    // </inheritdoc>
    public Task GenerateAllMdReportAsync()
    {
        return GenerateAllTextFilesReportAsync("md");
    }

    // </inheritdoc>
    public Task GenerateAllTxtReportAsync()
    {
        return GenerateAllTextFilesReportAsync("txt");
    }

    // </inheritdoc>
    public Task<Stream?> GenerateWebAllMdReportAsync()
    {
        return GenerateWebAllTextFilesReportAsync("md");
    }

    // </inheritdoc>
    public Task<Stream?> GenerateWebAllTxtReportAsync()
    {
        return GenerateWebAllTextFilesReportAsync("txt");
    }

    // </inheritdoc>
    public async Task GenerateDocxReportAsync(Guid disciplineId)
    {
        var discipline = await disciplineRepository.GetByIdAsync(disciplineId);
        if (discipline == null)
        {
            return;
        }

        var outputPath = GetOutputPath(discipline, "docx");
        await reportGenerator.GenerateDocxReportAsync(new[] { disciplineId }, outputPath);
    }

    // </inheritdoc>
    public async Task<Stream?> GenerateWebDocxReportAsync(Guid disciplineId)
    {
        var discipline = await disciplineRepository.GetByIdAsync(disciplineId);
        if (discipline == null)
        {
            return null;
        }

        return await reportGenerator.GenerateDocxReportAsync(new[] { disciplineId });
    }

    // </inheritdoc>
    public async Task GenerateTextFilesReportAsync(Guid disciplineId, string fileExtension)
    {
        var discipline = await disciplineRepository.GetByIdAsync(disciplineId);
        if (discipline == null)
        {
            return;
        }

        var outputPath = GetOutputPath(discipline, fileExtension);
        await reportGenerator.GenerateTextFilesReportAsync(new[] { disciplineId }, outputPath);
    }

    // </inheritdoc>
    public async Task<Stream?> GenerateWebTextFilesReportAsync(Guid disciplineId)
    {
        var discipline = await disciplineRepository.GetByIdAsync(disciplineId);
        if (discipline == null)
        {
            return null;
        }

        return await reportGenerator.GenerateTextFilesReportAsync(new[] { disciplineId });
    }

    // </inheritdoc>
    public async Task<Stream> GenerateWebTextFilesReportAsync(IEnumerable<Guid> disciplineIds)
    {
        return await reportGenerator.GenerateTextFilesReportAsync(disciplineIds);
    }

    // </inheritdoc>
    public async Task GenerateAllTextFilesReportSeparateAsync(string fileExtension)
    {
        var disciplineIds = await GetAllDisciplineIdsAsync();
        if (!disciplineIds.Any() || string.IsNullOrWhiteSpace(fileExtension))
        {
            return;
        }

        foreach (var disciplineId in disciplineIds)
        {
            await GenerateTextFilesReportAsync(disciplineId, fileExtension);
        }
    }

    private async Task GenerateAllTextFilesReportAsync(string fileExtension)
    {
        var disciplineIds = await GetAllDisciplineIdsAsync();
        if (!disciplineIds.Any() || string.IsNullOrWhiteSpace(fileExtension))
        {
            return;
        }

        var outputPath = filePath.GetFullReportPath(fileExtension);
        await reportGenerator.GenerateTextFilesReportAsync(disciplineIds, outputPath);
    }

    private async Task<Stream?> GenerateWebAllTextFilesReportAsync(string fileExtension)
    {
        var disciplineIds = await GetAllDisciplineIdsAsync();
        if (!disciplineIds.Any() || string.IsNullOrWhiteSpace(fileExtension))
        {
            return null;
        }

        return await reportGenerator.GenerateTextFilesReportAsync(disciplineIds);
    }

    private async Task<Guid[]> GetAllDisciplineIdsAsync()
    {
        //return (await disciplineRepository.GetAllProjectedAsync(d => d.Id)).ToArray();
        // TODO rewrite it and do all the logic in the database
        return (await disciplineRepository.GetAllNoTrackAsync(null)).OrderBy(d => d.Type).Select(d => d.Id).ToArray();
    }

    private string GetOutputPath(Discipline discipline, string fileExtension)
    {
        var filename = string.IsNullOrWhiteSpace(discipline.Title + discipline.Subtitle)
            ? "report_no_title"
            : discipline.Title + discipline.Subtitle;

        return filePath.GetReportPath(filename, fileExtension);
    }
}