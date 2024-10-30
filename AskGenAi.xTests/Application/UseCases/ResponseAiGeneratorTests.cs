using AskGenAi.Application.UseCases;
using AskGenAi.Core.Entities;
using AskGenAi.Core.Interfaces;
using Moq;
using AutoFixture;

namespace AskGenAi.xTests.Application.UseCases;

public class ResponseAiGeneratorTests
{
    private readonly Fixture _fixture = new();
    private readonly Mock<IChatModelManager> _mockChatModelManager;
    private readonly Mock<IHistoryBuilder> _mockHistoryBuilder;
    private readonly Mock<IOnPremisesRepository<Discipline>> _mockDisciplineRepository;
    private readonly Mock<IOnPremisesRepository<Question>> _mockQuestionRepository;
    private readonly Mock<IOnPremisesRepository<Response>> _mockResponseRepository;
    private readonly ResponseAiGenerator _responseAiGenerator;

    public ResponseAiGeneratorTests()
    {
        _mockChatModelManager = new Mock<IChatModelManager>();
        _mockHistoryBuilder = new Mock<IHistoryBuilder>();
        _mockDisciplineRepository = new Mock<IOnPremisesRepository<Discipline>>();
        _mockQuestionRepository = new Mock<IOnPremisesRepository<Question>>();
        _mockResponseRepository = new Mock<IOnPremisesRepository<Response>>();

        _responseAiGenerator = new ResponseAiGenerator(
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
        var disciplines = new List<Discipline>
        {
            new()
            {
                Id = _fixture.Create<Guid>(), Type = 1, Title = "Discipline1", Scope = "Scope1", Subtitle = "Subtitle1"
            }
        };
        var questions = new List<Question>
        {
            new() { Id = _fixture.Create<Guid>(), DisciplineType = 1, Context = "Question1" }
        };
        var responses = Array.Empty<Response>();

        _mockDisciplineRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(disciplines);
        _mockQuestionRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(questions);
        _mockResponseRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(responses);
        _mockHistoryBuilder
            .Setup(x => x.BuildQuestionHistory(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>())).Returns("HistoryMessage");
        _mockChatModelManager.Setup(x => x.GetChatMessageContentAsync()).ReturnsAsync("Response1");

        // Act
        await _responseAiGenerator.RunAsync();

        // Assert
        _mockChatModelManager.Verify(x => x.AddSystemMessage("HistoryMessage"), Times.Once);
        _mockChatModelManager.Verify(x => x.AddUserMessage("Question1"), Times.Once);
        _mockResponseRepository.Verify(
            x => x.AddAsync(It.Is<Response>(r => r.Context == "Response1" && r.QuestionId == questions[0].Id)),
            Times.Once);
    }

    [Fact]
    public async Task RunAsync_ShouldSkipExistingResponses()
    {
        // Arrange
        var disciplines = new List<Discipline>
        {
            new()
            {
                Id = _fixture.Create<Guid>(), Type = 1, Title = "Discipline1", Scope = "Scope1", Subtitle = "Subtitle1"
            }
        };
        var questions = new List<Question>
        {
            new() { Id = _fixture.Create<Guid>(), DisciplineType = 1, Context = "Question1" }
        };
        var responses = new List<Response>
        {
            new() { Id = _fixture.Create<Guid>(), QuestionId = questions[0].Id, Context = "ExistingResponse" }
        };

        _mockDisciplineRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(disciplines);
        _mockQuestionRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(questions);
        _mockResponseRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(responses);

        // Act
        await _responseAiGenerator.RunAsync();

        // Assert
        _mockChatModelManager.Verify(x => x.AddUserMessage(It.IsAny<string>()), Times.Never);
        _mockResponseRepository.Verify(x => x.AddAsync(It.IsAny<Response>()), Times.Never);
    }
}