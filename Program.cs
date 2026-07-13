using GameStore.API.Dtos;

const string GetGameEndpointName = "GetGame";

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

List<GameDto> games =
[
    new (1, "EA fc 27", "Sport", 19.99M, new DateOnly(2026, 09, 01)),
    new (2, "EA fc 26", "Sport", 9.9M, new DateOnly(2025, 09, 01)),
    new (3, "Street Fighter II", "Fighting", 19.9M, new DateOnly(1992, 7, 15)),
    new (4, "Harvest Moon: Back to Nature", "Simulation", 5, new DateOnly(2010, 11, 27))
    
];

// Get Method
app.MapGet("/games", () => games);

// Get by Id Method
app.MapGet("/games/{id}", (int id) => games.Find(game => game.Id == id))
    .WithName(GetGameEndpointName);

// Post Method: Use Dto for create data Game
app.MapPost("/games", (CreateGameDto newGame) =>
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
app.MapPut("/games/{id}", (int id,UpdateGameDto updatedGame) =>
    {
    var index = games.FindIndex(game => game.Id == id);

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
app.MapDelete("/games/{id}", (int id) =>
    {
      games.RemoveAll(game => game.Id == id);

      return Results.NoContent();   
    }
);

app.Run();
