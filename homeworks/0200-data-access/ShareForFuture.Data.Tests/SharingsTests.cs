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
}
