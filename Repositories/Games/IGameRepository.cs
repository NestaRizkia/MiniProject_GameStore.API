using GameStore.API.Dtos.Games;
using GameStore.API.Models;

namespace GameStore.API.Repositories.Games;

public interface IGameRepository
{
    Task<(List<GameSummaryDto> Games, int TotalCount)> GetAllAsync(GameFilterDto filter);
    Task<Game?> GetByIdAsync(int id);
    Task<Game> AddAsync(Game game);
    Task UpdateAsync(Game game);
    Task DeleteAsync(int id);
}