using AskGenAi.Core.Entities;
using AskGenAi.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace AskGenAi.xTests.Infrastructure.ReportGenerator;

public class ReportGeneratorTests
{
    private readonly Mock<IRepository<Discipline>> _mockDisciplineRepository;
    private readonly Mock<IFileSystem> _mockFileSystem;
    private readonly AskGenAi.Infrastructure.ReportGenerator.ReportGenerator _reportGenerator;

    public ReportGeneratorTests()
    {
        _mockDisciplineRepository = new Mock<IRepository<Discipline>>();
        _mockFileSystem = new Mock<IFileSystem>();
        Mock<ILogger<AskGenAi.Infrastructure.ReportGenerator.ReportGenerator>> mockLogger = new();

        _reportGenerator = new AskGenAi.Infrastructure.ReportGenerator.ReportGenerator(
            _mockDisciplineRepository.Object,
            _mockFileSystem.Object,
            mockLogger.Object);
    }

    [Fact]
    public async Task GenerateDocxReportAsync_ShouldGenerateDocxReport()
    {
        // Arrange
        var disciplineId = Guid.NewGuid();
        var discipline = new Discipline
        {
            Id = disciplineId,
            Title = "Test Discipline",
            Subtitle = "Test Subtitle",
            Scope = "Test Scope",
            Questions = new List<Question>
            {
                new Question
                {
                    Id = Guid.NewGuid(),
                    Context = "Test Question",
                    Responses = new List<Response>
                    {
                        new Response { Context = "Test Response" }
                    }
                }
            }
        };

        _mockDisciplineRepository.Setup(repo => repo.GetByIdAsync(disciplineId))
            .ReturnsAsync(discipline);

        var outputPath = "test.docx";

        // Act
        await _reportGenerator.GenerateDocxReportAsync(new[] { disciplineId }, outputPath);

        // Assert
        _mockFileSystem.Verify(fs => fs.WriteAllTextAsync(outputPath, It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GenerateDocxReportAsync_ShouldReturnStream()
    {
        // Arrange
        var disciplineId = Guid.NewGuid();
        var discipline = new Discipline
        {
            Id = disciplineId,
            Title = "Test Discipline",
            Subtitle = "Test Subtitle",
            Scope = "Test Scope",
            Questions = new List<Question>
            {
                new Question
                {
                    Id = Guid.NewGuid(),
                    Context = "Test Question",
                    Responses = new List<Response>
                    {
                        new Response { Context = "Test Response" }
                    }
                }
            }
        };

        _mockDisciplineRepository.Setup(repo => repo.GetByIdAsync(disciplineId))
            .ReturnsAsync(discipline);

        // Act
        var stream = await _reportGenerator.GenerateDocxReportAsync(new[] { disciplineId });

        // Assert
        Assert.NotNull(stream);
        Assert.True(stream.Length > 0);
    }

    [Fact]
    public async Task GenerateTextFilesReportAsync_ShouldGenerateTextReport()
    {
        // Arrange
        var disciplineId = Guid.NewGuid();
        var discipline = new Discipline
        {
            Id = disciplineId,
            Title = "Test Discipline",
            Subtitle = "Test Subtitle",
            Scope = "Test Scope",
            Questions = new List<Question>
            {
                new Question
                {
                    Id = Guid.NewGuid(),
                    Context = "Test Question",
                    Responses = new List<Response>
                    {
                        new Response { Context = "Test Response" }
                    }
                }
            }
        };

        _mockDisciplineRepository.Setup(repo => repo.GetByIdAsync(disciplineId))
            .ReturnsAsync(discipline);

        var outputPath = "test.txt";

        // Act
        await _reportGenerator.GenerateTextFilesReportAsync(new[] { disciplineId }, outputPath);

        // Assert
        _mockFileSystem.Verify(fs => fs.WriteAllTextAsync(outputPath, It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GenerateTextFilesReportAsync_ShouldReturnStream()
    {
        // Arrange
        var disciplineId = Guid.NewGuid();
        var discipline = new Discipline
        {
            Id = disciplineId,
            Title = "Test Discipline",
            Subtitle = "Test Subtitle",
            Scope = "Test Scope",
            Questions = new List<Question>
            {
                new Question
                {
                    Id = Guid.NewGuid(),
                    Context = "Test Question",
                    Responses = new List<Response>
                    {
                        new Response { Context = "Test Response" }
                    }
                }
            }
        };

        _mockDisciplineRepository.Setup(repo => repo.GetByIdAsync(disciplineId))
            .ReturnsAsync(discipline);

        // Act
        var stream = await _reportGenerator.GenerateTextFilesReportAsync(new[] { disciplineId });

        // Assert
        Assert.NotNull(stream);
        Assert.True(stream.Length > 0);
    }
}