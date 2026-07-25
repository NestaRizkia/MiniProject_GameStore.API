using GameStore.API.Dtos;
using GameStore.API.Dtos.Games;
using GameStore.API.Services;
using GameStore.API.Services.Games;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers;

[ApiController]
[Route("games")]
public class GamesController(IGameService gameService, HashidService hashidService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedResult<GameSummaryDto>>> GetGames([FromQuery] GameFilterDto filter, CancellationToken cancellationToken)
    {
        var result = await gameService.GetFilteredGamesAsync(filter, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GameDetailsDto>> GetGameById (string id, CancellationToken cancellationToken)
    {
        var decodedId = hashidService.Decode(id);

        var game = await gameService.GetGameByIdAsync(decodedId, cancellationToken);
        if(game is null)
        {
            return NotFound();
        }

        return Ok(game);
    }

    [Authorize(Policy = "WriteGamesPolicy")]
    [HttpPost]
    public async Task<ActionResult<GameDetailsDto>> AddGame(CreateGameDto createdGame,CancellationToken cancellationToken)
    {
        var game = await gameService.AddGameAsync(createdGame, cancellationToken);
        return CreatedAtAction(nameof(GetGameById), new { id = hashidService.Encode(game.Id) }, game);
    }

    [Authorize(Policy = "WriteGamesPolicy")]
    [HttpPatch("{id}")]
    public async Task<ActionResult> PatchGame(string id, PatchGameDto patchGame, CancellationToken cancellationToken)
    {
        var decodedId = hashidService.Decode(id);

        await gameService.PatchGameAsync(decodedId, patchGame, cancellationToken);
        return Ok();
    }

    [Authorize(Policy = "WriteGamesPolicy")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteGame(string id, CancellationToken cancellationToken)
    {
        var decodedId = hashidService.Decode(id);

        await gameService.DeleteGameAsync(decodedId, cancellationToken);
        return NoContent();
    }
}
