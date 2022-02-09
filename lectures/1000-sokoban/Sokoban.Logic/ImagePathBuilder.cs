namespace Sokoban.Logic;

public static class ImagePathBuilder
{
    public static string Prefix { get; set; } = string.Empty;

    public static string Build(string relativePath)
        => $"{Prefix}{relativePath}";
}
