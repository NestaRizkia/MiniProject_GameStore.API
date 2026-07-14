using GameStore.API.Dtos.Games;
using GameStore.API.Models;

namespace GameStore.API.Repositories.Games;

public interface IGameRepository
{
    Task<List<Game>> GetAllAsync();
    Task<Game?> GetByIdAsync(int id);
    Task<(List<Game> Games, int TotalCount)> GetFilteredGamesAsync(GameFilterDto filter);
    Task<Game> AddAsync(Game game);
    Task UpdateAsync(Game game);
    Task DeleteAsync(int id);
}