using AskGenAi.Application.UseCases;
using AskGenAi.Core.Aggregators;
using AskGenAi.Core.Interfaces;
using AskGenAi.Core.Models;
using Moq;

namespace AskGenAi.xTests.Application.UseCases;

public class ClassNormalizerServiceTests
{
    private readonly Mock<IFilePath> _mockFilePath;
    private readonly Mock<IJsonFileSerializer<DisciplineOnPremises>> _mockDisciplineFileSerializer;
    private readonly Mock<IJsonFileSerializer<QuestionOnPremises>> _mockQuestionFileSerializer;
    private readonly ClassNormalizerService _classNormalizerService;

    public ClassNormalizerServiceTests()
    {
        Mock<IOnPremisesRepository<QuestionOnPremises>> mockQuestionRepository = new();
        _mockFilePath = new Mock<IFilePath>();
        _mockDisciplineFileSerializer = new Mock<IJsonFileSerializer<DisciplineOnPremises>>();
        _mockQuestionFileSerializer = new Mock<IJsonFileSerializer<QuestionOnPremises>>();

        _classNormalizerService = new ClassNormalizerService(
            mockQuestionRepository.Object,
            _mockFilePath.Object,
            _mockDisciplineFileSerializer.Object,
            _mockQuestionFileSerializer.Object);
    }

    [Fact]
    public async Task NormalizeDisciplineAsync_ShouldNormalizeAndSaveDisciplines()
    {
        // Arrange
        var disciplines = new List<DisciplineOnPremises> { new() { Id = Guid.Empty } };
        var root = new Root<DisciplineOnPremises> { Data = disciplines, Version = "1.0.0" };
        _mockFilePath.Setup(x => x.GetLocalDisciplinePath()).Returns("disciplinePath");
        _mockDisciplineFileSerializer.Setup(x => x.DeserializeAsync("disciplinePath")).ReturnsAsync(root);

        // Act
        await _classNormalizerService.NormalizeDisciplineAsync();

        // Assert
        _mockDisciplineFileSerializer.Verify(
            x => x.SerializeAsync(It.Is<Root<DisciplineOnPremises>>(r => r.Data.TrueForAll(d => d.Id != Guid.Empty)),
                It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task NormalizeQuestionAsync_ShouldNormalizeAndSaveQuestions()
    {
        // Arrange
        var questions = new List<QuestionOnPremises> { new() { Id = Guid.Empty } };
        var root = new Root<QuestionOnPremises> { Data = questions, Version = "1.0.0" };
        _mockFilePath.Setup(x => x.GetLocalQuestionsPath()).Returns("questionsPath");
        _mockQuestionFileSerializer.Setup(x => x.DeserializeAsync("questionsPath")).ReturnsAsync(root);

        // Act
        await _classNormalizerService.NormalizeQuestionAsync();

        // Assert
        _mockQuestionFileSerializer.Verify(
            x => x.SerializeAsync(It.Is<Root<QuestionOnPremises>>(r => r.Data.TrueForAll(q => q.Id != Guid.Empty)),
                It.IsAny<string>()),
            Times.Once);
    }

    [Fact]
    public async Task NormalizeQuestionsAsync_ShouldNormalizeAndSaveAllQuestions()
    {
        // Arrange
        var questions = new List<QuestionOnPremises> { new() { Id = Guid.Empty } };
        var root = new Root<QuestionOnPremises> { Data = questions, Version = "1.0.0" };
        _mockFilePath.Setup(x => x.GetQuestionsListFilename()).Returns(["questionsFile1", "questionsFile2"]);
        _mockQuestionFileSerializer.Setup(x => x.DeserializeAsync(It.IsAny<string>())).ReturnsAsync(root);

        // Act
        await _classNormalizerService.NormalizeQuestionsAsync();

        // Assert
        _mockQuestionFileSerializer.Verify(
            x => x.SerializeAsync(It.Is<Root<QuestionOnPremises>>(r => r.Data.TrueForAll(q => q.Id != Guid.Empty)),
                It.IsAny<string>()),
            Times.Once);
    }
}