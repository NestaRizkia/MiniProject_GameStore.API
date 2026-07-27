using GameStore.API.Dtos;
using GameStore.API.Dtos.Games;
using GameStore.API.Services.Interfaces;
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

    [HttpPost("details")]
    public async Task<ActionResult<GameSummaryDto>> GetGameById(IdRequest request, CancellationToken cancellationToken)
    {
        var game = await gameService.GetGameByIdAsync(request.Id, cancellationToken);
        if(game is null)
        {
            return NotFound();
        }

        return Ok(game);
    }

    [Authorize(Policy = "WriteGamesPolicy")]
    [HttpPost]
    public async Task<ActionResult<GameSummaryDto>> AddGame(CreateGameDto createdGame,CancellationToken cancellationToken)
    {
        var game = await gameService.AddGameAsync(createdGame, cancellationToken);
        return Ok(game);
    }

    [Authorize(Policy = "WriteGamesPolicy")]
    [HttpPatch("update")]
    public async Task<ActionResult> PatchGame(PatchGameDto patchGame, CancellationToken cancellationToken)
    {
        await gameService.PatchGameAsync(patchGame.Id, patchGame, cancellationToken);
        return Ok();
    }

    [Authorize(Policy = "WriteGamesPolicy")]
    [HttpPost("remove")]
    public async Task<ActionResult> DeleteGame(IdRequest request, CancellationToken cancellationToken)
    {
        await gameService.DeleteGameAsync(request.Id, cancellationToken);
        return Ok();
    }
}