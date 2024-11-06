using System.Linq.Expressions;
using AskGenAi.Application.UseCases;
using AskGenAi.Core.Entities;
using AskGenAi.Core.Enums;
using AskGenAi.Core.Interfaces;
using Moq;

namespace AskGenAi.xTests.Application.UseCases;

public class ResponseAiGeneratorTests
{
    private readonly Mock<IChatModelManager> _mockChatModelManager;
    private readonly Mock<IHistoryBuilder> _mockHistoryBuilder;
    private readonly Mock<IRepository<Discipline>> _mockDisciplineRepository;
    private readonly Mock<IRepository<Response>> _mockResponseRepository;
    private readonly Mock<IRepository<Question>> _mockQuestionRepository;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly ResponseAiGenerator _responseAiGenerator;

    public ResponseAiGeneratorTests()
    {
        _mockChatModelManager = new Mock<IChatModelManager>();
        _mockHistoryBuilder = new Mock<IHistoryBuilder>();
        _mockDisciplineRepository = new Mock<IRepository<Discipline>>();
        _mockResponseRepository = new Mock<IRepository<Response>>();
        _mockQuestionRepository = new Mock<IRepository<Question>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _mockDisciplineRepository.Setup(repo => repo.UnitOfWork).Returns(_unitOfWorkMock.Object);
        _mockResponseRepository.Setup(repo => repo.UnitOfWork).Returns(_unitOfWorkMock.Object);
        _mockQuestionRepository.Setup(repo => repo.UnitOfWork).Returns(_unitOfWorkMock.Object);

        _responseAiGenerator = new ResponseAiGenerator(
            _mockChatModelManager.Object,
            _mockHistoryBuilder.Object,
            _mockDisciplineRepository.Object,
            _mockResponseRepository.Object,
            _mockQuestionRepository.Object,
            TimeSpan.FromSeconds(1)
        );
    }

    [Fact]
    public async Task RunForAllWithoutResponseAsync_ShouldSkipNullDisciplines()
    {
        // Arrange
        var questions = new List<Question>
        {
            new() { Id = Guid.NewGuid(), DisciplineId = Guid.NewGuid() }
        };

        _mockQuestionRepository
            .Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Question, bool>>>(), CancellationToken.None))
            .ReturnsAsync(questions);

        _mockDisciplineRepository
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Discipline)null!);

        // Act
        await _responseAiGenerator.RunForAllWithoutResponseAsync();

        // Assert
        _mockChatModelManager.Verify(mgr => mgr.AddSystemMessage(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task RunForAllWithoutResponseAsync_ShouldGenerateResponses()
    {
        // Arrange
        var disciplineId = Guid.NewGuid();
        var questions = new List<Question>
        {
            new() { Id = Guid.NewGuid(), DisciplineId = disciplineId, Context = "Question 1" }
        };

        var discipline = new Discipline
        {
            Id = disciplineId,
            Type = DisciplineType.NetRuntime,
            Title = "Title",
            Subtitle = "Subtitle",
            Scope = "Scope"
        };

        _mockQuestionRepository
            .Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Question, bool>>>(), CancellationToken.None))
            .ReturnsAsync(questions);

        _mockDisciplineRepository
            .Setup(repo => repo.GetByIdAsync(disciplineId))
            .ReturnsAsync(discipline);

        _mockHistoryBuilder
            .Setup(builder => builder.BuildQuestionHistory(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>()))
            .Returns("History");

        _mockChatModelManager
            .Setup(mgr => mgr.GetChatMessageContentAsync())
            .ReturnsAsync("Response");

        // Act
        await _responseAiGenerator.RunForAllWithoutResponseAsync();

        // Assert
        _mockChatModelManager.Verify(mgr => mgr.AddSystemMessage("History"), Times.Once);
        _mockChatModelManager.Verify(mgr => mgr.AddUserMessage("Question 1"), Times.Once);
        _mockResponseRepository.Verify(repo => repo.AddRangeAsync(CancellationToken.None, It.IsAny<Response[]>()),
            Times.Once);
        _unitOfWorkMock.Verify(repo => repo.SaveChangesAsync(default), Times.Once);
    }
}