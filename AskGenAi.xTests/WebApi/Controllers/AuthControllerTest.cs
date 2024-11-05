using AskGenAi.Core.Entities;
using AskGenAi.WebApi.Auth.Services;
using AskGenAi.WebApi.Controllers;
using AskGenAi.WebApi.DTOs;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AskGenAi.xTests.WebApi.Controllers;

public class AuthControllerTest
{
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly AuthController _authController;

    public AuthControllerTest()
    {
        _userServiceMock = new Mock<IUserService>();
        _tokenServiceMock = new Mock<ITokenService>();
        _authController = new AuthController(_userServiceMock.Object, _tokenServiceMock.Object);
    }

    [Fact]
    public async Task Login_ShouldReturnBadRequest_WhenFieldsAreEmpty()
    {
        // Arrange
        var request = new LoginRequest { Email = "", Password = "" };

        // Act
        var result = await _authController.Login(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("All fields are required.", badRequestResult.Value);
    }

    [Fact]
    public async Task Login_ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
    {
        // Arrange
        var request = new LoginRequest { Email = "test@example.com", Password = "Password123" };
        _userServiceMock.Setup(us => us.ValidateUserCredentialsAsync(request.Email, request.Password))
            .ReturnsAsync((User)null!);

        // Act
        var result = await _authController.Login(request);

        // Assert
        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.Equal("Invalid credentials", unauthorizedResult.Value);
    }

    [Fact]
    public async Task Login_ShouldReturnOk_WhenCredentialsAreValid()
    {
        // Arrange
        var request = new LoginRequest { Email = "test@example.com", Password = "Password123" };
        var user = new User { Id = Guid.NewGuid(), Email = request.Email };
        var roles = new List<string> { "User" };
        const string token = "generatedToken";

        _userServiceMock.Setup(us => us.ValidateUserCredentialsAsync(request.Email, request.Password))
            .ReturnsAsync(user);
        _userServiceMock.Setup(us => us.GetUserRolesAsync(user.Id)).ReturnsAsync(roles);
        _tokenServiceMock.Setup(ts => ts.GenerateToken(user, roles)).Returns(token);

        // Act
        var result = await _authController.Login(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.Equal(token, (okResult.Value as LoginResponse)!.Token);
    }

    [Fact]
    public async Task Register_ShouldReturnBadRequest_WhenFieldsAreEmpty()
    {
        // Arrange
        var request = new RegisterRequest { Email = "", Password = "" };

        // Act
        var result = await _authController.Register(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("All fields are required.", badRequestResult.Value);
    }

    [Fact]
    public async Task Register_ShouldReturnBadRequest_WhenUserAlreadyExists()
    {
        // Arrange
        var request = new RegisterRequest { Email = "test@example.com", Password = "Password123" };
        var existingUser = new User { Id = Guid.NewGuid(), Email = request.Email };

        _userServiceMock.Setup(us => us.CheckUserExistAsync(request.Email)).ReturnsAsync(existingUser);

        // Act
        var result = await _authController.Register(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Username or email is already taken.", badRequestResult.Value);
    }

    [Fact]
    public async Task Register_ShouldReturnOk_WhenUserIsRegisteredSuccessfully()
    {
        // Arrange
        var request = new RegisterRequest { Email = "test@example.com", Password = "Password123" };

        _userServiceMock.Setup(us => us.CheckUserExistAsync(request.Email)).ReturnsAsync((User)null!);
        _userServiceMock.Setup(us => us.CreateUserAsync(request.Email, request.Password)).Returns(Task.CompletedTask);

        // Act
        var result = await _authController.Register(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("User registered successfully.", okResult.Value);
    }
}