using GameStore.API.Models;

namespace GameStore.API.Repositories.Genres;

public interface IGenreRepository
{
    Task<List<Genre>> GetAllAsync(CancellationToken cancellationToken);
    Task<Genre?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Genre> AddAsync(Genre genre, CancellationToken cancellationToken);
    Task UpdateAsync(Genre genre, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
}