using GameStore.API.Data;
using GameStore.API.Repositories.Games;
using GameStore.API.Services.Games; 

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddValidation(); 
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IGameService, GameService>();
builder.AddGameStoreDb();

var app = builder.Build();

app.MapControllers();

app.MigrateDb();

app.Run();
