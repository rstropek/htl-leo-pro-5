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
        => Ok(context.Rooms
            .Select(r => new RoomSummary(
                r.Id,
                r.Description,
                GetRoomDetailsUrl(r.Id, Url, Request))));

    /// <summary>
    /// Get a single room by id
    /// </summary>
    /// <param name="id">ID of the room to get</param>
    [HttpGet("{id}", Name = nameof(GetRoom))]
    [ProducesResponseType(typeof(RoomDetailsResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RoomDetailsResult>> GetRoom(int id)
    {
        var room = await context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
        if (room == null) { return NotFound(); }

        return Ok(await GetRoomDetails(id, room.Description));
    }

    /// <summary>
    /// Creates a new room
    /// </summary>
    /// <param name="room">Room to create</param>
    [HttpPost]
    [ProducesResponseType(typeof(Room), StatusCodes.Status201Created)]
    public async Task<ActionResult<Room>> AddRoom(NewRoom room)
    {
        var newRoom = new Room { Description = room.Description };
        context.Rooms.Add(newRoom);
        await context.SaveChangesAsync();
        return Created(GetRoomDetailsUrl(newRoom.Id, Url, Request), newRoom);
    }

    /// <summary>
    /// Updates a room. Replaces all existing doors, items, and monsters with the given room details.
    /// </summary>
    /// <param name="id">ID of the room to update</param>
    /// <param name="roomDetails">New room details</param>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(RoomDetailsResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RoomDetailsResult>> UpdateRoom(int id, NewRoomDetails roomDetails)
    {
        var room = await context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
        if (room == null) { return NotFound(); }

        // Verify that room IDs for all doors exist
        foreach (var d in roomDetails.Doors)
        {
            if (await context.Rooms.CountAsync(r => r.Id == d.LinkedRoomId) != 1)
            {
                return NotFound();
            }
        }

        // Update room data
        room.Description = roomDetails.Description;

        // Replace all doors
        context.Doors.RemoveRange(await context.Doors.Where(d => d.LinkedRoom1Id == id).ToArrayAsync());
        context.Doors.AddRange(roomDetails.Doors.Select(d => new Door
        {
            LinkedRoom1Id = id,
            LinkedRoom2Id = d.LinkedRoomId,
            Description = d.Description,
            InitiallyLocked = d.InitiallyLocked,
            RequiredItemToUnlock = d.RequiredItemToUnlock
        }));

        // Replace all items
        context.Items.RemoveRange(await context.Items.Where(i => i.RoomId == id).ToArrayAsync());
        context.Items.AddRange(roomDetails.Items.Select(d => new Item
        {
            RoomId = id,
            Description = d.Description,
            ItemType = d.ItemType
        }));

        // Replace all monsters
        context.Monsters.RemoveRange(await context.Monsters.Where(i => i.RoomId == id).ToArrayAsync());
        context.Monsters.AddRange(roomDetails.Monsters.Select(d => new Monster
        {
            RoomId = id,
            Description = d.Description,
            ArmorStrength = d.ArmorStrength,
            AttacksOnEntry = d.AttacksOnEntry,
            AttackStrength = d.AttackStrength,
            LifePower = d.LifePower,
        }));

        await context.SaveChangesAsync();
        return Ok(await GetRoomDetails(id, room.Description));
    }

    /// <summary>
    /// Deletes the room with the given id including all of the room's monsters, items, and doors
    /// </summary>
    /// <param name="id">ID of the room to delete</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteRoom(int id)
    {
        var room = await context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
        if (room == null) { return NotFound(); }

        // Remove dependent data
        context.Monsters.RemoveRange(await context.Monsters.Where(m => m.RoomId == id).ToArrayAsync());
        context.Items.RemoveRange(await context.Items.Where(i => i.RoomId == id).ToArrayAsync());
        context.Doors.RemoveRange(await context.Doors.Where(d => d.LinkedRoom1Id == id || d.LinkedRoom2Id == id).ToArrayAsync());

        // Remove room
        context.Rooms.Remove(room);

        await context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Get room details for a given id and description
    /// </summary>
    /// <remarks>
    /// Note that this method is not ideal. You could read doors, items, and monsters
    /// in parallel. For simplicity reasons, this method does it sequentially.
    /// </remarks>
    private async Task<RoomDetailsResult> GetRoomDetails(int id, string description)
        => new RoomDetailsResult(
            id,
            description,
            await context.Doors
                .Where(d => d.LinkedRoom1Id == id)
                .Select(d => new RoomDoorResult(
                    d.Id,
                    d.LinkedRoom2Id,
                    d.Description,
                    d.InitiallyLocked,
                    d.RequiredItemToUnlock))
                .ToListAsync(),
            await context.Items
                .Where(i => i.RoomId == id)
                .Select(i => new RoomItemResult(
                    i.Id,
                    i.Description,
                    i.ItemType))
                .ToListAsync(),
            await context.Monsters
                .Where(m => m.RoomId == id)
                .Select(m => new RoomMonsterResult(
                    m.Id,
                    m.Description,
                    m.AttacksOnEntry,
                    m.LifePower,
                    m.AttackStrength,
                    m.ArmorStrength))
                .ToListAsync());

    /// <summary>
    /// Gets the URL for retrieving room details.
    /// </summary>
    /// <param name="id">Id of the room for which we want the Url</param>
    /// <param name="url">URL helper (you get get it from the <see cref="ControllerBase.Url"/> property)</param>
    /// <param name="request">Current HTTP request (you get get it from the <see cref="ControllerBase.Request"/> property)</param>
    /// <returns>
    /// Url for the room details (e.g. https://localhost:7243/api/rooms/4)
    /// </returns>
    /// <remarks>
    /// Note that this implementation is correct. However, it is not simple to understand.
    /// In exams, it is ok if you use a simple string building mechanism with string constants.
    /// In real life however, you should use an algorithm similar to this one.
    /// </remarks>
    internal static string GetRoomDetailsUrl(int id, IUrlHelper url, HttpRequest request)
        => url.RouteUrl(
            nameof(GetRoom),
            new { id },
            request.Scheme,
            request.Host.ToString())!;
}
