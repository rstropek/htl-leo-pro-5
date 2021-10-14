using Microsoft.EntityFrameworkCore;

namespace ShareForFuture.Data.Tests;

public class DatabaseFixture : IDisposable
{
    public DatabaseFixture()
    {
        var factory = new S4fDbContextFactory();
        Context = factory.CreateDbContext();

        Context.Database.EnsureDeleted();
        Context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Context.Dispose();
    }

    public S4fDbContext Context { get; }

    public async Task CleanDatabase()
    {
        var generator = new DemoDataGenerator(Context);
        await generator.ClearAll();
    }
}