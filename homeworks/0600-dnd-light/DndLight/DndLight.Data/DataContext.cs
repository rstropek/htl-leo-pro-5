namespace DndLight;

public class DndContext : DbContext
{
    public DndContext(DbContextOptions<DndContext> options) : base(options) { }

    public DbSet<GameSetup> Games => Set<GameSetup>();

    public DbSet<Room> Rooms => Set<Room>();

    public DbSet<Door> Doors => Set<Door>();

    public DbSet<Item> Items => Set<Item>();

    public DbSet<Monster> Monsters => Set<Monster>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Door>()
            .HasOne(r => r.LinkedRoom1)
            .WithMany()
            .HasForeignKey(d => d.LinkedRoom1Id)
            .OnDelete(DeleteBehavior.ClientSetNull);
        builder.Entity<Door>()
            .HasOne(r => r.LinkedRoom2)
            .WithMany()
            .HasForeignKey(d => d.LinkedRoom2Id)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }
}

public class DndContextFactory : IDesignTimeDbContextFactory<DndContext>
{
    public DndContext CreateDbContext(string[]? args = null)
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        var optionsBuilder = new DbContextOptionsBuilder<DndContext>();
        optionsBuilder.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);

        return new DndContext(optionsBuilder.Options);
    }
}