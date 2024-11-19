using AskGenAi.Application.UseCases;
using AskGenAi.Core.Entities;
using AskGenAi.Core.Interfaces;
using Moq;

namespace AskGenAi.xTests.Application.UseCases;

public class ReportGeneratorHandlerTests
{
    private readonly Mock<IReportGenerator> _mockReportGenerator;
    private readonly Mock<IRepository<Discipline>> _mockDisciplineRepository;
    private readonly Mock<IFilePath> _mockFilePath;
    private readonly ReportGeneratorHandler _reportGeneratorHandler;

    public ReportGeneratorHandlerTests()
    {
        _mockReportGenerator = new Mock<IReportGenerator>();
        _mockDisciplineRepository = new Mock<IRepository<Discipline>>();
        _mockFilePath = new Mock<IFilePath>();

        _reportGeneratorHandler = new ReportGeneratorHandler(
            _mockReportGenerator.Object,
            _mockDisciplineRepository.Object,
            _mockFilePath.Object);
    }

    [Fact]
    public async Task GenerateAllDocxReportAsync_ShouldGenerateDocxReport()
    {
        // Arrange
        var disciplineIds = new[] { Guid.NewGuid(), Guid.NewGuid() };
        _mockDisciplineRepository.Setup(repo => repo.GetAllNoTrackAsync(null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(disciplineIds.Select(id => new Discipline { Id = id }));

        var outputPath = "test.docx";
        _mockFilePath.Setup(fp => fp.GetFullReportPath("docx")).Returns(outputPath);

        // Act
        await _reportGeneratorHandler.GenerateAllDocxReportAsync();

        // Assert
        _mockReportGenerator.Verify(rg => rg.GenerateDocxReportAsync(disciplineIds, outputPath), Times.Once);
    }

    [Fact]
    public async Task GenerateWebAllDocxReportAsync_ShouldReturnStream()
    {
        // Arrange
        var disciplineIds = new[] { Guid.NewGuid(), Guid.NewGuid() };
        _mockDisciplineRepository.Setup(repo => repo.GetAllNoTrackAsync(null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(disciplineIds.Select(id => new Discipline { Id = id }));

        var stream = new MemoryStream();
        _mockReportGenerator.Setup(rg => rg.GenerateDocxReportAsync(disciplineIds))
            .ReturnsAsync(stream);

        // Act
        var result = await _reportGeneratorHandler.GenerateWebAllDocxReportAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(stream, result);
    }

    [Fact]
    public async Task GenerateAllTxtReportAsync_ShouldGenerateTxtReport()
    {
        // Arrange
        var disciplineIds = new[] { Guid.NewGuid(), Guid.NewGuid() };
        _mockDisciplineRepository.Setup(repo => repo.GetAllNoTrackAsync(null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(disciplineIds.Select(id => new Discipline { Id = id }));

        var outputPath = "test.txt";
        _mockFilePath.Setup(fp => fp.GetFullReportPath("txt")).Returns(outputPath);

        // Act
        await _reportGeneratorHandler.GenerateAllTxtReportAsync();

        // Assert
        _mockReportGenerator.Verify(rg => rg.GenerateTextFilesReportAsync(disciplineIds, outputPath), Times.Once);
    }

    [Fact]
    public async Task GenerateWebAllTxtReportAsync_ShouldReturnStream()
    {
        // Arrange
        var disciplineIds = new[] { Guid.NewGuid(), Guid.NewGuid() };
        _mockDisciplineRepository.Setup(repo => repo.GetAllNoTrackAsync(null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(disciplineIds.Select(id => new Discipline { Id = id }));

        var stream = new MemoryStream();
        _mockReportGenerator.Setup(rg => rg.GenerateTextFilesReportAsync(disciplineIds))
            .ReturnsAsync(stream);

        // Act
        var result = await _reportGeneratorHandler.GenerateWebAllTxtReportAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(stream, result);
    }

    [Fact]
    public async Task GenerateDocxReportAsync_ShouldGenerateDocxReportForDiscipline()
    {
        // Arrange
        var disciplineId = Guid.NewGuid();
        var discipline = new Discipline
        {
            Id = disciplineId,
            Title = "Test Discipline",
            Subtitle = "Test Subtitle"
        };

        _mockDisciplineRepository.Setup(repo => repo.GetByIdAsync(disciplineId))
            .ReturnsAsync(discipline);

        var outputPath = "test.docx";
        _mockFilePath.Setup(fp => fp.GetReportPath(It.IsAny<string>(), "docx")).Returns(outputPath);

        // Act
        await _reportGeneratorHandler.GenerateDocxReportAsync(disciplineId);

        // Assert
        _mockReportGenerator.Verify(rg => rg.GenerateDocxReportAsync(new[] { disciplineId }, outputPath), Times.Once);
    }

    [Fact]
    public async Task GenerateWebDocxReportAsync_ShouldReturnStreamForDiscipline()
    {
        // Arrange
        var disciplineId = Guid.NewGuid();
        var discipline = new Discipline
        {
            Id = disciplineId,
            Title = "Test Discipline",
            Subtitle = "Test Subtitle"
        };

        _mockDisciplineRepository.Setup(repo => repo.GetByIdAsync(disciplineId))
            .ReturnsAsync(discipline);

        var stream = new MemoryStream();
        _mockReportGenerator.Setup(rg => rg.GenerateDocxReportAsync(new[] { disciplineId }))
            .ReturnsAsync(stream);

        // Act
        var result = await _reportGeneratorHandler.GenerateWebDocxReportAsync(disciplineId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(stream, result);
    }

    [Fact]
    public async Task GenerateTextFilesReportAsync_ShouldGenerateTxtReportForDiscipline()
    {
        // Arrange
        var disciplineId = Guid.NewGuid();
        var discipline = new Discipline
        {
            Id = disciplineId,
            Title = "Test Discipline",
            Subtitle = "Test Subtitle"
        };

        _mockDisciplineRepository.Setup(repo => repo.GetByIdAsync(disciplineId))
            .ReturnsAsync(discipline);

        var outputPath = "test.txt";
        _mockFilePath.Setup(fp => fp.GetReportPath(It.IsAny<string>(), "txt")).Returns(outputPath);

        // Act
        await _reportGeneratorHandler.GenerateTextFilesReportAsync(disciplineId, "txt");

        // Assert
        _mockReportGenerator.Verify(rg => rg.GenerateTextFilesReportAsync(new[] { disciplineId }, outputPath),
            Times.Once);
    }

    [Fact]
    public async Task GenerateWebTextFilesReportAsync_ShouldReturnStreamForDiscipline()
    {
        // Arrange
        var disciplineId = Guid.NewGuid();
        var discipline = new Discipline
        {
            Id = disciplineId,
            Title = "Test Discipline",
            Subtitle = "Test Subtitle"
        };

        _mockDisciplineRepository.Setup(repo => repo.GetByIdAsync(disciplineId))
            .ReturnsAsync(discipline);

        var stream = new MemoryStream();
        _mockReportGenerator.Setup(rg => rg.GenerateTextFilesReportAsync(new[] { disciplineId }))
            .ReturnsAsync(stream);

        // Act
        var result = await _reportGeneratorHandler.GenerateWebTextFilesReportAsync(disciplineId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(stream, result);
    }
}