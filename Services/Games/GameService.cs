using GameStore.API.Models;
using GameStore.API.Repositories.Games;
using GameStore.API.Dtos.Games;

namespace GameStore.API.Services.Games;
public class GameService(IGameRepository gameRepository) : IGameService
{
    public async Task<List<GameSummaryDto>> GetGamesAsync()
    {
        var games = await gameRepository.GetAllAsync();
        return games.Select(game => new GameSummaryDto(
            game.Id,
            game.Name,
            game.Genre.Name,
            game.Price,
            game.ReleaseDate
        )).ToList();
    }

    public async Task<GameDetailsDto> GetGameByIdAsync(int id)
    {
        var game = await gameRepository.GetByIdAsync(id);
        
        if(game is null)
        {
            return null;
        }

        return new GameDetailsDto(
            game.Id, game.Name, game.GenreId, game.Price, game.ReleaseDate
        );
    }

    public async Task<GameDetailsDto> AddGameAsync(CreateGameDto createGame)
    {
        var game = new Game{
            Name = createGame.Name,
            GenreId = createGame.GenreId,
            Price = createGame.Price,
            ReleaseDate = createGame.ReleaseDate
        };

        var result = await gameRepository.AddAsync(game);
        
        return new GameDetailsDto(
            result.Id,
            result.Name,
            result.GenreId,
            result.Price,
            result.ReleaseDate
        );
    }

    public async Task UpdateGameAsync(int id, UpdateGameDto updatedGame)
    {
        var existingGame = await gameRepository.GetByIdAsync(id);
        if (existingGame is null)
        {
            throw new KeyNotFoundException($"Game with id {id} not Found");
        }

        existingGame.Name = updatedGame.Name;
        existingGame.GenreId = updatedGame.GenreId;
        existingGame.Price = updatedGame.Price;
        existingGame.ReleaseDate = updatedGame.ReleaseDate;

        await gameRepository.UpdateAsync(existingGame);
    }

    public async Task DeleteGameAsync(int id)
    {
        await gameRepository.DeleteAsync(id);
    }
}