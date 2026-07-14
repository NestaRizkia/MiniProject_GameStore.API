using GameStore.API.Models;
using GameStore.API.Data;
using GameStore.API.Dtos.Games;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Repositories.Games;

public class GameRepository(GameStoreContext dbContext) : IGameRepository
{
    public async Task<List<Game>> GetAllAsync()
    {
        return await dbContext.Games
            .FromSqlRaw(@"
                SELECT ""Id"", ""Name"", ""GenreId"", ""Price"", ""ReleaseDate"" 
                FROM ""Games""")
            .Include(game => game.Genre)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Game?> GetByIdAsync(int id)
    {
        return await dbContext.Games.FindAsync(id);
    }

    public async Task<(List<Game> Games, int TotalCount)> GetFilteredGamesAsync(GameFilterDto filter)
    {
        var query = dbContext.Games.Include(game => game.Genre).AsQueryable();

        // Apply search filter - case-insensitive search on Name and Genre
        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTerm = filter.SearchTerm.ToLower();
            query = query.Where(g => 
                g.Name.ToLower().Contains(searchTerm) || 
                (g.Genre != null && g.Genre.Name.ToLower().Contains(searchTerm)));
        }

        // Apply price range filters
        if (filter.MinPrice.HasValue)
        {
            query = query.Where(g => g.Price >= filter.MinPrice.Value);
        }

        if (filter.MaxPrice.HasValue)
        {
            query = query.Where(g => g.Price <= filter.MaxPrice.Value);
        }

        // Apply date range filters
        if (filter.StartDate.HasValue)
        {
            query = query.Where(g => g.ReleaseDate >= filter.StartDate.Value);
        }

        if (filter.EndDate.HasValue)
        {
            query = query.Where(g => g.ReleaseDate <= filter.EndDate.Value);
        }

        // Get total count before pagination
        var totalCount = await query.CountAsync();

        // Apply pagination
        var games = await query
            .OrderBy(g => g.Name)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .AsNoTracking()
            .ToListAsync();

        return (games, totalCount);
    }

    public async Task<Game> AddAsync(Game game)
    {
        dbContext.Games.Add(game);
        await dbContext.SaveChangesAsync();
        return game;
    }

    public async Task UpdateAsync(Game game)
    {
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await dbContext.Games
            .Where(game => game.Id == id)
            .ExecuteDeleteAsync();
    }
}