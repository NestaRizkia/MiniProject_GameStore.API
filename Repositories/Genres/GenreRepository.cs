using GameStore.API.Models;
using GameStore.API.Data;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Repositories.Genres;

public class GenreRepository(GameStoreContext dbContext) : IGenreRepository
{
    public async Task<List<Genre>> GetAllAsync()
    {
        return await dbContext.Genres.ToListAsync();
    }

    public async Task<Genre?> GetByIdAsync(int id)
    {
        return await dbContext.Genres.FindAsync(id);
    }

    public async Task<Genre> AddAsync(Genre genre)
    {
        dbContext.Genres.Add(genre);
        await dbContext.SaveChangesAsync();
        return genre;
    }

    public async Task UpdateAsync(Genre genre)
    {
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await dbContext.Genres
            .Where(genre => genre.Id == id)
            .ExecuteDeleteAsync();
    }
}
