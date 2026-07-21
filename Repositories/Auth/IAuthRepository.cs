using GameStore.API.Models;

namespace GameStore.API.Repositories.Auth;

public interface IAuthRepository
{
    Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken);
    Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
    Task<User> CreateUserAsync(User user, CancellationToken cancellationToken);
    Task<bool> UserExistsAsync(string username, string email, CancellationToken cancellationToken);
}
