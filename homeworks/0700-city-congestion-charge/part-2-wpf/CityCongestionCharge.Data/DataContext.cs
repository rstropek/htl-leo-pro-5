using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CityCongestionCharge.Data;

public partial class CccDataContext : DbContext
{
    public CccDataContext(DbContextOptions<CccDataContext> options) : base(options) { }

    public DbSet<Owner> Owners => Set<Owner>();
    public DbSet<Car> Cars => Set<Car>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Detection> Detections => Set<Detection>();
}

/// <summary>
/// Data Context factory
/// </summary>
/// <remarks>
/// We need the data context factory to be able to generate migrations in
/// the class library instead of the ASP.NET Core executable.
/// </remarks>
public class CccDataContextFactory : IDesignTimeDbContextFactory<CccDataContext>
{
    public CccDataContext CreateDbContext(string[]? args = null)
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        var optionsBuilder = new DbContextOptionsBuilder<CccDataContext>();
        optionsBuilder.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);

        return new CccDataContext(optionsBuilder.Options);
    }
}
