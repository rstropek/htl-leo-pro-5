namespace ShareForFuture.Data.Tests;

using Microsoft.EntityFrameworkCore;

public class ComplaintTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture fixture;

    public ComplaintTests(DatabaseFixture fixture)
    {
        this.fixture = fixture;
        fixture.CleanDatabase().Wait();
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task CreateComplaint()
    {
        var newComplaint = DataGenerator.CreateComplaint();

        // Insert complaint
        fixture.Context.Complaints.Add(newComplaint);
        await fixture.Context.SaveChangesAsync();
        fixture.Context.ChangeTracker.Clear();

        // Check if ID was set
        Assert.True(newComplaint.Id > 0);

        // Make sure that complaint is in DB
        var complaints = await fixture.Context.Complaints.Where(u => u.Id == newComplaint.Id).ToListAsync();
        Assert.NotEmpty(complaints);
    }

    [Fact]
    [Trait("Category", "IntegrationTest")]
    public async Task CreateComplaintNote()
    {
        var newComplaintNote = new ComplaintNote
        {
            Complait = DataGenerator.CreateComplaint(),
            TextNote = "FooBar",
        };

        // Insert complaint note
        fixture.Context.ComplaintNotes.Add(newComplaintNote);
        await fixture.Context.SaveChangesAsync();

        // Remove complaint
        fixture.Context.Complaints.Remove(newComplaintNote.Complait);
        await fixture.Context.SaveChangesAsync();

        // Make sure that complaint note was removed, too
        Assert.Empty(await fixture.Context.ComplaintNotes.ToArrayAsync());
    }
}
