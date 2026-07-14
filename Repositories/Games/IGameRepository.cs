using GameStore.API.Models;

namespace GameStore.API.Repositories.Games;

public interface IGameRepository
{
    Task<List<Game>> GetAllAsync();
    Task<Game?> GetByIdAsync(int id);
    Task<Game> AddAsync(Game game);
    Task UpdateAsync(int id, Game game);
    Task DeleteAsync(int id);
}