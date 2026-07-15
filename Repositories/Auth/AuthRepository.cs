using GameStore.API.Data;
using GameStore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Repositories.Auth;

public class AuthRepository(GameStoreContext dbContext) : IAuthRepository
{
    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await dbContext.Users
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> CreateUserAsync(User user)
    {
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
        return user;
    }

    public async Task<bool> UserExistsAsync(string username, string email)
    {
        return await dbContext.Users
            .AnyAsync(u => u.Username == username || u.Email == email);
    }
}
