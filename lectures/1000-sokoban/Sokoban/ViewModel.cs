namespace Sokoban;

public class PlayerPositionToThickness : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Position position)
        {
            throw new InvalidOperationException("Source is not a player");
        }

        if (!targetType.IsAssignableFrom(typeof(Thickness)))
        {
            throw new InvalidOperationException("Destination is not a thickness");
        }

        return new Thickness(position.CellIx * 64d, position.RowIx * 64d, 0d, 0d);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public static class KeyToDirection
{
    public static PlayerDirection Convert(Key key)
        => key switch
        {
            Key.Left => PlayerDirection.Left,
            Key.Up => PlayerDirection.Up,
            Key.Down => PlayerDirection.Down,
            Key.Right => PlayerDirection.Right,
            _ => throw new InvalidOperationException("Invalid key, should never happen")
        };
}