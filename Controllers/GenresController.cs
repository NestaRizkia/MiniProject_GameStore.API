using GameStore.API.Dtos.Genres;
using GameStore.API.Services.Genres;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers;

[ApiController]
[Route("genres")]
public class GenresController(IGenreService genreService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<GenreDetailsDto>>> GetGenres(CancellationToken cancellationToken)
    {
        var genres = await genreService.GetGenresAsync(cancellationToken);
        return Ok(genres);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GenreDetailsDto>> GetGenreById(int id, CancellationToken cancellationToken)
    {
        var genre = await genreService.GetGenreByIdAsync(id, cancellationToken);
        if (genre is null)
        {
            return NotFound();
        }

        return Ok(genre);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<GenreDetailsDto>> AddGenre(CreateGenreDto createGenre, CancellationToken cancellationToken)
    {
        var genre = await genreService.AddGenreAsync(createGenre, cancellationToken);
        return CreatedAtAction(nameof(GetGenreById), new { id = genre.Id }, genre);
    }

    [Authorize]
    [HttpPatch("{id}")]
    public async Task<ActionResult> PatchGenre(int id, PatchGenreDto patchGenre, CancellationToken cancellationToken)
    {
        await genreService.PatchGenreAsync(id, patchGenre, cancellationToken);
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteGenre(int id, CancellationToken cancellationToken)
    {
        await genreService.DeleteGenreAsync(id, cancellationToken);
        return NoContent();
    }
}
