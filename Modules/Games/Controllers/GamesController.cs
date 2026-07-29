using GameStore.API.Common.Request;
using GameStore.API.Common.Responses;
using GameStore.API.Modules.Games.Dtos;
using GameStore.API.Modules.Games.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Modules.Games.Controllers;

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

    [HttpPost]
    public async Task<ActionResult<GameSummaryDto>> AddGame(CreateGameDto createdGame,CancellationToken cancellationToken)
    {
        var game = await gameService.AddGameAsync(createdGame, cancellationToken);
        return Ok(game);
    }

    [HttpPatch("update")]
    public async Task<ActionResult> PatchGame(PatchGameDto patchGame, CancellationToken cancellationToken)
    {
        await gameService.PatchGameAsync(patchGame.Id, patchGame, cancellationToken);
        return Ok();
    }

    [HttpDelete("remove")]
    public async Task<ActionResult> DeleteGame(IdRequest request, CancellationToken cancellationToken)
    {
        await gameService.DeleteGameAsync(request.Id, cancellationToken);
        return Ok();
    }
}
