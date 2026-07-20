using GameStore.API.Models;
using GameStore.API.Repositories.Games;
using GameStore.API.Repositories.Genres;
using GameStore.API.Dtos;
using GameStore.API.Dtos.Games;

namespace GameStore.API.Services.Games;
public class GameService(IGameRepository gameRepository, IGenreRepository genreRepository) : IGameService
{
    public async Task<PaginatedResult<GameSummaryDto>> GetFilteredGamesAsync(GameFilterDto filter)
    {
        var (games, totalCount) = await gameRepository.GetAllAsync(filter);

        if (filter.PageSize > 0)
        {
            return new PaginatedResult<GameSummaryDto>(games, totalCount, filter.PageNumber, filter.PageSize);
        }

        return new PaginatedResult<GameSummaryDto>(games, totalCount, 1, totalCount);
    }

    public async Task<GameDetailsDto?> GetGameByIdAsync(int id)
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
        var genreExists = await genreRepository.GetByIdAsync(createGame.GenreId);
        if (genreExists is null)
        {
            throw new KeyNotFoundException($"Genre with id {createGame.GenreId} not found");
        }

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

    public async Task PatchGameAsync(int id, PatchGameDto patchGame)
    {
        var existingGame = await gameRepository.GetByIdAsync(id);
        if (existingGame is null)
        {
            throw new KeyNotFoundException($"Game with id {id} not Found");
        }

        if (patchGame.Name != null)
        {
            existingGame.Name = patchGame.Name;
        }

        if (patchGame.GenreId.HasValue)
        {
            var genreExists = await genreRepository.GetByIdAsync(patchGame.GenreId.Value);
            if (genreExists is null)
            {
                throw new KeyNotFoundException($"Genre with id {patchGame.GenreId.Value} not found");
            }
            
            existingGame.GenreId = patchGame.GenreId.Value;
        }

        if (patchGame.Price.HasValue)
        {
            existingGame.Price = patchGame.Price.Value;
        }

        if (patchGame.ReleaseDate.HasValue)
        {
            existingGame.ReleaseDate = patchGame.ReleaseDate.Value;
        }

        await gameRepository.UpdateAsync(existingGame);
    }

    public async Task DeleteGameAsync(int id)
    {
        await gameRepository.DeleteAsync(id);
    }
}