using AskGenAi.Core.Models;
using AskGenAi.Infrastructure.FileSystem;
using FluentAssertions;

namespace AskGenAi.xTests.Infrastructure.FileSystem;

public class FilePathTests
{
    private readonly FilePath _filePath = new();

    [Fact]
    public void GetQuestionsListFilename_ShouldReturnCorrectFilenames()
    {
        // Act
        var result = _filePath.GetQuestionsListFilename();

        // Assert
        result.Should().Contain(
        [
            "questions1", "questions2", "questions3", "questions4",
            "questions11", "questions12", "questions13", "questions14",
            "questions21", "questions22", "questions23",
            "questions31", "questions32"
        ]);
    }

    [Fact]
    public void GetLocalQuestionsFullPath_ShouldReturnCorrectPath()
    {
        // Act
        var result = _filePath.GetLocalQuestionsFullPath();

        // Assert
        result.Should().EndWith("AskGenAi.Infrastructure/Resources/question.json");
    }

    [Fact]
    public void GetLocalFullPathByType_ShouldReturnCorrectPath_ForQuestionType()
    {
        // Act
        var result = _filePath.GetLocalFullPathByType(typeof(QuestionOnPremises));

        // Assert
        result.Should().EndWith("AskGenAi.Infrastructure/Resources/question.json");
    }

    [Fact]
    public void GetLocalFullPathByType_ShouldReturnCorrectPath_ForDisciplineType()
    {
        // Act
        var result = _filePath.GetLocalFullPathByType(typeof(DisciplineOnPremises));

        // Assert
        result.Should().EndWith("AskGenAi.Infrastructure/Resources/disciplineV1.json");
    }

    [Fact]
    public void GetLocalFullPathByType_ShouldReturnCorrectPath_ForResponseType()
    {
        // Act
        var result = _filePath.GetLocalFullPathByType(typeof(ResponseOnPremises));

        // Assert
        result.Should().EndWith("AskGenAi.Infrastructure/Resources/response.json");
    }

    [Fact]
    public void GetLocalFullPathByType_ShouldReturnEmptyString_ForUnknownType()
    {
        // Act
        var result = _filePath.GetLocalFullPathByType(typeof(string));

        // Assert
        result.Should().BeEmpty();
    }
}