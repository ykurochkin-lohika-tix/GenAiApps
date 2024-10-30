using AskGenAi.Application.Services;
using AskGenAi.Core.Enums;
using FluentAssertions;

namespace AskGenAi.xTests.Application.Services;

public class HistoryBuilderTests
{
    private readonly HistoryBuilder _historyBuilder = new();

    [Fact]
    public void BuildQuestionHistory_ShouldReturnEmptyString_WhenRequiredParametersAreNullOrWhitespace()
    {
        // Act
        var result = _historyBuilder.BuildQuestionHistory(null, "tech", "discipline", "subDiscipline");

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void BuildQuestionHistory_ShouldReturnFormattedString_WhenAllParametersAreProvided()
    {
        // Arrange
        var personality = PersonalityHelper.GetPersonality(DisciplineType.NetRuntime);
        const string technologies = "C#, .NET";
        const string discipline = "Software Engineering";
        const string subDiscipline = "Backend Development";

        // Act
        var result = _historyBuilder.BuildQuestionHistory(personality, technologies, discipline, subDiscipline);

        // Assert
        result.Should().Contain(personality);
        result.Should().Contain(technologies);
        result.Should().Contain(discipline);
        result.Should().Contain(subDiscipline);
    }

    [Fact]
    public void BuildQuestionHistory_ShouldReturnFormattedString_WhenSubDisciplineIsNull()
    {
        // Arrange
        var personality = PersonalityHelper.GetPersonality(DisciplineType.DatabasesEventProcessing);
        const string technologies = "C#, .NET";
        const string discipline = "Software Engineering";

        // Act
        var result = _historyBuilder.BuildQuestionHistory(personality, technologies, discipline, null);

        // Assert
        result.Should().Contain(personality);
        result.Should().Contain(technologies);
        result.Should().Contain(discipline);
        result.Should().NotContain("null");
    }
}