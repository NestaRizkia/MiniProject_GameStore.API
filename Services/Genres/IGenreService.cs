using GameStore.API.Dtos.Genres;

namespace GameStore.API.Services.Genres;

public interface IGenreService
{
    Task<List<GenreDetailsDto>> GetGenresAsync();
    Task<GenreDetailsDto?> GetGenreByIdAsync(int id);
    Task<GenreDetailsDto> AddGenreAsync(CreateGenreDto createGenre);
    Task PatchGenreAsync(int id, PatchGenreDto patchGenre);
    Task DeleteGenreAsync(int id);
}