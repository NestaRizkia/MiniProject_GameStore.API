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
                SELECT ""Id"", ""Name"", ""GenreId"", ""Price"", ""ReleaseDate"", ""CreatedAt"", ""UpdatedAt"" 
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
        // Build LINQ query with filters
        var query = dbContext.Games.AsQueryable();

        // Search filter (Name)
        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTerm = filter.SearchTerm.ToLower();
            query = query.Where(g => g.Name.ToLower().Contains(searchTerm));
        }

        // Price range filter
        if (filter.MinPrice.HasValue && filter.MaxPrice.HasValue)
        {
            query = query.Where(g => g.Price >= filter.MinPrice.Value && g.Price <= filter.MaxPrice.Value);
        }
        else if (filter.MinPrice.HasValue)
        {
            query = query.Where(g => g.Price >= filter.MinPrice.Value);
        }
        else if (filter.MaxPrice.HasValue)
        {
            query = query.Where(g => g.Price <= filter.MaxPrice.Value);
        }

        // Date range filter
        if (filter.StartDate.HasValue && filter.EndDate.HasValue)
        {
            query = query.Where(g => g.ReleaseDate >= filter.StartDate.Value && g.ReleaseDate <= filter.EndDate.Value);
        }
        else if (filter.StartDate.HasValue)
        {
            query = query.Where(g => g.ReleaseDate >= filter.StartDate.Value);
        }
        else if (filter.EndDate.HasValue)
        {
            query = query.Where(g => g.ReleaseDate <= filter.EndDate.Value);
        }

        // Get total count with filters
        var totalCount = await query.CountAsync();

        // Add pagination and sorting
        var games = await query
            .OrderBy(g => g.Name)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Include(g => g.Genre)
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