namespace ShareForFuture.Data.Tests;

using Microsoft.EntityFrameworkCore;

public class SharingsTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture fixture;

    public SharingsTests(DatabaseFixture fixture)
    {
        this.fixture = fixture;
        fixture.CleanDatabase().Wait();
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task CreateSharing()
    {
        var newSharing = DataGenerator.CreateSharing();

        // Insert sharing
        fixture.Context.Sharings.Add(newSharing);
        await fixture.Context.SaveChangesAsync();
        fixture.Context.ChangeTracker.Clear();

        // Check if ID was set
        Assert.True(newSharing.Id > 0);

        // Make sure that sharing is in DB
        var sharings = await fixture.Context.Sharings.Where(u => u.Id == newSharing.Id).ToListAsync();
        Assert.NotEmpty(sharings);

        // Cleanup sharings
        fixture.Context.Sharings.Remove(sharings[0]);
        await fixture.Context.SaveChangesAsync();

        // Make sure that offering is still in DB
        var off = await fixture.Context.Offerings.ToListAsync();
        Assert.NotEmpty(off);
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task UntilBeforeFrom()
    {
        var newSharing = DataGenerator.CreateSharing();
        newSharing.Until = newSharing.From.AddDays(-1);

        fixture.Context.Sharings.Add(newSharing);
        await Assert.ThrowsAsync<DbUpdateException>(async () => await fixture.Context.SaveChangesAsync());
    }
}
