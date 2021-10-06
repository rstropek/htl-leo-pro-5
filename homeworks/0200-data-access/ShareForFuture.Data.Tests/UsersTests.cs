namespace ShareForFuture.Data.Tests;

using Microsoft.EntityFrameworkCore;

public class UsersTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture fixture;

    public UsersTests(DatabaseFixture fixture)
    {
        this.fixture = fixture;
        fixture.CleanDatabase().Wait();
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task CreateUser()
    {
        var newUser = DataGenerator.CreateUser();

        // Insert user
        fixture.Context.Users.Add(newUser);
        await fixture.Context.SaveChangesAsync();
        fixture.Context.ChangeTracker.Clear();

        // Check if ID was set
        Assert.True(newUser.Id > 0);

        // Make sure that user is in DB
        var users = await fixture.Context.Users.Where(u => u.Id == newUser.Id).ToListAsync();
        Assert.NotEmpty(users);
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task DeleteIdentitiesWhenDeletingUser()
    {
        var newUser = DataGenerator.CreateUser();

        // Add user with identities
        fixture.Context.Users.Add(newUser);
        await fixture.Context.SaveChangesAsync();

        // Remove user
        fixture.Context.Users.Remove(newUser);
        await fixture.Context.SaveChangesAsync();

        // Make sure that identities were deleted
        Assert.Empty(await fixture.Context.Identities.ToArrayAsync());
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task DuplicateEmail()
    {
        var newUser1 = DataGenerator.CreateUser();

        // Add user
        fixture.Context.Users.Add(newUser1);
        await fixture.Context.SaveChangesAsync();

        // Add user with identical email
        var newUser2 = DataGenerator.CreateUser();
        fixture.Context.Users.Add(newUser2);

        // Make sure that user with identical email has not been inserted
        await Assert.ThrowsAsync<DbUpdateException>(async () => await fixture.Context.SaveChangesAsync());
        Assert.Equal(1, await fixture.Context.Users.CountAsync());
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task DeleteGroup()
    {
        var newUser = DataGenerator.CreateUser(withGroup: true);

        Assert.Equal(3, await fixture.Context.UserGroups.CountAsync());

        // Add user
        fixture.Context.Users.Add(newUser);
        await fixture.Context.SaveChangesAsync();
        fixture.Context.ChangeTracker.Clear();

        // Try to remove group that has user -> must not work
        var group = await fixture.Context.UserGroups.FirstAsync(g => g.Id == newUser.UserGroupId);
        fixture.Context.UserGroups.Remove(group);
        await Assert.ThrowsAsync<DbUpdateException>(async () => await fixture.Context.SaveChangesAsync());

        // Ensure that group is still in DB
        Assert.Equal(3, await fixture.Context.UserGroups.CountAsync());
    }
}
