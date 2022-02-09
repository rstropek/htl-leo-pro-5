namespace Sokoban.Logic;

public static class Levels
{
    private readonly static string[][] LevelStructures = new[] {
        new [] {
            "XXXXX__",
            "X   X__",
            "X@XbXXX",
            "X b ..X",
            "XXXXXXX"
        },
        new [] {
            "XXXXXXX",
            "X.   .X",
            "X  b  X",
            "X b@b X",
            "X  b  X",
            "X.   .X",
            "XXXXXXX",
        },
        new [] {
            "XXXXXXX",
            "X.   .X",
            "X.bbb.X",
            "XXb@bXX",
            "X.bbb.X",
            "X.   .X",
            "XXXXXXX",
        },
        new [] {
            "__XXXXX_",
            "XXX   X_",
            "X.@b  X_",
            "XXX b.X_",
            "X.XXb X_",
            "X X . XX",
            "Xb Bbb.X",
            "X   .  X",
            "XXXXXXXX"
        },
        new [] {
            "____XXXXX_____________",
            "____X   X_____________",
            "____Xb  X_____________",
            "__XXX  bXXX___________",
            "__X  b  b X___________",
            "XXX X XXX X     XXXXXX",
            "X   X XXX XXXXXXX  ..X",
            "X b  b             ..X",
            "XXXXX XXXX X@XXXX  ..X",
            "____X      XXX__XXXXXX",
            "____XXXXXXXX__________"
        }
    };

    public static int NumberOfLevels => LevelStructures.Length;

    public static string[] GetLevel(int levelNumber)
        => LevelStructures[levelNumber].ToArray();
}
