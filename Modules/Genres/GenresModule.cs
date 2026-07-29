using GameStore.API.Modules.Genres.Repositories;
using GameStore.API.Modules.Genres.Repositories.Interfaces;
using GameStore.API.Modules.Genres.Services;
using GameStore.API.Modules.Genres.Services.Interfaces;

namespace GameStore.API.Modules.Genres;

public static class GenresModule
{
    public static IServiceCollection AddGenresModule(this IServiceCollection services)
    {
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<IGenreService, GenreService>();
        return services;
    }
}
