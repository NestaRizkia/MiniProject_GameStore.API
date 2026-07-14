using GameStore.API.Models;

namespace GameStore.API.Repositories.Genres;

public interface IGenreRepository
{
    Task<List<Genre>> GetAllAsync();
    Task<Genre?> GetByIdAsync(int id);
    Task<Genre> AddAsync(Genre genre);
    Task UpdateAsync(Genre genre);
    Task DeleteAsync(int id);
}