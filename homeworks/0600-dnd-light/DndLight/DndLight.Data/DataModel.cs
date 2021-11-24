namespace DndLight;

public class GameSetup
{
    public int Id { get; set; }

    public string Description { get; set; } = string.Empty;

    public Room? StartingRoom { get; set; }

    public int StartingRoomId { get; set; }

    public byte InitialLifePower { get; set; }

    public byte InitialAttackStrength { get; set; }

    public byte InitialArmorStrength { get; set; }
}

public class Room
{
    public int Id { get; set; }

    public string Description { get; set; } = string.Empty;
}

public enum ItemType
{
    IronKey,
    BronzeKey,
    SilverKey,
    GoldKey,
    DiamondKey,
    Armor,
    Sword,
    HealingPotion,
    MagicSpellbook,
}

/// <summary>
/// Door between two rooms
/// </summary>
/// <remarks>
/// A door always connects two doors. Doors can always be passed in both directions.
/// </remarks>
public class Door
{
    public int Id { get; set; }

    public string Description { get; set; } = string.Empty;

    public Room? LinkedRoom1 { get; set; }

    public int LinkedRoom1Id { get; set; }

    public Room? LinkedRoom2 { get; set; }

    public int LinkedRoom2Id { get; set; }

    public bool InitiallyLocked { get; set; }

    public ItemType? RequiredItemToUnlock { get; set; }
}

public class Item
{
    public int Id { get; set; }

    public string Description { get; set; } = string.Empty;

    public ItemType ItemType { get; set; }

    public Room? Room { get; set; }

    public int RoomId { get; set; }
}

public class Monster
{
    public int Id { get; set; }

    public string Description { get; set; } = string.Empty;

    public Room? Room { get; set; }

    public int RoomId { get; set; }

    public bool AttacksOnEntry { get; set; }

    public byte LifePower { get; set; }

    public byte AttackStrength { get; set; }

    public byte ArmorStrength { get; set; }
}
