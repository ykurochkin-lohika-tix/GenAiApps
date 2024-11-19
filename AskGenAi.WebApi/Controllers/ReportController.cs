using AskGenAi.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AskGenAi.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportController(IReportGeneratorHandler reportGeneratorHandler) : ControllerBase
{
    [HttpGet("generate-docx")]
    public async Task<IActionResult> GenerateDocxReport([FromQuery] List<Guid>? disciplineIds)
    {
        if (disciplineIds == null || disciplineIds.Count == 0)
        {
            return BadRequest("Discipline IDs are required.");
        }

        var stream = await reportGeneratorHandler.GenerateWebDocxReportAsync(disciplineIds[0]);

        if (stream != null)
            return File(stream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                "report.docx");

        return NotFound();
    }

    [HttpGet("generate-txt")]
    public async Task<IActionResult> GenerateTxtReport([FromQuery] List<Guid>? disciplineIds)
    {
        if (disciplineIds == null || disciplineIds.Count == 0)
        {
            return BadRequest("Discipline IDs are required.");
        }

        var stream = await reportGeneratorHandler.GenerateWebTextFilesReportAsync(disciplineIds[0]);

        if (stream != null)
            return File(stream, "text/plain", "report.txt");

        return NotFound();
    }

    [HttpGet("generate-md")]
    public async Task<IActionResult> GenerateMdReport([FromQuery] List<Guid>? disciplineIds)
    {
        if (disciplineIds == null || disciplineIds.Count == 0)
        {
            return BadRequest("Discipline IDs are required.");
        }

        var stream = await reportGeneratorHandler.GenerateWebTextFilesReportAsync(disciplineIds[0]);

        if (stream != null)
            return File(stream, "text/plain", "report.md");

        return NotFound();
    }
}