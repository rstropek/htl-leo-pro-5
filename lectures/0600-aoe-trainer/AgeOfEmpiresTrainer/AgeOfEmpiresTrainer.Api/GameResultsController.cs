namespace AgeOfEmpiresTrainer.Api;

[Route("api/[controller]")]
[ApiController]
public class GameResultsController : ControllerBase
{
    private readonly GameResultManager manager;

    public GameResultsController(GameResultManager manager)
    {
        this.manager = manager;
    }

    [HttpGet]
    public async Task<IEnumerable<GameResultManager.GameResultSummary>> GetGameResults(
        [FromQuery][Range(1, 3)] int? numberOfAIPlayers = null,
        [FromQuery] Civilization? civilization = null,
        [FromQuery] GameStatus? gameStatus = null)
    {
        // Todo: Add required code for returning game results here.
        //throw new NotImplementedException();
        #region Solution
        return await manager.GetGameResults(numberOfAIPlayers, civilization, gameStatus);
        #endregion
    }

    [HttpPost]
    public async Task<ActionResult> AddGameResult(GameResult gameResult)
    {
        // Todo: Add required code for adding game results here.
        //throw new NotImplementedException();
        #region Solution
        try
        {
            await manager.AddGameResult(gameResult);
            return Created(string.Empty, gameResult);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Type = "https://aoetrainer.com/errors/invalid-game-result",
                Title = "Invalid game result",
                Detail = ex.Message,
            });
        }
        #endregion
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteGameResult(int id)
    {
        // Todo: Add required code for removing game results here.
        //throw new NotImplementedException();
        #region Solution
        return await manager.TryDeleteGameResult(id) ? NoContent() : NotFound();
        #endregion
    }
}
