namespace ShareForFuture.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Reflection;

public class S4fDbContext : DbContext
{
    public S4fDbContext(DbContextOptions<S4fDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<UserGroup> UserGroups => Set<UserGroup>();
    public DbSet<Identity> Identities => Set<Identity>();
    public DbSet<Offering> Offerings => Set<Offering>();
    public DbSet<DeviceImage> DeviceImages => Set<DeviceImage>();
    public DbSet<DeviceCategory> DeviceCategories => Set<DeviceCategory>();
    public DbSet<DeviceSubCategory> DeviceSubCategories => Set<DeviceSubCategory>();
    public DbSet<OfferingTag> OfferingTags => Set<OfferingTag>();
    public DbSet<UnavailabilityPeriod> UnavailabilityPeriods => Set<UnavailabilityPeriod>();
    public DbSet<Sharing> Sharings => Set<Sharing>();
    public DbSet<Complaint> Complaints => Set<Complaint>();
    public DbSet<ComplaintNote> ComplaintNotes => Set<ComplaintNote>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}

public class S4fDbContextFactory : IDesignTimeDbContextFactory<S4fDbContext>
{
    public S4fDbContext CreateDbContext(string[]? args = null)
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        var optionsBuilder = new DbContextOptionsBuilder<S4fDbContext>();
        optionsBuilder.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);

        return new S4fDbContext(optionsBuilder.Options);
    }
}
