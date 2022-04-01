using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WoerdleSolver.Logic;

namespace WoerdleSolver.WebApi.Controllers;

[Route("api/[controller]/game")]
[ApiController]
public class SolverController : ControllerBase
{
    private readonly SolverContext context;
    private readonly ISolverFactory factory;

    public record GameIdDto(int GameId);

    public record StoreResultDto(string Word, string Result);

    public SolverController(SolverContext context, ISolverFactory factory)
    {
        this.context = context;
        this.factory = factory;
    }

    /// <summary>
    /// Creates a new game
    /// </summary>
    /// <response code="201">Game created</response>
    [HttpPost(Name = nameof(NewGame))]
    public async Task<ActionResult<GameIdDto>> NewGame()
    {
        var newGame = new Game();
        context.Games.Add(newGame);
        await context.SaveChangesAsync();

        return Created(string.Empty, new GameIdDto(newGame.Id));
    }

    /// <summary>
    /// Stores a guess for an existing game
    /// </summary>
    /// <response code="201">Result Stored</response>
    /// <response code="404">Game not found</response>
    /// <response code="400">Error in word or result</response>
    [HttpPost("{id}", Name = nameof(StoreResult))]
    public async Task<IActionResult> StoreResult(int id, StoreResultDto req)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Gets possible words for an existing game
    /// </summary>
    /// <response code="404">Game not found</response>
    [HttpGet("{id}", Name = nameof(Guess))]
    public async Task<ActionResult<IEnumerable<string>>> PossibleWords(int id)
    {
        throw new NotImplementedException();
    }
}
