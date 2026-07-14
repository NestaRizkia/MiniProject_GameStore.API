using GameStore.API.Dtos.Games;
using GameStore.API.Services.Games;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers;

[ApiController]
[Route("games")]
public class GamesController(IGameService gameService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<GameSummaryDto>>> GetGames()
    {
        var games = await gameService.GetGamesAsync();
        return Ok(games);
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
    [HttpPost]
    public async Task<ActionResult<GameDetailsDto>> AddGame(CreateGameDto createdGame)
    {
        var game = await gameService.AddGameAsync(createdGame);
        return CreatedAtAction(nameof(GetGameById), new { id = game.Id}, game);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateGame(int id, UpdateGameDto updatedGame)
    {
        try
        {
            await gameService.UpdateGameAsync(id, updatedGame);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteGame(int id)
    {
        await gameService.DeleteGameAsync(id);
        return NoContent();
    }
}
