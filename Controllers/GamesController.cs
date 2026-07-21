using GameStore.API.Dtos;
using GameStore.API.Dtos.Games;
using GameStore.API.Services.Games;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers;

[ApiController]
[Route("games")]
public class GamesController(IGameService gameService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedResult<GameSummaryDto>>> GetGames([FromQuery] GameFilterDto filter, CancellationToken cancellationToken)
    {
        var result = await gameService.GetFilteredGamesAsync(filter, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GameDetailsDto>> GetGameById (int id, CancellationToken cancellationToken)
    {
        var game = await gameService.GetGameByIdAsync(id, cancellationToken);
        if(game is null)
        {
            return NotFound();
        }

        return Ok(game);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<GameDetailsDto>> AddGame(CreateGameDto createdGame,CancellationToken cancellationToken)
    {
        var game = await gameService.AddGameAsync(createdGame, cancellationToken);
        return CreatedAtAction(nameof(GetGameById), new { id = game.Id}, game);
    }

    [Authorize]
    [HttpPatch("{id}")]
    public async Task<ActionResult> PatchGame(int id, PatchGameDto patchGame, CancellationToken cancellationToken)
    {
        await gameService.PatchGameAsync(id, patchGame, cancellationToken);
        return Ok();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteGame(int id, CancellationToken cancellationToken)
    {
        await gameService.DeleteGameAsync(id, cancellationToken);
        return NoContent();
    }
}
