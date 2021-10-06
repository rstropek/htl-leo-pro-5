namespace ShareForFuture.Data.Tests;

using Microsoft.EntityFrameworkCore;

public class OfferingsTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture fixture;

    public OfferingsTests(DatabaseFixture fixture)
    {
        this.fixture = fixture;
        fixture.CleanDatabase().Wait();
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task CreateOffering()
    {
        var newOffering = DataGenerator.CreateOffering();

        // Insert offering
        fixture.Context.Offerings.Add(newOffering);
        await fixture.Context.SaveChangesAsync();
        fixture.Context.ChangeTracker.Clear();

        // Check if ID was set
        Assert.True(newOffering.Id > 0);

        // Make sure that offering is in DB
        var offerings = await fixture.Context.Offerings.Where(u => u.Id == newOffering.Id).ToListAsync();
        Assert.NotEmpty(offerings);

        // Cleanup offerings
        fixture.Context.Offerings.Remove(offerings[0]);
        await fixture.Context.SaveChangesAsync();

        // Make sure that category is still in DB
        var cat = await fixture.Context.DeviceSubCategories.Include(s => s.Category).ToListAsync();
        Assert.NotEmpty(cat);

        // Make sure that tags are still in DB
        var tags = await fixture.Context.OfferingTags.ToListAsync();
        Assert.NotEmpty(tags);

        // Make sure that image data items have been deleted
        Assert.Empty(await fixture.Context.DeviceImages.ToArrayAsync());
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task UnavailabilityPeriods()
    {
        var newOffering = DataGenerator.CreateOffering();

        // Insert offering
        fixture.Context.Offerings.Add(newOffering);
        await fixture.Context.SaveChangesAsync();

        // Add valid period
        newOffering.UnavailabilityPeriods.Add(new()
        {
            From = DateTime.Now.AddDays(-5),
            Until = DateTime.Now.AddDays(-1),
        });
        await fixture.Context.SaveChangesAsync();

        // Remove offering
        fixture.Context.Offerings.Remove(newOffering);
        await fixture.Context.SaveChangesAsync();

        // Make sure that unavailability periods have been deleted
        Assert.Empty(await fixture.Context.UnavailabilityPeriods.ToArrayAsync());
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task InvalidUnavailabilityPeriod()
    {
        var newOffering = DataGenerator.CreateOffering();

        // Insert offering
        fixture.Context.Offerings.Add(newOffering);
        await fixture.Context.SaveChangesAsync();

        // Add invalid period
        newOffering.UnavailabilityPeriods.Add(new()
        {
            From = DateTime.Now.AddDays(-1),
            Until = DateTime.Now.AddDays(-5),
        });

        // Make sure that we cannot insert an invalid period
        await Assert.ThrowsAsync<DbUpdateException>(async () => await fixture.Context.SaveChangesAsync());
    }
}
