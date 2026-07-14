using GameStore.API.Dtos;
using GameStore.API.Dtos.Games;

namespace GameStore.API.Services.Games;

public interface IGameService
{
    Task<List<GameSummaryDto>> GetGamesAsync();
    Task<PaginatedResult<GameSummaryDto>> GetFilteredGamesAsync(GameFilterDto filter);
    Task<GameDetailsDto?> GetGameByIdAsync(int id);
    Task<GameDetailsDto> AddGameAsync(CreateGameDto createdGame);
    Task UpdateGameAsync(int id, UpdateGameDto updatedGame);
    Task DeleteGameAsync(int id);
}