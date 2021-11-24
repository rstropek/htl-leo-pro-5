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
    /// Helper method turning a <see cref="GameSetup"/> into a <see cref="GameSetupSummary"/>.
    /// </summary>
    /// <param name="g">Game setup to convert</param>
    /// <param name="url">URL helper (you get get it from the <see cref="ControllerBase.Url"/> property)</param>
    /// <param name="request">Current HTTP request (you get get it from the <see cref="ControllerBase.Request"/> property)</param>
    private static GameSetupSummary ProjectGameSetupToSummary(GameSetup g, IUrlHelper url, HttpRequest request)
        => new(g.Id,
            g.Description,
            new(
                g.StartingRoomId,
                g.StartingRoom!.Description,
                RoomsController.GetRoomDetailsUrl(g.StartingRoomId, url, request)),
            g.InitialLifePower,
            g.InitialAttackStrength,
            g.InitialArmorStrength);

    /// <summary>
    /// Gets a filtered list of all game setups. You get all game setups whose
    /// name contains the given description filter.
    /// </summary>
    [HttpGet]
    public ActionResult<IEnumerable<GameSetupSummary>> GetGameSetups([FromQuery] string? descriptionFilter)
        => Ok(context.Games
                .Where(g => descriptionFilter == null || g.Description.Contains(descriptionFilter))
                .Include(g => g.StartingRoom)
                .Select(g => ProjectGameSetupToSummary(g, Url, Request)));

    /// <summary>
    /// Get a single game setup by id
    /// </summary>
    /// <param name="id">ID of the game setup to get</param>
    [HttpGet("{id}", Name = nameof(GetGameSetup))]
    [ProducesResponseType(typeof(GameSetupSummary), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GameSetupSummary>> GetGameSetup(int id)
    {
        var gameSetup = await context.Games
            .Include(g => g.StartingRoom)
            .FirstOrDefaultAsync(r => r.Id == id);
        if (gameSetup == null) { return NotFound(); }

        return Ok(ProjectGameSetupToSummary(gameSetup, Url, Request));
    }

    /// <summary>
    /// Creates a new game setup
    /// </summary>
    /// <param name="gameSetup">Game setup to create</param>
    [HttpPost]
    [ProducesResponseType(typeof(GameSetupSummary), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GameSetupSummary>> AddGameSetup(NewGameSetup gameSetup)
    {
        // Verify that starting room exists
        var room = await context.Rooms.FirstOrDefaultAsync(r => r.Id == gameSetup.StartingRoomId);
        if (room == null) { return NotFound(); }

        var newGameSetup = new GameSetup
        {
            Description = gameSetup.Description,
            StartingRoom = room,
            StartingRoomId = gameSetup.StartingRoomId,
            InitialArmorStrength = gameSetup.InitialArmorStrength,
            InitialAttackStrength = gameSetup.InitialAttackStrength,
            InitialLifePower = gameSetup.InitialLifePower,
        };
        context.Games.Add(newGameSetup);

        await context.SaveChangesAsync();
        return Created(string.Empty, ProjectGameSetupToSummary(newGameSetup, Url, Request));
    }

    /// <summary>
    /// Updates a game setup
    /// </summary>
    /// <param name="id">ID of the game setup to update</param>
    /// <param name="gameSetup">Game setup details</param>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(GameSetupSummary), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GameSetupSummary>> UpdateRoom(int id, NewGameSetup gameSetup)
    {
        var gs = await context.Games.FirstOrDefaultAsync(r => r.Id == id);
        if (gs == null) { return NotFound(); }

        // Verify that starting room exists
        var room = await context.Rooms.FirstOrDefaultAsync(r => r.Id == gameSetup.StartingRoomId);
        if (room == null) { return NotFound(); }

        // Update properties
        // Are you getting tired of copying properties around? In practice, you should
        // use libraries like "AutoMapper" for that. However, that is out-of-scope of this course.
        gs.Description = gameSetup.Description;
        gs.StartingRoom = room;
        gs.StartingRoomId = room.Id;
        gs.InitialArmorStrength = gameSetup.InitialArmorStrength;
        gs.InitialAttackStrength = gameSetup.InitialAttackStrength;
        gs.InitialLifePower = gameSetup.InitialLifePower;

        await context.SaveChangesAsync();

        return Ok(ProjectGameSetupToSummary(gs, Url, Request));
    }

    /// <summary>
    /// Deletes the game setup with the given id
    /// </summary>
    /// <param name="id">ID of the game setup to delete</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteGameSetup(int id)
    {
        var gs = await context.Games.FirstOrDefaultAsync(r => r.Id == id);
        if (gs == null) { return NotFound(); }

        context.Games.Remove(gs);
        await context.SaveChangesAsync();
        return NoContent();
    }
}
