using GameStore.API.Dtos.Genres;

namespace GameStore.API.Services.Genres;

public interface IGenreService
{
    Task<List<GenreDetailsDto>> GetGenresAsync(CancellationToken cancellationToken);
    Task<GenreDetailsDto?> GetGenreByIdAsync(int id, CancellationToken cancellationToken);
    Task<GenreDetailsDto> AddGenreAsync(CreateGenreDto createGenre, CancellationToken cancellationToken);
    Task PatchGenreAsync(int id, PatchGenreDto patchGenre, CancellationToken cancellationToken);
    Task DeleteGenreAsync(int id, CancellationToken cancellationToken);
}