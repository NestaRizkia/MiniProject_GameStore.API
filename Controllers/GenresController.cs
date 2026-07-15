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
    public async Task<ActionResult<List<GenreDetailsDto>>> GetGenres()
    {
        var genres = await genreService.GetGenresAsync();
        return Ok(genres);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GenreDetailsDto>> GetGenreById(int id)
    {
        var genre = await genreService.GetGenreByIdAsync(id);
        if (genre is null)
        {
            return NotFound();
        }

        return Ok(genre);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<GenreDetailsDto>> AddGenre(CreateGenreDto createGenre)
    {
        var genre = await genreService.AddGenreAsync(createGenre);
        return CreatedAtAction(nameof(GetGenreById), new { id = genre.Id }, genre);
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("{id}")]
    public async Task<ActionResult> PatchGenre(int id, PatchGenreDto patchGenre)
    {
        await genreService.PatchGenreAsync(id, patchGenre);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteGenre(int id)
    {
        await genreService.DeleteGenreAsync(id);
        return NoContent();
    }
}
