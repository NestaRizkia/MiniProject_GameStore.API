using GameStore.API.Data;
using GameStore.API.Middlewares;
using GameStore.API.Repositories.Games;
using GameStore.API.Repositories.Genres;
using GameStore.API.Services.Games;
using GameStore.API.Services.Genres;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Register repositories
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();

// Register services
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IGenreService, GenreService>();

// Configure Database
builder.Services.AddDbContext<GameStoreContext>(options => 
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("neonconnection"));
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseExceptionHandler();
app.MapControllers();
app.Run();
