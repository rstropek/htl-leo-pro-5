namespace DndLight.WebApi;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class GameSetupController : ControllerBase
{
    private readonly DndContext context;

    #region DTOs
    public record GameSetupSummary(
        int Id,
        string Description,
        RoomsController.RoomSummary StartingRoom,
        byte InitialLifePower,
        byte InitialAttackStrength,
        byte InitialArmorStrength);

    public record NewGameSetup(
        string Description,
        int StartingRoomId,
        byte InitialLifePower,
        byte InitialAttackStrength,
        byte InitialArmorStrength);
    #endregion

    public GameSetupController(DndContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Gets a filtered list of all game setups. You get all game setups whose
    /// name contains the given description filter.
    /// </summary>
    [HttpGet]
    public ActionResult<IEnumerable<GameSetupSummary>> GetGameSetups([FromQuery] string? descriptionFilter)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get a single game setup by id
    /// </summary>
    /// <param name="id">ID of the game setup to get</param>
    [HttpGet("{id}", Name = nameof(GetGameSetup))]
    [ProducesResponseType(typeof(GameSetupSummary), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<ActionResult<GameSetupSummary>> GetGameSetup(int id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Creates a new game setup
    /// </summary>
    /// <param name="gameSetup">Game setup to create</param>
    [HttpPost]
    [ProducesResponseType(typeof(GameSetupSummary), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<ActionResult<GameSetupSummary>> AddGameSetup(NewGameSetup gameSetup)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Updates a game setup
    /// </summary>
    /// <param name="id">ID of the game setup to update</param>
    /// <param name="gameSetup">Game setup details</param>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(GameSetupSummary), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<ActionResult<GameSetupSummary>> UpdateRoom(int id, NewGameSetup gameSetup)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Deletes the game setup with the given id
    /// </summary>
    /// <param name="id">ID of the game setup to delete</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<ActionResult> DeleteGameSetup(int id)
    {
        throw new NotImplementedException();
    }
}
