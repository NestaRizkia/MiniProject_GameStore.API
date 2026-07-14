using GameStore.API.Dtos.Genres;

namespace GameStore.API.Services.Genres;

public interface IGenreService
{
    Task<List<GenreDetailsDto>> GetGenresAsync();
    Task<GenreDetailsDto?> GetGenreByIdAsync(int id);
    Task<GenreDetailsDto> AddGenreAsync(CreateGenreDto createGenre);
    Task UpdateGenreAsync(int id, UpdateGenreDto updateGenre);
    Task DeleteGenreAsync(int id);
}