using GameStore.API.Data;
using GameStore.API.Middlewares;
using GameStore.API.Repositories.Games;
using GameStore.API.Repositories.Genres;
using GameStore.API.Services.Games;
using GameStore.API.Services.Genres;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IGenreService, GenreService>();

builder.Services.AddDbContext<GameStoreContext>(options => 
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("neonconnection"));
});

var app = builder.Build();

app.MapControllers();
app.UseExceptionHandler();
// Migrate DB
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
    dbContext.Database.Migrate();
}

app.Run();
