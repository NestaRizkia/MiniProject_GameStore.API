using GameStore.API.Models;
using GameStore.API.Dtos.Genres;
using GameStore.API.Repositories.Genres;

namespace GameStore.API.Services.Genres;

public class GenreService(IGenreRepository genreRepository) : IGenreService
{
    public async Task<List<GenreDetailsDto>> GetGenresAsync()
    {
        var genres = await genreRepository.GetAllAsync();
        return genres.Select(genre => new GenreDetailsDto(
            genre.Id,
            genre.Name
        )).ToList();
    }

    public async Task<GenreDetailsDto?> GetGenreByIdAsync(int id)
    {
        var genre = await genreRepository.GetByIdAsync(id);

        if (genre is null)
        {
            return null;
        }

        return new GenreDetailsDto(
            genre.Id,
            genre.Name
        );
    }

    public async Task<GenreDetailsDto> AddGenreAsync(CreateGenreDto createGenre)
    {
        var genre = new Genre
        {
            Name = createGenre.Name
        };

        var result = await genreRepository.AddAsync(genre);

        return new GenreDetailsDto(
            result.Id,
            result.Name
        );
    }

    public async Task UpdateGenreAsync(int id, UpdateGenreDto updatedGenre)
    {
        var existingGenre = await genreRepository.GetByIdAsync(id);
        if (existingGenre is null)
        {
            throw new KeyNotFoundException($"Genre with id {id} not Found");
        }

        existingGenre.Name = updatedGenre.Name;

        await genreRepository.UpdateAsync(existingGenre);
    }

    public async Task DeleteGenreAsync(int id)
    {
        await genreRepository.DeleteAsync(id);
    }
}