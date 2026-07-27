using GameStore.API.Dtos;
using GameStore.API.Dtos.Games;

namespace GameStore.API.Services.Interfaces;

public interface IGameService
{
    Task<PaginatedResult<GameSummaryDto>> GetFilteredGamesAsync(GameFilterDto filter, CancellationToken cancellationToken);
    Task<GameSummaryDto?> GetGameByIdAsync(int id, CancellationToken cancellationToken);
    Task<GameSummaryDto> AddGameAsync(CreateGameDto createdGame, CancellationToken cancellationToken);
    Task PatchGameAsync(int id, PatchGameDto patchGame, CancellationToken cancellationToken);
    Task DeleteGameAsync(int id, CancellationToken cancellationToken);
}