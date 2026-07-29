using GameStore.API.Modules.Games.Repositories;
using GameStore.API.Modules.Games.Repositories.Interfaces;
using GameStore.API.Modules.Games.Services;
using GameStore.API.Modules.Games.Services.Interfaces;

namespace GameStore.API.Modules.Games;

public static class GamesModule
{
    public static IServiceCollection AddGamesModule(this IServiceCollection services)
    {
        services.AddScoped<IGameRepository, GameRepository>();
        services.AddScoped<IGameService, GameService>();
        return services;
    }
}
