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
        await Context.Database.ExecuteSqlRawAsync(@$"
            DELETE FROM [{nameof(S4fDbContext.UnavailabilityPeriods)}];
            DELETE FROM [{nameof(S4fDbContext.Sharings)}];
            DELETE FROM [{nameof(S4fDbContext.ComplaintNotes)}];
            DELETE FROM [{nameof(S4fDbContext.Complaints)}];
            DELETE FROM [{nameof(S4fDbContext.DeviceImages)}];
            DELETE FROM [{nameof(S4fDbContext.Offerings)}];
            DELETE FROM [OfferingsTags];
            DELETE FROM [{nameof(S4fDbContext.OfferingTags)}];
            DELETE FROM [{nameof(S4fDbContext.DeviceSubCategories)}];
            DELETE FROM [{nameof(S4fDbContext.DeviceCategories)}];
            DELETE FROM [{nameof(S4fDbContext.Identities)}];
            DELETE FROM [{nameof(S4fDbContext.Users)}];
        ");
        Context.ChangeTracker.Clear();
    }
}