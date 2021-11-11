namespace AgeOfEmpiresTrainer.Api;

public class AoeTrainerContext: DbContext
{
    public AoeTrainerContext(DbContextOptions<AoeTrainerContext> options) : base(options) { }

    public DbSet<GameResult> GameResults => Set<GameResult>();

    // Todo: Add additional DbSets here if necessary
    #region Solution
    public DbSet<AIPlayer> AIPlayers => Set<AIPlayer>();
    #endregion

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Todo: Add model configuration here if necessary
    }
}

public enum Civilization
{
    Rus,
    HolyRomanEmpire,
    Chinese,
    English,
    DelhiSultanate,
    Mongols,
    AbbasidDynasty,
    French,
}

public enum GameStatus
{
    Lose,
    Win,
}

public enum DifficultyLevel
{
    Easy,
    Intermediate,
    Hard,
}

public class GameResult
{
    public int Id { get; set; }

    // Todo: Complete this class
    #region Solution
    public byte NumberOfAIPlayers { get; set; }

    public GameStatus GameStatus { get; set; }

    public byte CivilizationLevel { get; set; }

    public string? Notes { get; set; }

    public List<AIPlayer> AIPlayers { get; set; } = new();
    #endregion
}

// Todo: Add additional model types here if necessary
#region Solution
public class AIPlayer
{
    public int Id { get; set; }

    public Civilization Civilization { get; set; }

    public DifficultyLevel DifficultyLevel { get; set; }

    public bool Defeated { get; set; }

    public GameResult? GameResult { get; set; }

    public int GameResultId { get; set; }
}
#endregion