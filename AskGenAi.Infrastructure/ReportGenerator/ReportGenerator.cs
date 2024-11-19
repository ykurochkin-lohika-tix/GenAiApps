using AskGenAi.Core.Entities;
using AskGenAi.Core.Interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text;
using Microsoft.Extensions.Logging;

namespace AskGenAi.Infrastructure.ReportGenerator;

public class ReportGenerator(IRepository<Discipline> disciplineRepository, IFileSystem fileSystem, ILogger<ReportGenerator> logger) : IReportGenerator
{
    // <inheritdoc />
    public async Task GenerateDocxReportAsync(IEnumerable<Guid> disciplineIds, string outputPath)
    {
        using var stream = await GenerateDocxReportStreamAsync(disciplineIds);
        await using var fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
        stream.WriteTo(fileStream);
    }

    // <inheritdoc />
    public async Task<Stream> GenerateDocxReportAsync(IEnumerable<Guid> disciplineIds)
    {
        var stream = await GenerateDocxReportStreamAsync(disciplineIds);
        stream.Position = 0; // Reset the stream position to the beginning
        return stream;
    }

    // <inheritdoc />
    public async Task GenerateTextFilesReportAsync(IEnumerable<Guid> disciplineIds, string outputPath)
    {
        var reportContent = await GenerateTextReportContentAsync(disciplineIds);
        await fileSystem.WriteAllTextAsync(outputPath, reportContent);
    }

    // <inheritdoc />
    public async Task<Stream> GenerateTextFilesReportAsync(IEnumerable<Guid> disciplineIds)
    {
        var reportContent = await GenerateTextReportContentAsync(disciplineIds);
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(reportContent));
        stream.Position = 0; // Reset the stream position to the beginning
        return stream;
    }

    private async Task<string> GenerateTextReportContentAsync(IEnumerable<Guid> disciplineIds)
    {
        var reportContent = new StringBuilder();

        foreach (var disciplineId in disciplineIds)
        {
            var discipline = await disciplineRepository.GetByIdAsync(disciplineId);
            if (discipline == null)
            {
                continue;
            }

            reportContent.AppendLine($"# Discipline: {discipline.Title}");
            reportContent.AppendLine($"Subtitle: {discipline.Subtitle}");
            reportContent.AppendLine($"Scope: {discipline.Scope}");
            reportContent.AppendLine();

            foreach (var question in discipline.Questions)
            {
                reportContent.AppendLine($"## Question: {question.Context}");
                foreach (var response in question.Responses)
                {
                    reportContent.AppendLine($"  Response: {response.Context}");
                }

                reportContent.AppendLine();
            }
            logger.LogInformation("Generated report for discipline {DisciplineId}", disciplineId);
        }

        logger.LogInformation("Generated report for disciplines");
        return reportContent.ToString();
    }

    private async Task<MemoryStream> GenerateDocxReportStreamAsync(IEnumerable<Guid> disciplineIds)
    {
        var stream = new MemoryStream();
        using var wordDocument = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document, true);
        var mainPart = wordDocument.AddMainDocumentPart();
        mainPart.Document = new Document();
        var body = new Body();

        foreach (var disciplineId in disciplineIds)
        {
            var discipline = await disciplineRepository.GetByIdAsync(disciplineId);
            if (discipline == null)
            {
                continue;
            }

            body.AppendChild(new Paragraph(new Run(new Text($"Discipline: {discipline.Title}"))));
            body.AppendChild(new Paragraph(new Run(new Text($"Subtitle: {discipline.Subtitle}"))));
            body.AppendChild(new Paragraph(new Run(new Text($"Scope: {discipline.Scope}"))));
            body.AppendChild(new Paragraph(new Run(new Text(""))));

            foreach (var question in discipline.Questions)
            {
                body.AppendChild(new Paragraph(new Run(new Text($"Question: {question.Context}"))));
                foreach (var response in question.Responses)
                {
                    body.AppendChild(new Paragraph(new Run(new Text($"  Response: {response.Context}"))));
                }

                body.AppendChild(new Paragraph(new Run(new Text(""))));
            }

            logger.LogInformation("Generated report for discipline {DisciplineId}", disciplineId);
        }

        logger.LogInformation("Generated report for disciplines");
        mainPart.Document.Append(body);
        mainPart.Document.Save();
        return stream;
    }
}