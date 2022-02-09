using Sokoban.Logic;

namespace Sokoban.Web;

public static class KeyToDirection
{
    public static PlayerDirection Convert(string key)
        => key switch
        {
            "ArrowLeft" => PlayerDirection.Left,
            "ArrowUp" => PlayerDirection.Up,
            "ArrowRight" => PlayerDirection.Right,
            "ArrowDown" => PlayerDirection.Down,
            _ => throw new InvalidOperationException("Invalid key, should never happen")
        };
}