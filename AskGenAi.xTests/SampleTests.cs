using AutoFixture;
using FluentAssertions;

namespace AskGenAi.xTests;

public class SampleTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void ExampleTest_ShouldPass_WhenConditionIsMet()
    {
        // Arrange
        var expectedValue = _fixture.Create<int>();

        // Act
        var actualValue = expectedValue;

        // Assert
        actualValue.Should().Be(expectedValue);
    }
}