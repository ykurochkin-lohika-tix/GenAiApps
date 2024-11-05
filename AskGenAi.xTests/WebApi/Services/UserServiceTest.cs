using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using AutoFixture;
using Moq;
using AskGenAi.Core.Entities;
using AskGenAi.Core.Interfaces;
using AskGenAi.WebApi.Auth.Services;

namespace AskGenAi.xTests.WebApi.Services;

public class UserServiceTest
{
    private readonly Fixture _fixture = new();

    private readonly Mock<IRepository<User>> _userRepositoryMock;
    private readonly Mock<IRepository<UserRole>> _userRoleRepositoryMock;
    private readonly Mock<IPasswordHasher<User>> _passwordHasherMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly IUserService _userService;

    public UserServiceTest()
    {
        _userRepositoryMock = new Mock<IRepository<User>>();
        _userRoleRepositoryMock = new Mock<IRepository<UserRole>>();
        _passwordHasherMock = new Mock<IPasswordHasher<User>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _userRepositoryMock.Setup(repo => repo.UnitOfWork).Returns(_unitOfWorkMock.Object);

        _userService = new UserService(
            _userRepositoryMock.Object,
            _userRoleRepositoryMock.Object,
            _passwordHasherMock.Object);
    }

    [Fact]
    public async Task CreateUserAsync_ShouldCreateUser()
    {
        // Arrange
        const string email = "test@example.com";
        const string password = "Password123";
        var user = new User { Id = _fixture.Create<Guid>(), Email = email };
        _passwordHasherMock.Setup(ph => ph.HashPassword(user, password)).Returns("hashedPassword");

        // Act
        await _userService.CreateUserAsync(email, password);

        // Assert
        _userRepositoryMock.Verify(ur => ur.AddAsync(It.IsAny<User>(), default), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task ValidateUserCredentialsAsync_ShouldReturnUser_WhenCredentialsAreValid()
    {
        // Arrange
        const string email = "test@example.com";
        const string password = "Password123";
        var user = new User { Id = _fixture.Create<Guid>(), Email = email, PasswordHash = "hashedPassword" };
        _userRepositoryMock.Setup(ur => ur.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), default))
            .ReturnsAsync(user);
        _passwordHasherMock.Setup(ph => ph.VerifyHashedPassword(user, "hashedPassword", password))
            .Returns(PasswordVerificationResult.Success);

        // Act
        var result = await _userService.ValidateUserCredentialsAsync(email, password);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
    }

    [Fact]
    public async Task ValidateUserCredentialsAsync_ShouldReturnNull_WhenCredentialsAreInvalid()
    {
        // Arrange
        const string email = "test@example.com";
        const string password = "Password123";
        var user = new User { Id = _fixture.Create<Guid>(), Email = email, PasswordHash = "hashedPassword" };
        _userRepositoryMock.Setup(ur => ur.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), default))
            .ReturnsAsync(user);
        _passwordHasherMock.Setup(ph => ph.VerifyHashedPassword(user, "hashedPassword", password))
            .Returns(PasswordVerificationResult.Failed);

        // Act
        var result = await _userService.ValidateUserCredentialsAsync(email, password);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetUserRolesAsync_ShouldReturnRoles()
    {
        // Arrange
        var userId = _fixture.Create<Guid>();
        var userRoles = new List<UserRole>
        {
            new() { UserId = userId, Role = new Role { Name = "Admin" } },
            new() { UserId = userId, Role = new Role { Name = "User" } }
        };
        _userRoleRepositoryMock
            .Setup(ur => ur.GetAllNoTrackAsync(It.IsAny<Expression<Func<UserRole, bool>>>(), default))
            .ReturnsAsync(userRoles);

        // Act
        var roles = await _userService.GetUserRolesAsync(userId);

        // Assert
        var collection = roles as string[] ?? roles.ToArray();
        Assert.Contains("Admin", collection);
        Assert.Contains("User", collection);
    }

    [Fact]
    public async Task CheckUserExistAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        const string email = "test@example.com";
        var user = new User { Id = _fixture.Create<Guid>(), Email = email };
        _userRepositoryMock.Setup(ur => ur.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), default))
            .ReturnsAsync(user);

        // Act
        var result = await _userService.CheckUserExistAsync(email);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
    }

    [Fact]
    public async Task CheckUserExistAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Arrange
        const string email = "test@example.com";
        _userRepositoryMock.Setup(ur => ur.FirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), default))
            .ReturnsAsync((User)null!);

        // Act
        var result = await _userService.CheckUserExistAsync(email);

        // Assert
        Assert.Null(result);
    }
}