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

    // Todo: Adjust the return type of the following function if necessary.
    // Todo: Add parameters to the following function if necessary.
    [HttpGet]
    public Task<IEnumerable<object>> GetGameResults()
    {
        // Todo: Add required code for returning game results here.
        throw new NotImplementedException();
    }

    // Todo: Add parameters to the following function if necessary.
    [HttpPost]
    public Task<ActionResult> AddGameResult()
    {
        // Todo: Add required code for adding game results here.
        throw new NotImplementedException();
    }

    [HttpDelete("{id}")]
    public Task<ActionResult> DeleteGameResult(int id)
    {
        // Todo: Add required code for removing game results here.
        throw new NotImplementedException();
    }
}
