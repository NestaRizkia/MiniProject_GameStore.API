using GameStore.API.Models;
using GameStore.API.Data;
using GameStore.API.Dtos.Games;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace GameStore.API.Repositories.Games;

public class GameRepository(GameStoreContext dbContext) : IGameRepository
{
    public async Task<List<Game>> GetAllAsync()
    {
        return await dbContext.Games
            .FromSqlRaw(@"
                SELECT ""Id"", ""Name"", ""GenreId"", ""Price"", ""ReleaseDate"", ""CreatedAt"", ""UpdatedAt"" 
                FROM ""Games""")
            .Include(game => game.Genre)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Game?> GetByIdAsync(int id)
    {
        return await dbContext.Games.FindAsync(id);
    }

    public async Task<(List<Game> Games, int TotalCount)> GetFilteredGamesAsync(GameFilterDto filter)
    {
        // Build dynamic SQL query with filters
        var sql = @"
            SELECT ""Id"", ""Name"", ""GenreId"", ""Price"", ""ReleaseDate"", ""CreatedAt"", ""UpdatedAt"" 
            FROM ""Games"" 
            WHERE 1=1";

        var parameters = new List<NpgsqlParameter>();
        var paramIndex = 0;

        // Search filter (Name)
        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            sql += $" AND LOWER(""Name"") LIKE @p{paramIndex}";
            parameters.Add(new NpgsqlParameter($"@p{paramIndex}", $"%{filter.SearchTerm.ToLower()}%"));
            paramIndex++;
        }

        // Price range filter using BETWEEN
        if (filter.MinPrice.HasValue && filter.MaxPrice.HasValue)
        {
            sql += $" AND ""Price"" BETWEEN @p{paramIndex} AND @p{paramIndex + 1}";
            parameters.Add(new NpgsqlParameter($"@p{paramIndex}", filter.MinPrice.Value));
            parameters.Add(new NpgsqlParameter($"@p{paramIndex + 1}", filter.MaxPrice.Value));
            paramIndex += 2;
        }
        else if (filter.MinPrice.HasValue)
        {
            sql += $" AND ""Price"" >= @p{paramIndex}";
            parameters.Add(new NpgsqlParameter($"@p{paramIndex}", filter.MinPrice.Value));
            paramIndex++;
        }
        else if (filter.MaxPrice.HasValue)
        {
            sql += $" AND ""Price"" <= @p{paramIndex}";
            parameters.Add(new NpgsqlParameter($"@p{paramIndex}", filter.MaxPrice.Value));
            paramIndex++;
        }

        // Date range filter using BETWEEN
        if (filter.StartDate.HasValue && filter.EndDate.HasValue)
        {
            sql += $" AND ""ReleaseDate"" BETWEEN @p{paramIndex} AND @p{paramIndex + 1}";
            parameters.Add(new NpgsqlParameter($"@p{paramIndex}", filter.StartDate.Value));
            parameters.Add(new NpgsqlParameter($"@p{paramIndex + 1}", filter.EndDate.Value));
            paramIndex += 2;
        }
        else if (filter.StartDate.HasValue)
        {
            sql += $" AND ""ReleaseDate"" >= @p{paramIndex}";
            parameters.Add(new NpgsqlParameter($"@p{paramIndex}", filter.StartDate.Value));
            paramIndex++;
        }
        else if (filter.EndDate.HasValue)
        {
            sql += $" AND ""ReleaseDate"" <= @p{paramIndex}";
            parameters.Add(new NpgsqlParameter($"@p{paramIndex}", filter.EndDate.Value));
            paramIndex++;
        }

        // Get total count with filters
        var countSql = $"SELECT COUNT(*) FROM ({sql}) AS filtered";
        int totalCount;
        
        await using (var connection = dbContext.Database.GetDbConnection())
        {
            await connection.OpenAsync();
            await using var countCommand = connection.CreateCommand();
            countCommand.CommandText = countSql;
            foreach (var param in parameters)
            {
                countCommand.Parameters.Add(new NpgsqlParameter(param.ParameterName, param.Value));
            }
            totalCount = Convert.ToInt32(await countCommand.ExecuteScalarAsync());
        }

        // Add pagination
        sql += $@" ORDER BY ""Name"" 
                   OFFSET @offset ROWS 
                   FETCH NEXT @pageSize ROWS ONLY";

        parameters.Add(new NpgsqlParameter("@offset", (filter.PageNumber - 1) * filter.PageSize));
        parameters.Add(new NpgsqlParameter("@pageSize", filter.PageSize));

        // Execute query
        var games = await dbContext.Games
            .FromSqlRaw(sql, parameters.ToArray())
            .Include(game => game.Genre)
            .AsNoTracking()
            .ToListAsync();

        return (games, totalCount);
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