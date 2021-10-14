namespace ShareForFuture.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

// Requirement: Users can file complaints about other users (e.g. borrower did return the borrowed device broken).
public class Complaint
{
    public int Id { get; set; }

    public User? Complainer { get; set; }
    public int ComplainerId { get; set; }

    public User? Complainee { get; set; }
    public int ComplaineeId { get; set; }

    // Requirement: S4F employees will try to settle the complaints.
    public User? AssignedTo { get; set; }
    public int AssignedToId { get; set; }

    // Requirement: The S4F employee can mark the complaint as done once it will have been settled.
    public DateTime? DoneTimestamp { get; set; }

    // Requirement: S4F wants to be able to generate statics about the duration of complaints.
    public DateTime CreatedTimestamp { get; set; }

    // Requirement: Both involved users and the assigned S4F employee can store notes (text, pictures) to the complaint.
    public List<ComplaintNote> Notes { get; set; } = new();
}

public class ComplaintEntityTypeConfiguration : IEntityTypeConfiguration<Complaint>
{
    public void Configure(EntityTypeBuilder<Complaint> builder)
    {
        builder.HasKey(o => o.Id);
        builder.HasOne(c => c.Complainer).WithMany(u => u.Complaints)
            .HasForeignKey(c => c.ComplainerId).OnDelete(DeleteBehavior.Restrict)
            .IsRequired(true);
        builder.HasOne(c => c.Complainee).WithMany(u => u.ComplaintsAbout)
            .HasForeignKey(c => c.ComplaineeId).OnDelete(DeleteBehavior.Restrict)
            .IsRequired(true);

        // Requirement: When a complaint comes in, a S4F manager assigns it to an S4F
        //  employee who will be responsible for the complaint.
        builder.HasOne(c => c.AssignedTo).WithMany(u => u.AssignedComplaints)
            .HasForeignKey(c => c.AssignedToId).OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        // Requirement: S4F wants to be able to generate statics about the duration of complaints.
        builder.Property(c => c.CreatedTimestamp).HasDefaultValueSql("GETDATE()");
    }
}

// Requirement: Both involved users and the assigned S4F employee can store notes (text, pictures) to the complaint.
public class ComplaintNote
{
    public int Id { get; set; }

    public string? TextNote { get; set; }
    public byte[]? Picture { get; set; }

    public Complaint? Complait { get; set; }
    public int ComplaintId { get; set; }
}

public class ComplaintNoteEntityTypeConfiguration : IEntityTypeConfiguration<ComplaintNote>
{
    public void Configure(EntityTypeBuilder<ComplaintNote> builder)
    {
        builder.HasKey(o => o.Id);

        // Note: TextNote does not have a max. length; this is by design.
        // Note: Picture does not have a max. length; this is by design.

        builder.HasOne(c => c.Complait).WithMany(u => u.Notes)
            .HasForeignKey(c => c.ComplaintId).OnDelete(DeleteBehavior.Cascade)
            .IsRequired(true);
    }
}