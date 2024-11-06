using AskGenAi.Application.UseCases;
using AskGenAi.Core.Interfaces;
using Moq;
using AutoFixture;
using AskGenAi.Core.Models;

namespace AskGenAi.xTests.Application.UseCases;

public class LocalResponseAiGeneratorTests
{
    private readonly Fixture _fixture = new();
    private readonly Mock<IChatModelManager> _mockChatModelManager;
    private readonly Mock<IHistoryBuilder> _mockHistoryBuilder;
    private readonly Mock<IOnPremisesRepository<DisciplineOnPremises>> _mockDisciplineRepository;
    private readonly Mock<IOnPremisesRepository<QuestionOnPremises>> _mockQuestionRepository;
    private readonly Mock<IOnPremisesRepository<ResponseOnPremises>> _mockResponseRepository;
    private readonly LocalResponseAiGenerator _localResponseAiGenerator;

    public LocalResponseAiGeneratorTests()
    {
        _mockChatModelManager = new Mock<IChatModelManager>();
        _mockHistoryBuilder = new Mock<IHistoryBuilder>();
        _mockDisciplineRepository = new Mock<IOnPremisesRepository<DisciplineOnPremises>>();
        _mockQuestionRepository = new Mock<IOnPremisesRepository<QuestionOnPremises>>();
        _mockResponseRepository = new Mock<IOnPremisesRepository<ResponseOnPremises>>();

        _localResponseAiGenerator = new LocalResponseAiGenerator(
            _mockChatModelManager.Object,
            _mockHistoryBuilder.Object,
            _mockDisciplineRepository.Object,
            _mockQuestionRepository.Object,
            _mockResponseRepository.Object,
            TimeSpan.Zero);
    }

    [Fact]
    public async Task RunAsync_ShouldGenerateResponsesForQuestions()
    {
        // Arrange
        var disciplines = new List<DisciplineOnPremises>
        {
            new()
            {
                Id = _fixture.Create<Guid>(), Type = 1, Title = "Discipline1", Scope = "Scope1", Subtitle = "Subtitle1"
            }
        };
        var questions = new List<QuestionOnPremises>
        {
            new() { Id = _fixture.Create<Guid>(), DisciplineType = 1, Context = "Question1" }
        };
        var responses = Array.Empty<ResponseOnPremises>();

        _mockDisciplineRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(disciplines);
        _mockQuestionRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(questions);
        _mockResponseRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(responses);
        _mockHistoryBuilder
            .Setup(x => x.BuildQuestionHistory(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>())).Returns("HistoryMessage");
        _mockChatModelManager.Setup(x => x.GetChatMessageContentAsync()).ReturnsAsync("Response1");

        // Act
        await _localResponseAiGenerator.RunForAllAsync();

        // Assert
        _mockChatModelManager.Verify(x => x.AddSystemMessage("HistoryMessage"), Times.Once);
        _mockChatModelManager.Verify(x => x.AddUserMessage("Question1"), Times.Once);
        _mockResponseRepository.Verify(
            x => x.AddAsync(It.Is<ResponseOnPremises>(r => r.Context == "Response1" && r.QuestionId == questions[0].Id)),
            Times.Once);
    }

    [Fact]
    public async Task RunAsync_ShouldSkipExistingResponses()
    {
        // Arrange
        var disciplines = new List<DisciplineOnPremises>
        {
            new()
            {
                Id = _fixture.Create<Guid>(), Type = 1, Title = "Discipline1", Scope = "Scope1", Subtitle = "Subtitle1"
            }
        };
        var questions = new List<QuestionOnPremises>
        {
            new() { Id = _fixture.Create<Guid>(), DisciplineType = 1, Context = "Question1" }
        };
        var responses = new List<ResponseOnPremises>
        {
            new() { Id = _fixture.Create<Guid>(), QuestionId = questions[0].Id, Context = "ExistingResponse" }
        };

        _mockDisciplineRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(disciplines);
        _mockQuestionRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(questions);
        _mockResponseRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(responses);

        // Act
        await _localResponseAiGenerator.RunForAllAsync();

        // Assert
        _mockChatModelManager.Verify(x => x.AddUserMessage(It.IsAny<string>()), Times.Never);
        _mockResponseRepository.Verify(x => x.AddAsync(It.IsAny<ResponseOnPremises>()), Times.Never);
    }
}