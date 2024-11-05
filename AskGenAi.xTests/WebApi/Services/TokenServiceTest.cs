using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using AutoFixture;
using AskGenAi.Core.Entities;
using AskGenAi.WebApi.Auth.Services;
using AskGenAi.WebApi.Configurations;

namespace AskGenAi.xTests.WebApi.Services;

public class TokenServiceTest
{
    private readonly ITokenService _tokenService;
    private readonly JwtSettings _jwtSettings;
    private readonly Fixture _fixture = new();

    public TokenServiceTest()
    {
        _jwtSettings = new JwtSettings
        {
            Key = "Il+onLCfUmavpl3/F3mEW8DIcloQIDiOTi/cmlMZIEi=", // generated 256-bit key
            Issuer = "TestIssuer",
            Audience = "TestAudience"
        };

        var options = Options.Create(_jwtSettings);
        _tokenService = new TokenService(options);
    }

    [Fact]
    public void GenerateToken_ShouldReturnValidToken()
    {
        // Arrange
        var user = new User
        {
            Id = _fixture.Create<Guid>(),
            Email = "test@example.com"
        };
        var roles = new List<string> { "Admin", "User" };

        // Act
        var token = _tokenService.GenerateToken(user, roles);

        // Assert
        Assert.NotNull(token);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        Assert.Equal(_jwtSettings.Issuer, jwtToken.Issuer);
        Assert.Equal(_jwtSettings.Audience, jwtToken.Audiences.First());
        Assert.Contains(jwtToken.Claims, c => c.Type == ClaimTypes.NameIdentifier && c.Value == user.Id.ToString());
        Assert.Contains(jwtToken.Claims, c => c.Type == ClaimTypes.Email && c.Value == user.Email);
        Assert.Contains(jwtToken.Claims, c => c.Type == ClaimTypes.Role && c.Value == "Admin");
        Assert.Contains(jwtToken.Claims, c => c.Type == ClaimTypes.Role && c.Value == "User");
    }

    [Fact]
    public void GenerateToken_ShouldExpireInOneHour()
    {
        // Arrange
        var user = new User
        {
            Id = _fixture.Create<Guid>(),
            Email = "test@example.com"
        };
        var roles = new List<string> { "Admin", "User" };

        // Act
        var token = _tokenService.GenerateToken(user, roles);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var expiration = jwtToken.ValidTo;
        Assert.True(expiration > DateTime.UtcNow);
        Assert.True(expiration <= DateTime.UtcNow.AddHours(1));
    }
}