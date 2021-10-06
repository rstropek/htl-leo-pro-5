namespace ShareForFuture.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class Offering
{
    public int Id { get; set; }

    public User? User { get; set; }
    public int UserId { get; set; }

    // Requirement: For each device, S4F must store basic description data including title,
    //  description, condition (like new, used, heavily used, to be repaired).
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DeviceCondition Condition { get; set; } = DeviceCondition.Used;

    // Requirement:  S4F wants to be able to send out regular (e.g. every month) reminder
    //  emails to people offering devices asking whether the data about the device is still valid.
    public DateTime? LastSuccessfullAvailabilityVerification { get; set; }

    // Requirement: S4F must store [...] 0..many images of the device.
    public List<DeviceImage> Images { get; set; } = new();

    // Requirement: Each device must be assigned to a subcategory.
    public DeviceSubCategory? SubCategory { get; set; }
    public int SubCategoryId { get; set; }

    // Requirement: Users offering devices can assign 0..many tags to each device.
    public List<OfferingTag> Tags { get; set; } = new();

    // Requirement: Users can mark devices that they offer as currently available or currently unavailable.
    public DateTime? UnavailableSince { get; set; }
    public bool IsAvailable => UnavailableSince is null;
    public bool IsUnavailable => UnavailableSince is DateTime;

    // Requirement: Users can store periods of time through which a device will not be available.
    public List<UnavailabilityPeriod> UnavailabilityPeriods { get; set; } = new();

    public List<Sharing> Sharings { get; set; } = new();
}

public enum DeviceCondition
{
    LikeNew,
    Used,
    HeavilyUsed,
    ToBeRepaired,
}

public class OfferingEntityTypeConfiguration : IEntityTypeConfiguration<Offering>
{
    public void Configure(EntityTypeBuilder<Offering> builder)
    {
        builder.HasKey(o => o.Id);

        // Requirement: Every user can offer 0..many devices for sharing.
        builder.HasOne(o => o.User).WithMany(u => u.Offerings)
            .HasForeignKey(o => o.UserId).OnDelete(DeleteBehavior.Restrict)
            .IsRequired(true);

        builder.Property(o => o.Condition).HasConversion<string>();
        builder.Property(o => o.Title).HasMaxLength(100);
        // Description has no max length; this is by design.

        // Requirement: Each device must be assigned to a subcategory.
        builder.HasOne(o => o.SubCategory).WithMany(c => c.Offerings)
            .HasForeignKey(o => o.SubCategoryId).OnDelete(DeleteBehavior.Restrict)
            .IsRequired(true);

        // Requirement: Users offering devices can assign 0..many tags to each device.
        builder.HasMany(o => o.Tags).WithMany(t => t.Offerings).UsingEntity(
            "OfferingsTags",
            j => j.HasOne(typeof(OfferingTag)).WithMany().OnDelete(DeleteBehavior.Restrict),
            j => j.HasOne(typeof(Offering)).WithMany().OnDelete(DeleteBehavior.Cascade));

        // Requirement: Users can store periods of time through which a device will not be available.
        builder.HasMany(o => o.UnavailabilityPeriods).WithOne()
            .OnDelete(DeleteBehavior.Cascade).IsRequired();
    }
}

// Requirement: S4F must store [...] 0..many images of the device.
public class DeviceImage
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public byte[] ImageData { get; set; } = Array.Empty<byte>();

    public Offering? Offering { get; set; }
    public int? OfferingId { get; set; }
}

public class DeviceImageEntityTypeConfiguration : IEntityTypeConfiguration<DeviceImage>
{
    public void Configure(EntityTypeBuilder<DeviceImage> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Title).HasMaxLength(100);
        // ImageData has no max length; this is by design.

        builder.HasOne(i => i.Offering).WithMany(o => o.Images)
            .HasForeignKey(i => i.OfferingId).OnDelete(DeleteBehavior.Cascade)
            .IsRequired(true);
    }
}

// Requirement: S4F contains a list of device categories.
public class DeviceCategory
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;

    // Requirement: Each category consists of a list of subcategories.
    public List<DeviceSubCategory> SubCategories { get; set; } = new();
}

public class DeviceCategoryImageEntityTypeConfiguration : IEntityTypeConfiguration<DeviceCategory>
{
    public void Configure(EntityTypeBuilder<DeviceCategory> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Title).HasMaxLength(100);
    }
}

// Requirement: Each category consists of a list of subcategories.
public class DeviceSubCategory
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;

    public DeviceCategory? Category { get; set; }
    public int CategoryId { get; set; }

    // Requirement: Each device must be assigned to a subcategory.
    public List<Offering> Offerings { get; set; } = new();
}

public class DeviceSubCategoryImageEntityTypeConfiguration : IEntityTypeConfiguration<DeviceSubCategory>
{
    public void Configure(EntityTypeBuilder<DeviceSubCategory> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Title).HasMaxLength(100);

        // Requirement: Each category consists of a list of subcategories. 
        builder.HasOne(d => d.Category).WithMany(d => d.SubCategories)
            .HasForeignKey(d => d.CategoryId).OnDelete(DeleteBehavior.Restrict)
            .IsRequired(true);
    }
}

// Requirement: A tag is just a text property.
public class OfferingTag
{
    public int Id { get; set; }
    public string Tag { get; set; } = string.Empty;

    public List<Offering> Offerings { get; set; } = new();
}

public class OfferingTagImageEntityTypeConfiguration : IEntityTypeConfiguration<OfferingTag>
{
    public void Configure(EntityTypeBuilder<OfferingTag> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Tag).HasMaxLength(100);
        builder.HasIndex(d => d.Tag).IsUnique();
    }
}

// Requirement: Users can store periods of time through which a device will not be available.
public class UnavailabilityPeriod
{
    public int Id { get; set; }
    public DateTime From { get; set; }
    public DateTime? Until { get; set; }
}

public class UnavailabilityPeriodEntityTypeConfiguration : IEntityTypeConfiguration<UnavailabilityPeriod>
{
    public void Configure(EntityTypeBuilder<UnavailabilityPeriod> builder)
    {
        builder.HasKey(p => p.Id);
        builder.HasCheckConstraint("UntilAfterFrom", 
            @$"[{nameof(UnavailabilityPeriod.Until)}] IS NULL
                OR [{nameof(UnavailabilityPeriod.Until)}] > [{nameof(UnavailabilityPeriod.From)}]");
    }
}