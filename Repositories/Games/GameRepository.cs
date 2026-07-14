using GameStore.API.Models;
using GameStore.API.Data;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Repositories.Games;

public class GameRepository(GameStoreContext dbContext) : IGameRepository
{
    public async Task<List<Game>> GetAllAsync()
    {
        return await dbContext.Games
            .Include(game => game.Genre)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Game?> GetByIdAsync(int id)
    {
        return await dbContext.Games.FindAsync(id);
    }

    public async Task<Game> AddAsync(Game game)
    {
        dbContext.Games.Add(game);
        await dbContext.SaveChangesAsync();
        return game;
    }

    public async Task UpdateAsync(int id, Game game)
    {
        await dbContext.Games.FindAsync(id);

        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await dbContext.Games
            .Where(game => game.Id == id)
            .ExecuteDeleteAsync();
    }
}