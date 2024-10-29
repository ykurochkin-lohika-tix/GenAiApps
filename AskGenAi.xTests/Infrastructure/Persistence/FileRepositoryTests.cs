using AskGenAi.Core.Entities;
using AskGenAi.Core.Interfaces;
using AskGenAi.Infrastructure.Persistence;
using AutoFixture;
using Moq;

namespace AskGenAi.xTests.Infrastructure.Persistence;

public class FileRepositoryTests
{
    private const string TestFilePath = "test.json";

    private readonly Mock<IJsonFileSerializer<Common.Services.TestEntity>> _mockJsonFileSerializer;
    private readonly FileRepository<Common.Services.TestEntity> _fileRepository;
    private readonly Fixture _fixture = new();
    private readonly List<Common.Services.TestEntity> _entities;
    private readonly Common.Services.TestEntity _entity;
    private readonly Guid _entityId;

    public FileRepositoryTests()
    {
        _mockJsonFileSerializer = new Mock<IJsonFileSerializer<Common.Services.TestEntity>>();
        Mock<IFilePath> mockFilePathService = new();
        mockFilePathService.Setup(x => x.GetLocalFullPathByType(It.IsAny<Type>())).Returns(TestFilePath);

        _entityId = _fixture.Create<Guid>();
        _entity = new Common.Services.TestEntity { Id = _entityId };
        _entities = new List<Common.Services.TestEntity> { _entity };
        _mockJsonFileSerializer.Setup(x => x.Deserialize(TestFilePath))
            .Returns(new Root<Common.Services.TestEntity> { Data = _entities });

        _fileRepository =
            new FileRepository<Common.Services.TestEntity>(_mockJsonFileSerializer.Object, mockFilePathService.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllEntities()
    {
        // Act
        var result = await _fileRepository.GetAllAsync();

        // Assert
        Assert.Equal(_entities, result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnEntity_WhenEntityExists()
    {
        // Act
        var result = await _fileRepository.GetByIdAsync(_entityId);

        // Assert
        Assert.Equal(_entityId, result?.Id);
    }

    [Fact]
    public async Task AddAsync_ShouldAddEntity()
    {
        // Arrange
        var testEntity = new Common.Services.TestEntity { Id = Guid.NewGuid() };

        // Act
        await _fileRepository.AddAsync(testEntity);

        // Assert
        _mockJsonFileSerializer.Verify(
            x => x.SerializeAsync(It.Is<Root<Common.Services.TestEntity>>(r => r.Data.Contains(testEntity)),
                TestFilePath), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity_WhenEntityExists()
    {
        // Arrange
        var updatedEntity = new Common.Services.TestEntity { Id = _entityId, Name = "Updated" };

        // Act
        await _fileRepository.UpdateAsync(updatedEntity);

        // Assert
        _mockJsonFileSerializer.Verify(
            x => x.SerializeAsync(
                It.Is<Root<Common.Services.TestEntity>>(
                    r => r.Data.Contains(updatedEntity) && !r.Data.Contains(_entity)),
                TestFilePath), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveEntity_WhenEntityExists()
    {
        // Act
        await _fileRepository.DeleteAsync(_entityId);

        // Assert
        _mockJsonFileSerializer.Verify(
            x => x.SerializeAsync(It.Is<Root<Common.Services.TestEntity>>(r => !r.Data.Contains(_entity)),
                TestFilePath), Times.Once);
    }
}