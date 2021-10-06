using Microsoft.EntityFrameworkCore;
using ShareForFuture.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Read more about EFCore-related developer exception page at
// https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.databasedeveloperpageexceptionfilterserviceextensions.adddatabasedeveloperpageexceptionfilter
builder.Services.AddDbContext<S4fDbContext>(
    options => options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]))
    .AddDatabaseDeveloperPageExceptionFilter();

// Read more about health checks at
// https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks#entity-framework-core-dbcontext-probe
builder.Services.AddHealthChecks()
    .AddDbContextCheck<S4fDbContext>("UserGroupsAvailable", 
        customTestQuery: async (context, _) => await context.UserGroups.CountAsync() == 4);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseHealthChecks("/health");

app.Run();
