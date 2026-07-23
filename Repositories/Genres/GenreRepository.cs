using GameStore.API.Models;
using GameStore.API.Data;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Repositories.Genres;

public class GenreRepository(GameStoreContext dbContext) : IGenreRepository
{
    public async Task<List<Genre>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Genres
            .FromSqlRaw(@"
                SELECT ""Id"", ""Name"", ""CreatedAt"", ""UpdatedAt"" 
                FROM ""Genres""")
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Genre?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await dbContext.Genres
            .Where(g => g.Id == id)
            .Select(g => new Genre
            {
                Id = g.Id,
                Name = g.Name,
                CreatedAt = g.CreatedAt,
                UpdatedAt = g.UpdatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Genre> AddAsync(Genre genre, CancellationToken cancellationToken)
    {
        dbContext.Genres.Add(genre);
        await dbContext.SaveChangesAsync(cancellationToken);
        return genre;
    }

    public async Task UpdateAsync(Genre genre, CancellationToken cancellationToken)
    {
        dbContext.Genres.Update(genre);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var hasGames = await dbContext.Games.AnyAsync(g => g.GenreId == id, cancellationToken);
        if (hasGames)
            throw new InvalidOperationException("Cannot delete genre with existing games");

        await dbContext.Genres
            .Where(genre => genre.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }
}
