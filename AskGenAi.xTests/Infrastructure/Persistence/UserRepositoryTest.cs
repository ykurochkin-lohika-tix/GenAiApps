using AskGenAi.Core.Entities;
using AskGenAi.Core.Interfaces;
using AskGenAi.Infrastructure.Persistence;
using AskGenAi.xTests.Utils;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace AskGenAi.xTests.Infrastructure.Persistence;

public class UserRepositoryTest
{
    private readonly IRepository<User> _userRepository;

    public UserRepositoryTest()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddDbContextInMemory();

        var dbContextMemory = EfCoreContextHelper.GetAppDbContextInMemory();

        // mocks
        serviceCollection.AddSingleton(dbContextMemory);

        // real
        serviceCollection.AddSingleton<IRepository<User>, Repository<User>>();

        // build
        var serviceProvider = serviceCollection.BuildServiceProvider();
        _userRepository = serviceProvider.GetRequiredService<IRepository<User>>();
    }

    [Fact]
    public async Task AddAsync_ShouldAddUser()
    {
        var user = new User { Id = Guid.NewGuid(), Name = "Test User", Email = "test@example.com" };

        await _userRepository.AddAsync(user);
        await _userRepository.UnitOfWork.SaveChangesAsync();

        var retrievedUser = await _userRepository.GetByIdAsync(user.Id);
        Assert.NotNull(retrievedUser);
        Assert.Equal(user.Name, retrievedUser.Name);
    }

    [Fact]
    public async Task RemoveByIdAsync_ShouldRemoveUser()
    {
        var user = new User { Id = Guid.NewGuid(), Name = "Test User", Email = "test@example.com" };

        await _userRepository.AddAsync(user);
        await _userRepository.UnitOfWork.SaveChangesAsync();

        await _userRepository.RemoveByIdAsync(user.Id);
        await _userRepository.UnitOfWork.SaveChangesAsync();

        var retrievedUser = await _userRepository.GetByIdAsync(user.Id);
        Assert.Null(retrievedUser);
    }

    [Fact]
    public async Task Update_ShouldUpdateUser()
    {
        var user = new User { Id = Guid.NewGuid(), Name = "Test User", Email = "test@example.com" };

        await _userRepository.AddAsync(user);
        await _userRepository.UnitOfWork.SaveChangesAsync();

        user.Name = "Updated User";
        _userRepository.Update(user);
        await _userRepository.UnitOfWork.SaveChangesAsync();

        var retrievedUser = await _userRepository.GetByIdAsync(user.Id);
        Assert.NotNull(retrievedUser);
        Assert.Equal("Updated User", retrievedUser.Name);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllUsers()
    {
        var user1 = new User { Id = Guid.NewGuid(), Name = "User 1", Email = "user1@example.com" };
        var user2 = new User { Id = Guid.NewGuid(), Name = "User 2", Email = "user2@example.com" };

        await _userRepository.AddAsync(user1);
        await _userRepository.AddAsync(user2);
        await _userRepository.UnitOfWork.SaveChangesAsync();

        var users = await _userRepository.GetAllAsync(null);
        Assert.Equal(2, users.Count());
    }

    [Fact]
    public async Task CountAsync_ShouldReturnCorrectCount()
    {
        var user1 = new User { Id = Guid.NewGuid(), Name = "User 1", Email = "user1@example.com" };
        var user2 = new User { Id = Guid.NewGuid(), Name = "User 2", Email = "user2@example.com" };

        await _userRepository.AddAsync(user1);
        await _userRepository.AddAsync(user2);
        await _userRepository.UnitOfWork.SaveChangesAsync();

        var count = await _userRepository.CountAsync(u => u.Email.Contains("example.com"));
        Assert.Equal(2, count);
    }

    [Fact]
    public async Task Add_ShouldAddUser()
    {
        var user = new User { Id = Guid.NewGuid(), Name = "Test User", Email = "test@example.com" };

        _userRepository.Add(user);
        await _userRepository.UnitOfWork.SaveChangesAsync();

        var retrievedUser = await _userRepository.GetByIdAsync(user.Id);
        Assert.NotNull(retrievedUser);
        Assert.Equal(user.Name, retrievedUser.Name);
    }

    [Fact]
    public async Task AddRangeAsync_ShouldAddUsers()
    {
        var user1 = new User { Id = Guid.NewGuid(), Name = "User 1", Email = "user1@example.com" };
        var user2 = new User { Id = Guid.NewGuid(), Name = "User 2", Email = "user2@example.com" };

        await _userRepository.AddRangeAsync(CancellationToken.None, user1, user2);
        await _userRepository.UnitOfWork.SaveChangesAsync();

        var retrievedUser1 = await _userRepository.GetByIdAsync(user1.Id);
        var retrievedUser2 = await _userRepository.GetByIdAsync(user2.Id);

        Assert.NotNull(retrievedUser1);
        Assert.NotNull(retrievedUser2);
    }

    [Fact]
    public async Task Remove_ShouldRemoveUser()
    {
        var user = new User { Id = Guid.NewGuid(), Name = "Test User", Email = "test@example.com" };

        _userRepository.Add(user);
        await _userRepository.UnitOfWork.SaveChangesAsync();

        _userRepository.Remove(user);
        await _userRepository.UnitOfWork.SaveChangesAsync();

        var retrievedUser = await _userRepository.GetByIdAsync(user.Id);
        Assert.Null(retrievedUser);
    }

    [Fact]
    public async Task FirstOrDefaultAsync_ShouldReturnFirstMatchingUser()
    {
        var user1 = new User { Id = Guid.NewGuid(), Name = "User 1", Email = "user1@example.com" };
        var user2 = new User { Id = Guid.NewGuid(), Name = "User 2", Email = "user2@example.com" };

        await _userRepository.AddAsync(user1);
        await _userRepository.AddAsync(user2);
        await _userRepository.UnitOfWork.SaveChangesAsync();

        Expression<Func<User, bool>> predicate = u => u.Email.Contains("user2");
        var retrievedUser = await _userRepository.FirstOrDefaultAsync(predicate);

        Assert.NotNull(retrievedUser);
        Assert.Equal(user2.Name, retrievedUser.Name);
    }

    [Fact]
    public async Task GetProjectedAsync_ShouldReturnProjectedUsers()
    {
        var user1 = new User { Id = Guid.NewGuid(), Name = "User 1", Email = "user1@example.com" };
        var user2 = new User { Id = Guid.NewGuid(), Name = "User 2", Email = "user2@example.com" };

        await _userRepository.AddAsync(user1);
        await _userRepository.AddAsync(user2);
        await _userRepository.UnitOfWork.SaveChangesAsync();

        var projectedUsers = (await _userRepository.GetProjectedAsync(u => new { u.Name })).ToArray();

        Assert.Equal(2, projectedUsers.Count());
        Assert.Contains(projectedUsers, u => u.Name == "User 1");
        Assert.Contains(projectedUsers, u => u.Name == "User 2");
    }
}