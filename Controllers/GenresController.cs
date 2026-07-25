using GameStore.API.Dtos.Genres;
using GameStore.API.Services;
using GameStore.API.Services.Genres;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers;

[ApiController]
[Route("genres")]
public class GenresController(IGenreService genreService, HashidService hashidService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<GenreDetailsDto>>> GetGenres(CancellationToken cancellationToken)
    {
        var genres = await genreService.GetGenresAsync(cancellationToken);
        return Ok(genres);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GenreDetailsDto>> GetGenreById(string id, CancellationToken cancellationToken)
    {
        var decodedId = hashidService.Decode(id);

        var genre = await genreService.GetGenreByIdAsync(decodedId, cancellationToken);
        if (genre is null)
        {
            return NotFound();
        }

        return Ok(genre);
    }

    [Authorize(Policy = "WriteGenresPolicy")]
    [HttpPost]
    public async Task<ActionResult<GenreDetailsDto>> AddGenre(CreateGenreDto createGenre, CancellationToken cancellationToken)
    {
        var genre = await genreService.AddGenreAsync(createGenre, cancellationToken);
        return CreatedAtAction(nameof(GetGenreById), new { id = hashidService.Encode(genre.Id) }, genre);
    }

    [Authorize(Policy = "WriteGenresPolicy")]
    [HttpPatch("{id}")]
    public async Task<ActionResult> PatchGenre(string id, PatchGenreDto patchGenre, CancellationToken cancellationToken)
    {
        var decodedId = hashidService.Decode(id);

        await genreService.PatchGenreAsync(decodedId, patchGenre, cancellationToken);
        return NoContent();
    }

    [Authorize(Policy = "WriteGenresPolicy")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteGenre(string id, CancellationToken cancellationToken)
    {
        var decodedId = hashidService.Decode(id);

        await genreService.DeleteGenreAsync(decodedId, cancellationToken);
        return NoContent();
    }
}
