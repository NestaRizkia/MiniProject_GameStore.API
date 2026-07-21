using GameStore.API.Models;
using GameStore.API.Dtos.Genres;
using GameStore.API.Repositories.Genres;

namespace GameStore.API.Services.Genres;

public class GenreService(IGenreRepository genreRepository) : IGenreService
{
    public async Task<List<GenreDetailsDto>> GetGenresAsync(CancellationToken cancellationToken)
    {
        var genres = await genreRepository.GetAllAsync(cancellationToken);
        return genres.Select(genre => new GenreDetailsDto(
            genre.Id,
            genre.Name
        )).ToList();
    }

    public async Task<GenreDetailsDto?> GetGenreByIdAsync(int id, CancellationToken cancellationToken)
    {
        var genre = await genreRepository.GetByIdAsync(id, cancellationToken);

        if (genre is null)
        {
            return null;
        }

        return new GenreDetailsDto(
            genre.Id,
            genre.Name
        );
    }

    public async Task<GenreDetailsDto> AddGenreAsync(CreateGenreDto createGenre, CancellationToken cancellationToken)
    {
        var genre = new Genre
        {
            Name = createGenre.Name
        };

        var result = await genreRepository.AddAsync(genre, cancellationToken);

        return new GenreDetailsDto(
            result.Id,
            result.Name
        );
    }

    public async Task PatchGenreAsync(int id, PatchGenreDto patchGenre, CancellationToken cancellationToken)
    {
        var existingGenre = await genreRepository.GetByIdAsync(id, cancellationToken);
        if (existingGenre is null)
        {
            throw new KeyNotFoundException($"Genre with id {id} not Found");
        }

        // Only update fields that are provided (not null)
        if (patchGenre.Name != null)
        {
            existingGenre.Name = patchGenre.Name;
        }

        await genreRepository.UpdateAsync(existingGenre, cancellationToken);
    }

    public async Task DeleteGenreAsync(int id, CancellationToken cancellationToken)
    {
        await genreRepository.DeleteAsync(id, cancellationToken);
    }
}