using GameStore.API.Dtos;
using GameStore.API.Dtos.Games;

namespace GameStore.API.Services.Games;

public interface IGameService
{
    Task<PaginatedResult<GameSummaryDto>> GetFilteredGamesAsync(GameFilterDto filter);
    Task<GameDetailsDto?> GetGameByIdAsync(int id);
    Task<GameDetailsDto> AddGameAsync(CreateGameDto createdGame);
    Task PatchGameAsync(int id, PatchGameDto patchGame);
    Task DeleteGameAsync(int id);
}