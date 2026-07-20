using GameStore.API.Data;
using GameStore.API.Dtos.Games;
using GameStore.API.Models;
using Fastenshtein;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace GameStore.API.Repositories.Games;

public class GameRepository(GameStoreContext dbContext) : IGameRepository
{
    public async Task<(List<GameSummaryDto> Games, int TotalCount)> GetAllAsync(GameFilterDto filter)
    {
        var sql = @"
            SELECT g.""Id"", g.""Name"", g.""Price"", g.""ReleaseDate"", g.""UpdatedAt"",
                   ge.""Name"" AS ""Genre""
            FROM ""Games"" g
            INNER JOIN ""Genres"" ge ON g.""GenreId"" = ge.""Id""
            WHERE g.""Price"" BETWEEN @minPrice AND @maxPrice
              AND g.""ReleaseDate"" BETWEEN @startDate AND @endDate";

        var parameters = new NpgsqlParameter[]
        {
            new("@minPrice", filter.MinPrice),
            new("@maxPrice", filter.MaxPrice),
            new("@startDate", filter.StartDate),
            new("@endDate", filter.EndDate)
        };

        var games = await dbContext.Database
            .SqlQueryRaw<GameSummaryDto>(sql, parameters)
            .AsNoTracking()
            .ToListAsync();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTerm = filter.SearchTerm.Trim().ToLower();
            var levenshtein = new Levenshtein(searchTerm);

            games = games
                .Select(game => new
                {
                    Game = game,
                    Distance = levenshtein.DistanceFrom(game.Name.ToLower())
                })
                .Where(candidate => candidate.Distance <= filter.MaxEditDistance)
                .OrderBy(candidate => candidate.Distance)
                .ThenBy(candidate => candidate.Game.Name)
                .Select(candidate => candidate.Game)
                .ToList();
        }
        else
        {
            games = games
                .OrderBy(game => game.Name)
                .ToList();
        }

        var totalCount = games.Count;

        var paginatedGames = filter.PageSize > 0
            ? games.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToList()
            : games;

        return (paginatedGames, totalCount);
    }

    public async Task<Game?> GetByIdAsync(int id)
    {
        return await dbContext.Games.FindAsync(id);
    }

    public async Task<Game> AddAsync(Game game)
    {
        dbContext.Games.Add(game);
        await dbContext.SaveChangesAsync();
        return game;
    }

    public async Task UpdateAsync(Game game)
    {
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await dbContext.Games
            .Where(game => game.Id == id)
            .ExecuteDeleteAsync();
    }
}