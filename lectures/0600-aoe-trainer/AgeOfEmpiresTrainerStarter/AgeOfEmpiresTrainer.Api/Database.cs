namespace AgeOfEmpiresTrainer.Api;

public class AoeTrainerContext: DbContext
{
    public AoeTrainerContext(DbContextOptions<AoeTrainerContext> options) : base(options) { }

    public DbSet<GameResult> GameResults => Set<GameResult>();

    // Todo: Add additional DbSets here if necessary

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
}

// Todo: Add additional model types here if necessary
