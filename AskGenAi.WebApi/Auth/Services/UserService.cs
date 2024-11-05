using AskGenAi.Core.Entities;
using AskGenAi.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AskGenAi.WebApi.Auth.Services;

public interface IUserService
{
    Task CreateUserAsync(string email, string password);
    Task<User?> ValidateUserCredentialsAsync(string username, string password);
    Task<IEnumerable<string>> GetUserRolesAsync(Guid userId);
    Task<User?> CheckUserExistAsync(string email);
}

public class UserService(
    IRepository<User> userRepository,
    IRepository<UserRole> userRoleRepository,
    IPasswordHasher<User> passwordHasher) : IUserService
{
    // Hashes the password before saving a new user
    public async Task CreateUserAsync(string email, string password)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email
        };
        user.PasswordHash = passwordHasher.HashPassword(user, password);

        await userRepository.AddAsync(user);
        await userRepository.UnitOfWork.SaveChangesAsync();
    }

    // Verifies the hashed password during login
    public async Task<User?> ValidateUserCredentialsAsync(string email, string password)
    {
        var user = await userRepository.FirstOrDefaultAsync(u => u.Email == email);

        if (user == null ||
            passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password) != PasswordVerificationResult.Success)
        {
            return null;
        }

        return user;
    }

    public async Task<IEnumerable<string>> GetUserRolesAsync(Guid userId)
    {
        var userRoles = await userRoleRepository.GetAllNoTrackAsync(u => u.UserId == userId);

        return userRoles.Select(u => u.Role?.Name ?? string.Empty);
    }

    public Task<User?> CheckUserExistAsync(string email)
    {
        return userRepository.FirstOrDefaultAsync(u => u.Email == email);
    }
}