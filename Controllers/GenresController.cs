using GameStore.API.Dtos;
using GameStore.API.Dtos.Genres;
using GameStore.API.Services.Interfaces;
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

    [HttpPost("details")]
    public async Task<ActionResult<GenreDetailsDto>> GetGenreById(IdRequest request, CancellationToken cancellationToken)
    {
        var genre = await genreService.GetGenreByIdAsync(request.Id, cancellationToken);
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
        return Ok(genre);
    }

    [Authorize(Policy = "WriteGenresPolicy")]
    [HttpPatch("update")]
    public async Task<ActionResult> PatchGenre(PatchGenreDto patchGenre, CancellationToken cancellationToken)
    {
        await genreService.PatchGenreAsync(patchGenre.Id, patchGenre, cancellationToken);
        return Ok();
    }

    [Authorize(Policy = "WriteGenresPolicy")]
    [HttpPost("remove")]
    public async Task<ActionResult> DeleteGenre(IdRequest request, CancellationToken cancellationToken)
    {
        await genreService.DeleteGenreAsync(request.Id, cancellationToken);
        return Ok();
    }
}