using GameStore.API.Dtos;
using GameStore.API.Dtos.Games;

namespace GameStore.API.Services.Games;

public interface IGameService
{
    Task<PaginatedResult<GameSummaryDto>> GetFilteredGamesAsync(GameFilterDto filter, CancellationToken cancellationToken);
    Task<GameDetailsDto?> GetGameByIdAsync(int id, CancellationToken cancellationToken);
    Task<GameDetailsDto> AddGameAsync(CreateGameDto createdGame, CancellationToken cancellationToken);
    Task PatchGameAsync(int id, PatchGameDto patchGame, CancellationToken cancellationToken);
    Task DeleteGameAsync(int id, CancellationToken cancellationToken);
}