using GameStore.API.Models;
using GameStore.API.Data;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Repositories.Genres;

public class GenreRepository(GameStoreContext dbContext) : IGenreRepository
{
    public async Task<List<Genre>> GetAllAsync()
    {
        return await dbContext.Genres
            .FromSqlRaw(@"
                SELECT ""Id"", ""Name"", ""CreatedAt"", ""UpdatedAt"" 
                FROM ""Genres""")
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Genre?> GetByIdAsync(int id)
    {
        return await dbContext.Genres
            .FromSqlRaw(@"
                SELECT ""Id"", ""Name"", ""CreatedAt"", ""UpdatedAt""
                FROM ""Genres""
                WHERE ""Id"" = {0}", id)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task<Genre> AddAsync(Genre genre)
    {
        dbContext.Genres.Add(genre);
        await dbContext.SaveChangesAsync();
        return genre;
    }

    public async Task UpdateAsync(Genre genre)
    {
        dbContext.Genres.Update(genre);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var hasGames = await dbContext.Games.AnyAsync(g => g.GenreId == id);
        if (hasGames)
            throw new InvalidOperationException("Cannot delete genre with existing games");

        await dbContext.Genres
            .Where(genre => genre.Id == id)
            .ExecuteDeleteAsync();
    }
}
