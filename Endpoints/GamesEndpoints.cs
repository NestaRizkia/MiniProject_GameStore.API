using GameStore.API.Data;
using GameStore.API.Dtos.Games;
using GameStore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";

    public static void MapGamesEndpoints(this WebApplication app)
    {

        var group = app.MapGroup("/games");

        // Get Method
        group.MapGet("/", async (GameStoreContext dbContext) => 
            await dbContext.Games
                .Include(game => game.Genre)
                .Select(game => new GameSummaryDto(
                    game.Id,
                    game.Name,
                    game.Genre!.Name,
                    game.Price,
                    game.ReleaseDate
                ))
                .AsNoTracking()
                .ToListAsync()
        );

        // Get by Id Method
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
        var game = await dbContext.Games.FindAsync(id);

        return game is null? Results.NotFound() : Results.Ok(
            new GameDetailsDto(
                game.Id,
                game.Name,
                game.GenreId,
                game.Price,
                game.ReleaseDate
            )
        ); 
        }) .WithName(GetGameEndpointName);

        // Post Method: Use Dto for create data Game
        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
            {
                Game game = new ()
                {
                  Name = newGame.Name,
                  GenreId = newGame.GenreId,
                  Price = newGame.Price,
                  ReleaseDate = newGame.ReleaseDate
                };

                dbContext.Games.Add(game);
                await dbContext.SaveChangesAsync();

                GameDetailsDto gameDto = new(
                    game.Id,
                    game.Name,
                    game.GenreId,
                    game.Price,
                    game.ReleaseDate
                );

                return Results.CreatedAtRoute(GetGameEndpointName, new { id = gameDto.Id}, gameDto);
            }
        );

        // Put Method: Use Dto for update data Game
        group.MapPut("/{id}", async (int id,UpdateGameDto updatedGame, GameStoreContext dbContext) =>
            {
            var existingGame = await dbContext.Games.FindAsync(id);

            if(existingGame is null)
            {
                return Results.NotFound();
            }

            existingGame.Name = updatedGame.Name;
            existingGame.GenreId = updatedGame.GenreId;
            existingGame.Price = updatedGame.Price;
            existingGame.ReleaseDate = updatedGame.ReleaseDate;

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
            }
        );

        // Delete Method: Check the same id parameter with the actual object id
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
            {
                await dbContext.Games
                    .Where(game => game.Id == id)
                    .ExecuteDeleteAsync();

                return Results.NoContent();   
            }
        );
    }

}