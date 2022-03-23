using Microsoft.EntityFrameworkCore;

namespace Mathler.Solver.WebApi;

public class MathlerContext : DbContext
{
    public MathlerContext(DbContextOptions<MathlerContext> options) : base(options) { }

    public DbSet<Game> Games=> Set<Game>();
    public DbSet<Guess> Guesses => Set<Guess>();
}

public class Game
{
    public int Id { get; set; }

    public int ExpectedResult { get; set; }

    public List<Guess> Guesses { get; set; } = new();
}

public class Guess
{
    public int Id { get; set; }

    public string Formula { get; set; } = string.Empty;

    public string Result { get; set; } = string.Empty;

    public Game? Game { get; set; }

    public int GameId { get; set; }
}