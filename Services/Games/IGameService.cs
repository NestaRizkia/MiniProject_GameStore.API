using GameStore.API.Dtos.Games;
using GameStore.API.Models;

namespace GameStore.API.Services.Games;

public interface IGameService
{
    Task<List<GameSummaryDto>> GetGamesAsync();
    Task<GameDetailsDto?> GetGameByIdAsync(int id);
    Task<GameDetailsDto> AddGameAsync(CreateGameDto createdGame);
    Task UpdateGameAsync(int id, UpdateGameDto updatedGame);
    Task DeleteGameAsync(int id);
}