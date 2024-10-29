using AskGenAi.Core.Entities;
using AskGenAi.Infrastructure.Persistence;
using FluentAssertions;

namespace AskGenAi.xTests.Infrastructure.Persistence;

public class InMemoryRepositoryTests
{
    private readonly InMemoryRepository<TestEntity> _repository = new();

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllEntities()
    {
        // Arrange
        var entity1 = new TestEntity { Id = Guid.NewGuid(), Name = "Entity1" };
        var entity2 = new TestEntity { Id = Guid.NewGuid(), Name = "Entity2" };
        await _repository.AddAsync(entity1);
        await _repository.AddAsync(entity2);

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().Contain([entity1, entity2]);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnEntity_WhenEntityExists()
    {
        // Arrange
        var entity = new TestEntity { Id = Guid.NewGuid(), Name = "Entity" };
        await _repository.AddAsync(entity);

        // Act
        var result = await _repository.GetByIdAsync(entity.Id);

        // Assert
        result.Should().Be(entity);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenEntityDoesNotExist()
    {
        // Act
        var result = await _repository.GetByIdAsync(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_ShouldAddEntity()
    {
        // Arrange
        var entity = new TestEntity { Id = Guid.NewGuid(), Name = "Entity" };

        // Act
        await _repository.AddAsync(entity);
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().Contain(entity);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity_WhenEntityExists()
    {
        // Arrange
        var entity = new TestEntity { Id = Guid.NewGuid(), Name = "Entity" };
        await _repository.AddAsync(entity);
        var updatedEntity = new TestEntity { Id = entity.Id, Name = "Updated Entity" };

        // Act
        await _repository.UpdateAsync(updatedEntity);
        var result = await _repository.GetByIdAsync(entity.Id);

        // Assert
        result.Should().Be(updatedEntity);
    }

    [Fact]
    public async Task UpdateAsync_ShouldNotUpdateEntity_WhenEntityDoesNotExist()
    {
        // Arrange
        var entity = new TestEntity { Id = Guid.NewGuid(), Name = "Entity" };

        // Act
        await _repository.UpdateAsync(entity);
        var result = await _repository.GetByIdAsync(entity.Id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveEntity_WhenEntityExists()
    {
        // Arrange
        var entity = new TestEntity { Id = Guid.NewGuid(), Name = "Entity" };
        await _repository.AddAsync(entity);

        // Act
        await _repository.DeleteAsync(entity.Id);
        var result = await _repository.GetByIdAsync(entity.Id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_ShouldNotThrow_WhenEntityDoesNotExist()
    {
        // Act
        var act = async () => await _repository.DeleteAsync(Guid.NewGuid());

        // Assert
        await act.Should().NotThrowAsync();
    }
}

public class TestEntity : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}