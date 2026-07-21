using GameStore.API.Dtos.Games;
using GameStore.API.Models;

namespace GameStore.API.Repositories.Games;

public interface IGameRepository
{
    Task<(List<GameSummaryDto> Games, int TotalCount)> GetAllAsync(GameFilterDto filter, CancellationToken cancellationToken);
    Task<Game?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Game> AddAsync(Game game, CancellationToken cancellationToken);
    Task UpdateAsync(Game game, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
}