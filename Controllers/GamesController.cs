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
    public async Task<ActionResult<PaginatedResult<GameSummaryDto>>> GetGames([FromQuery] GameFilterDto filter)
    {
        var result = await gameService.GetFilteredGamesAsync(filter);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GameDetailsDto>> GetGameById (int id)
    {
        var game = await gameService.GetGameByIdAsync(id);
        if(game is null)
        {
            return NotFound();
        }

        return Ok(game);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<GameDetailsDto>> AddGame(CreateGameDto createdGame)
    {
        var game = await gameService.AddGameAsync(createdGame);
        return CreatedAtAction(nameof(GetGameById), new { id = game.Id}, game);
    }

    [Authorize]
    [HttpPatch("{id}")]
    public async Task<ActionResult> PatchGame(int id, PatchGameDto patchGame)
    {
        await gameService.PatchGameAsync(id, patchGame);
        return Ok();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteGame(int id)
    {
        await gameService.DeleteGameAsync(id);
        return NoContent();
    }
}
