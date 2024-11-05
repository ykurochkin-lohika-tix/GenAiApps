using AskGenAi.WebApi.Auth.Services;
using AskGenAi.WebApi.DTOs;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace AskGenAi.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IUserService userService, ITokenService tokenService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("All fields are required.");
        }

        var user = await userService.ValidateUserCredentialsAsync(request.Email, request.Password);
        if (user == null)
        {
            return Unauthorized("Invalid credentials");
        }

        var roles = await userService.GetUserRolesAsync(user.Id);
        var token = tokenService.GenerateToken(user, roles);

        return Ok(new LoginResponse(token));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("All fields are required.");
        }

        var existingUser = await userService.CheckUserExistAsync(request.Email);

        if (existingUser != null)
        {
            return BadRequest("Username or email is already taken.");
        }

        await userService.CreateUserAsync(request.Email, request.Password);

        return Ok("User registered successfully.");
    }
}