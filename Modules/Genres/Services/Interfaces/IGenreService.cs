using GameStore.API.Modules.Genres.Dtos;

namespace GameStore.API.Modules.Genres.Services.Interfaces;

public interface IGenreService
{
    Task<List<GenreDetailsDto>> GetGenresAsync(CancellationToken cancellationToken);
    Task<GenreDetailsDto?> GetGenreByIdAsync(int id, CancellationToken cancellationToken);
    Task<GenreDetailsDto> AddGenreAsync(CreateGenreDto createGenre, CancellationToken cancellationToken);
    Task PatchGenreAsync(int id, PatchGenreDto patchGenre, CancellationToken cancellationToken);
    Task DeleteGenreAsync(int id, CancellationToken cancellationToken);
}
