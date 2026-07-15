using GameStore.API.Models;

namespace GameStore.API.Repositories.Auth;

public interface IAuthRepository
{
    Task<User?> GetUserByUsernameAsync(string username);
    Task<User?> GetUserByEmailAsync(string email);
    Task<User> CreateUserAsync(User user);
    Task<bool> UserExistsAsync(string username, string email);
}
