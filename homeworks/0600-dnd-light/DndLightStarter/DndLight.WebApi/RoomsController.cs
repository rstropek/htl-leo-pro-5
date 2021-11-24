namespace DndLight.WebApi;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class RoomsController : ControllerBase
{
    private readonly DndContext context;

    #region DTOs
    public record RoomSummary(
        int Id,
        string Description,
        string RoomDetailsUrl);

    public record NewRoomDoor(
        int LinkedRoomId,
        string Description,
        bool InitiallyLocked,
        ItemType? RequiredItemToUnlock);

    public record NewRoomItem(
        string Description,
        ItemType ItemType);

    public record NewRoomMonster(
        string Description,
        bool AttacksOnEntry,
        byte LifePower,
        byte AttackStrength,
        byte ArmorStrength);

    public record NewRoom(string Description);

    public record NewRoomDetails(
        string Description,
        [Required] List<NewRoomDoor> Doors,
        [Required] List<NewRoomItem> Items,
        [Required] List<NewRoomMonster> Monsters);

    public record RoomDoorResult(
        int Id,
        int LinkedRoomId,
        string Description,
        bool InitiallyLocked,
        ItemType? RequiredItemToUnlock);

    public record RoomItemResult(
        int Id,
        string Description,
        ItemType ItemType);

    public record RoomMonsterResult(
        int Id,
        string Description,
        bool AttacksOnEntry,
        byte LifePower,
        byte AttackStrength,
        byte ArmorStrength);

    public record RoomDetailsResult(
        int Id,
        string Description,
        [Required] List<RoomDoorResult> Doors,
        [Required] List<RoomItemResult> Items,
        [Required] List<RoomMonsterResult> Monsters);
    #endregion

    public RoomsController(DndContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Gets a list of all rooms
    /// </summary>
    [HttpGet]
    public ActionResult<IEnumerable<RoomSummary>> GetRooms()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get a single room by id
    /// </summary>
    /// <param name="id">ID of the room to get</param>
    [HttpGet("{id}", Name = nameof(GetRoom))]
    [ProducesResponseType(typeof(RoomDetailsResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<ActionResult<RoomDetailsResult>> GetRoom(int id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Creates a new room
    /// </summary>
    /// <param name="room">Room to create</param>
    [HttpPost]
    [ProducesResponseType(typeof(Room), StatusCodes.Status201Created)]
    public Task<ActionResult<Room>> AddRoom(NewRoom room)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Updates a room. Replaces all existing doors, items, and monsters with the given room details.
    /// </summary>
    /// <param name="id">ID of the room to update</param>
    /// <param name="roomDetails">New room details</param>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(RoomDetailsResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<ActionResult<RoomDetailsResult>> UpdateRoom(int id, NewRoomDetails roomDetails)
    {
        throw new NotImplementedException();
    }


    /// <summary>
    /// Deletes the room with the given id including all of the room's monsters, items, and doors
    /// </summary>
    /// <param name="id">ID of the room to delete</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<ActionResult> DeleteRoom(int id)
    {
        throw new NotImplementedException();
    }
}
