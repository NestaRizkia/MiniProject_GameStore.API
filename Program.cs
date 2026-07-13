using GameStore.API.Data;
using GameStore.API.Endpoints;
using GameStore.API.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation(); 
builder.AddGameStoreDb();

var app = builder.Build();

app.MapGamesEndpoints();

app.MigrateDb();

app.Run();
