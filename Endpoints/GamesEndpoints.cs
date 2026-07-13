using GameStore.API.Dtos;

namespace GameStore.API.Endpoints;

public static class GamesEnpoints
{
    const string GetGameEndpointName = "GetGame";
    private static readonly List<GameDto> games =
    [
        new (1, "EA fc 27", "Sport", 19.99M, new DateOnly(2026, 09, 01)),
        new (2, "EA fc 26", "Sport", 9.9M, new DateOnly(2025, 09, 01)),
        new (3, "Street Fighter II", "Fighting", 19.9M, new DateOnly(1992, 7, 15)),
        new (4, "Harvest Moon: Back to Nature", "Simulation", 5, new DateOnly(2010, 11, 27))
        
    ];

    public static void MapGamesEndpoints(this WebApplication app)
    {

        var group = app.MapGroup("/games");

        // Get Method
        group.MapGet("/", () => games);

        // Get by Id Method
        group.MapGet("/{id}", (int id) =>
        {
        var game = games.Find(game => game.Id == id);

        return game is null? Results.NotFound() : Results.Ok(game); 
        }) .WithName(GetGameEndpointName);

        // Post Method: Use Dto for create data Game
        app.MapPost("/", (CreateGameDto newGame) =>
            {

                GameDto game = new
                (
                    games.Count + 1,
                    newGame.Name,
                    newGame.Genre,
                    newGame.Price,
                    newGame.ReleaseDate
                );

                games.Add(game);

                return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id}, game);
            }
        );

        // Put Method: Use Dto for update data Game
        app.MapPut("/{id}", (int id,UpdateGameDto updatedGame) =>
            {
            var index = games.FindIndex(game => game.Id == id);

            if(index < 0)
            {
                return Results.NotFound();
            }

            games[index] = new GameDto
            (
                    id,
                    updatedGame.Name,
                    updatedGame.Genre,
                    updatedGame.Price,
                    updatedGame.ReleaseDate
            );

            return Results.NoContent();
            }
        );

        // Delete Method: Check the same id parameter with the actual object id
        app.MapDelete("/{id}", (int id) =>
            {
            games.RemoveAll(game => game.Id == id);

            return Results.NoContent();   
            }
        );
    }

}