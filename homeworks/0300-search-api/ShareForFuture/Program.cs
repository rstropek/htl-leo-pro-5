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

// Fill with demo data
app.MapPost("/fill", async (S4fDbContext context) =>
{
    var generator = new DemoDataGenerator(context);
    await generator.ClearAll();
    await generator.Generate();
    return Results.Ok();
});

app.MapGet("/single-word", (S4fDbContext context, string filter) =>
{
    var offerings = context.FilteredOffers.FromSqlRaw(@"
        SELECT  o.Id,
                o.Title,
                o.[Description]
        FROM    Offerings o
        WHERE   o.Title LIKE @filter
                OR o.[Description] LIKE @filter
                OR EXISTS (
                    SELECT  1
                    FROM    OfferingTags ot
                            INNER JOIN OfferingsTags ots ON ot.Id = ots.TagsId
                    WHERE   ots.OfferingsId = o.Id
                            AND ot.Tag LIKE @filter
                )",
        new SqlParameter("@filter", SqlDbType.NVarChar)
        {
            Value = $"%{filter}%",
        }).AsNoTracking();
    return Results.Ok(offerings);
});


app.MapGet("/multi-word", (S4fDbContext context, string filter) =>
{
    var filterTable = new DataTable();
    filterTable.Columns.Add("Filter", typeof(string));
    foreach (var f in filter.Split(','))
    {
        filterTable.Rows.Add($"%{f}%");
    }

    var offerings = context.FilteredOffers.FromSqlRaw(@"
        WITH Hits AS (
            SELECT  o.Id,
                    CASE WHEN 
                        o.Title LIKE f.Filter
                        OR o.[Description] LIKE f.Filter
                        OR EXISTS (
                            SELECT  1
                            FROM    OfferingTags ot
                                    INNER JOIN OfferingsTags ots ON ot.Id = ots.TagsId
                            WHERE   ots.OfferingsId = o.Id
                                    AND ot.Tag LIKE f.Filter
                        )
                        THEN 1 
                        ELSE 0 
                    END AS ContainsFilter
            FROM    Offerings o
                    CROSS JOIN @Filters f
        ),
        FilteredHits AS (
            SELECT  h.Id,
                    SUM(h.ContainsFilter) AS Hits
            FROM    Hits h
            GROUP BY h.Id
            HAVING  SUM(h.ContainsFilter) > 0
        )
        SELECT  o.Id,
                o.Title,
                o.[Description]
        FROM    Offerings o
                INNER JOIN FilteredHits fh ON o.Id = fh.Id
        ORDER BY fh.Hits DESC",
        new SqlParameter("@Filters", filterTable)
        {
            SqlDbType = SqlDbType.Structured,
            TypeName = "dbo.FilterTags",
        }).AsNoTracking();
    return Results.Ok(offerings);
});

app.Run();
