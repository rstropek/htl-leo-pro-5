namespace ShareForFuture.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

// Requirement: S4F must store a list of users
public class User
{
    public int Id { get; set; }

    // Requirement: For each user, S4F must store basic personal data like name fields and physical address.
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;

    // Requirement: For each user, S4F must store a contact email address.
    public string ContactEmail { get; set; } = string.Empty;

    // Requirement: S4F must store when the contact data was last verified.
    public DateTime? LastSuccessfullEmailVerification { get; set; }

    // Requirement: S4F wants to be able to send reminder emails to users who have not
    //  logged in to S4F for a long time. The S4F database must be able to store the
    //  necessary data to enable that feature.
    public DateTime? LastSuccessfullLogin { get; set; }

    // Requirement: Users can belong to one [...] user group.
    public UserGroup? UserGroup { get; set; }
    public int? UserGroupId { get; set; }

    // Requirement: Every user can associate her account with 1..many identities.
    public List<Identity> Identities { get; set; } = new();

    // Requirement: Every user can offer 0..many devices for sharing.
    public List<Offering> Offerings { get; set; } = new();

    public List<Sharing> Lendings { get; set; } = new();

    public List<Complaint> Complaints { get; set; } = new();
    public List<Complaint> ComplaintsAbout { get; set; } = new();
    public List<Complaint> AssignedComplaints { get; set; } = new();
}

// Read more about grouping configurations and fluent API at
// https://docs.microsoft.com/en-us/ef/core/modeling/#grouping-configuration
public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).UseIdentityColumn();
        builder.Property(u => u.FirstName).HasMaxLength(100);
        builder.Property(u => u.LastName).HasMaxLength(100);
        builder.Property(u => u.ContactEmail).HasMaxLength(150);
        builder.Property(u => u.Street).HasMaxLength(100);
        builder.Property(u => u.City).HasMaxLength(100);
        builder.Property(u => u.ZipCode).HasMaxLength(10);
        builder.Property(u => u.Country).HasMaxLength(2).IsFixedLength(true);
        builder.Property(u => u.ContactPhone).HasMaxLength(30);

        // Requirement: The email address must be globally unique (i.e. there must not be
        //  two users with the same email address in the S4F database).
        builder.HasIndex(u => u.ContactEmail).IsUnique();

        // Requirement: Users can belong to one [...] user group.
        builder.HasOne(u => u.UserGroup).WithMany(g => g.Users)
            .HasForeignKey(u => u.UserGroupId).OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
    }
}

// Requirement: Users who are S4F employees are marked in the database.
public class UserGroup
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<User> Users { get; set; } = new();
}

public class UserGroupEntityTypeConfiguration : IEntityTypeConfiguration<UserGroup>
{
    public void Configure(EntityTypeBuilder<UserGroup> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Name).HasMaxLength(100);
        builder.HasIndex(u => new { u.Name }).IsUnique();

        // Requirement: Users can belong to one of the following user groups:
        //  Regular S4F employee, Manager, System administrator
        builder.HasData(new UserGroup { Id = 1, Name = "Regular S4F employee" });
        builder.HasData(new UserGroup { Id = 2, Name = "Manager" });
        builder.HasData(new UserGroup { Id = 3, Name = "System administrator" });
    }
}

public enum IdentityProvider
{
    Google,
    Microsoft,
    Facebook,
}

// Requirement: S4F does not store user names and passwords. The system relies on external identity providers.
public class Identity
{
    public int Id { get; set; }

    // Requirement: For each identity, S4F needs to store the identity provider
    //  and the technical user ID of the corresponding identity provider.
    public IdentityProvider Provider { get; set; }
    public string SubjectId { get; set; } = string.Empty;

    public User? User { get; set; }
    public int UserId { get; set; }
}

public class IdentityEntityTypeConfiguration : IEntityTypeConfiguration<Identity>
{
    public void Configure(EntityTypeBuilder<Identity> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).UseIdentityColumn();
        builder.Property(u => u.SubjectId).HasMaxLength(100);
        builder.Property(u => u.Provider).HasConversion<string>();
        builder.HasIndex(u => new { u.Provider, u.SubjectId }).IsUnique();

        // Requirement: Every user can associate her account with 1..many identities.
        builder.HasOne(u => u.User).WithMany(u => u.Identities)
            .HasForeignKey(u => u.UserId).OnDelete(DeleteBehavior.Cascade)
            .IsRequired(true);
    }
}
