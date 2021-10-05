using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ShareForFuture;

public class S4fDbContext : DbContext
{
    public S4fDbContext(DbContextOptions<S4fDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<UserGroup> UserGroups => Set<UserGroup>();
    public DbSet<Identity> Identities => Set<Identity>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
