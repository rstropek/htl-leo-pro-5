namespace ShareForFuture.Data.Tests;

using Microsoft.EntityFrameworkCore;

public class OfferingSearchTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture fixture;

    public OfferingSearchTests(DatabaseFixture fixture)
    {
        this.fixture = fixture;
        fixture.CleanDatabase().Wait();
    }

    [Theory]
    [Trait("Category", "IntegrationTest")]
    [InlineData("drill", 2)]
    [InlineData("cinema", 1)]
    public async Task SingleWordSearchLinqDrill(string filter, int numberOfResults)
    {
        var newOfferings = DataGenerator.CreateSearchOffering();

        // Insert offerings
        fixture.Context.Offerings.AddRange(newOfferings);
        await fixture.Context.SaveChangesAsync();

        // Search and check number of results
        var searcher = new OfferingSearch(fixture.Context);
        var drillResult = await searcher.SingleWordSearchLinq(filter).ToArrayAsync();
        Assert.Equal(numberOfResults, drillResult.Length);
    }

    [Theory]
    [Trait("Category", "IntegrationTest")]
    [InlineData("drill", 2)]
    [InlineData("cinema", 1)]
    public async Task SingleWordSearchSqlDrill(string filter, int numberOfResults)
    {
        var newOfferings = DataGenerator.CreateSearchOffering();

        // Insert offerings
        fixture.Context.Offerings.AddRange(newOfferings);
        await fixture.Context.SaveChangesAsync();

        // Search and check number of results
        var searcher = new OfferingSearch(fixture.Context);
        var drillResult = await searcher.SingleWordSearchSql(filter).ToArrayAsync();
        Assert.Equal(numberOfResults, drillResult.Length);
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task MultiWordSearch()
    {
        var newOfferings = DataGenerator.CreateSearchOffering();

        // Insert offerings
        fixture.Context.Offerings.AddRange(newOfferings);
        await fixture.Context.SaveChangesAsync();

        // Search and check number of results
        var searcher = new OfferingSearch(fixture.Context);
        var result = await searcher.MultiWordSearchSql("drill", "tool").ToArrayAsync();
        Assert.Equal(2, result.Length);
        Assert.Equal("Drilling machine", result[0].Title);
        Assert.Equal("Bosch driller", result[1].Title);
    }
}
