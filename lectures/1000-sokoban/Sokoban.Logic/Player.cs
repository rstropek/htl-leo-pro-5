namespace Sokoban.Logic;

public enum PlayerDirection
{
    Left,
    Up,
    Right,
    Down,
}

public readonly record struct Position(int RowIx, int CellIx);

public class Player : INotifyPropertyChanged
{
    private Position PositionValue;
    public Position Position
    {
        get => PositionValue;
        set
        {
            if (value != PositionValue)
            {
                PositionValue = value;
                PropertyChanged?.Invoke(this, new(nameof(Position)));
            }
        }
    }

    public string ImagePath => Direction switch
    {
        PlayerDirection.Left => ImagePathBuilder.Build("Player/player_14.png"),
        PlayerDirection.Up => ImagePathBuilder.Build("Player/player_02.png"),
        PlayerDirection.Right => ImagePathBuilder.Build("Player/player_17.png"),
        PlayerDirection.Down => ImagePathBuilder.Build("Player/player_05.png"),
        _ => throw new NotImplementedException("Unknown direction")
    };

    private PlayerDirection DirectionValue = PlayerDirection.Down;

    public event PropertyChangedEventHandler? PropertyChanged;

    public PlayerDirection Direction
    {
        get => DirectionValue;
        set
        {
            if (value != DirectionValue)
            {
                DirectionValue = value;
                PropertyChanged?.Invoke(this, new(nameof(Direction)));
                PropertyChanged?.Invoke(this, new(nameof(ImagePath)));
            }
        }
    }
}
