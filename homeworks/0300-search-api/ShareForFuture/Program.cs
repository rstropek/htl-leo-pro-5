using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ShareForFuture.Data;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Read more about EFCore-related developer exception page at
// https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.databasedeveloperpageexceptionfilterserviceextensions.adddatabasedeveloperpageexceptionfilter
builder.Services.AddDbContext<S4fDbContext>(
    options => options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]))
    .AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddScoped<OfferingSearch>();

// Read more about health checks at
// https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks#entity-framework-core-dbcontext-probe
builder.Services.AddHealthChecks()
    .AddDbContextCheck<S4fDbContext>("UserGroupsAvailable", 
        customTestQuery: async (context, _) => await context.UserGroups.CountAsync() == 4);

builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(b => b.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseHealthChecks("/health");

// Fill with demo data
app.MapPost("/fill", async (S4fDbContext context) =>
{
    var generator = new DemoDataGenerator(context);
    await generator.ClearAll();
    await generator.Generate();
    return Results.Ok();
});

app.MapGet("/single-word-linq", (OfferingSearch searcher, string filter)
    => Results.Ok(searcher.SingleWordSearchLinq(filter)));

app.MapGet("/single-word-sql", (OfferingSearch searcher, string filter)
    => Results.Ok(searcher.SingleWordSearchSql(filter)));

app.MapGet("/multi-word", (OfferingSearch searcher, string filter)
    => Results.Ok(searcher.MultiWordSearchSql(filter.Split(','))));

app.Run();
