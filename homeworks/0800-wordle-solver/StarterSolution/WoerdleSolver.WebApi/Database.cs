using Microsoft.EntityFrameworkCore;

namespace WoerdleSolver.WebApi;

public class SolverContext : DbContext
{
    public SolverContext(DbContextOptions<SolverContext> options) : base(options) { }

    public DbSet<Game> Games => Set<Game>();
    public DbSet<Guess> Guesses => Set<Guess>();
}

public class Game
{
    public int Id { get; set; }

    public List<Guess> Guesses { get; set; } = new();
}

public class Guess
{
    public int Id { get; set; }

    public string Word { get; set; } = string.Empty;

    public string Result { get; set; } = string.Empty;

    public Game? Game { get; set; }

    public int GameId { get; set; }
}