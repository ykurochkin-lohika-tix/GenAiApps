using AskGenAi.Common.Services;
using AskGenAi.Core.Entities;
using FluentAssertions;
using AskGenAi.Core.Interfaces;
using AutoFixture;
using System.Text.Json.Serialization;
using AskGenAi.Core.Aggregators;

namespace AskGenAi.xTests.Common.Services;

public class JsonFileSerializerTests
{
    private readonly IJsonFileSerializer<TestEntity> _serializer = new JsonFileSerializer<TestEntity>();
    private const string FilePath = "test.json";
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task DeserializeAsync_ShouldReturnRootObject_WhenFileIsValid()
    {
        // Arrange
        var guid = _fixture.Create<Guid>();
        var json = "{\"version\":\"1.0\",\"data\":[{\"id\":\"" + guid + "\",\"name\":\"Test\"}]}";
        await File.WriteAllTextAsync(FilePath, json);

        // Act
        var result = await _serializer.DeserializeAsync(FilePath);

        // Assert
        result.Should().NotBeNull();
        result!.Version.Should().Be("1.0");
        result.Data.Should().HaveCount(1);
        result.Data[0].Id.Should().Be(guid);
        result.Data[0].Name.Should().Be("Test");
    }

    [Fact]
    public async Task SerializeAsync_ShouldWriteToFile_WhenObjectIsValid()
    {
        // Arrange
        var guid = _fixture.Create<Guid>();
        var root = new Root<TestEntity>
        {
            Version = "1.0",
            Data = [new TestEntity { Id = guid, Name = "Test" }]
        };

        // Act
        var result = await _serializer.SerializeAsync(root, FilePath);

        // Assert
        var fileContent = await File.ReadAllTextAsync(FilePath);
        fileContent.Should().Be(result);
        fileContent.Should().Contain("\"version\": \"1.0\"");
        fileContent.Should().Contain("\"id\": \"" + guid + "\"");
        fileContent.Should().Contain("\"name\": \"Test\"");
    }

    [Fact]
    public void Deserialize_ShouldReturnRootObject_WhenFileIsValid()
    {
        var guid = _fixture.Create<Guid>();
        var json = "{\"version\":\"1.0\",\"data\":[{\"id\":\"" + guid + "\",\"name\":\"Test\"}]}";
        File.WriteAllText(FilePath, json);

        // Act
        var result = _serializer.Deserialize(FilePath);

        // Assert
        result.Should().NotBeNull();
        result!.Version.Should().Be("1.0");
        result.Data.Should().HaveCount(1);
        result.Data[0].Id.Should().Be(guid);
        result.Data[0].Name.Should().Be("Test");
    }

    [Fact]
    public void Serialize_ShouldWriteToFile_WhenObjectIsValid()
    {
        // Arrange
        var guid = _fixture.Create<Guid>();
        var root = new Root<TestEntity>
        {
            Version = "1.0",
            Data = [new TestEntity() { Id = guid, Name = "Test" }]
        };

        // Act
        var result = _serializer.Serialize(root, FilePath);

        // Assert
        var fileContent = File.ReadAllText(FilePath);
        fileContent.Should().Be(result);
        fileContent.Should().Contain("\"version\": \"1.0\"");
        fileContent.Should().Contain("\"id\": \"" + guid + "\"");
        fileContent.Should().Contain("\"name\": \"Test\"");
    }
}

public class TestEntity : IEntity
{
    [JsonPropertyName("id")] public Guid Id { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; } = default!;
}