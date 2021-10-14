namespace ShareForFuture.Data;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;

public class OfferingSearch
{
    private readonly S4fDbContext context;

    public OfferingSearch(S4fDbContext context)
    {
        this.context = context;
    }

    public IQueryable<OfferingSummary> SingleWordSearchLinq(string filter)
    {
        return context.Offerings
            .Where(o => o.Title.Contains(filter)
                || o.Description.Contains(filter)
                || o.Tags.Any(ot => ot.Tag.Contains(filter)))
            .Select(o => new OfferingSummary
            {
                Id = o.Id,
                Title = o.Title,
                Description = o.Description,
                Tags = string.Join(", ", o.Tags.Select(t => t.Tag).ToArray())
            })
            .AsNoTracking();
    }

    public IQueryable<OfferingSummary> SingleWordSearchSql(string filter)
    {
        return context.FilteredOffers.FromSqlRaw(@"
        SELECT  o.Id,
                o.Title,
                o.[Description],
                (
                        SELECT  STRING_AGG(ot.Tag, ', ')
                        FROM    OfferingTags ot
                                INNER JOIN OfferingsTags ots ON ot.Id = ots.TagsId
                        WHERE   ots.OfferingsId = o.Id
                ) AS Tags
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
    }

    public IQueryable<OfferingSummary> MultiWordSearchSql(params string[] filter)
    {
        var filterTable = new DataTable();
        filterTable.Columns.Add("Filter", typeof(string));
        foreach (var f in filter)
        {
            filterTable.Rows.Add($"%{f}%");
        }

        return context.FilteredOffers.FromSqlRaw(@"
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
                    o.[Description],
                    (
                            SELECT  STRING_AGG(ot.Tag, ', ')
                            FROM    OfferingTags ot
                                    INNER JOIN OfferingsTags ots ON ot.Id = ots.TagsId
                            WHERE   ots.OfferingsId = o.Id
                    ) AS Tags
            FROM    Offerings o
                    INNER JOIN FilteredHits fh ON o.Id = fh.Id
            ORDER BY fh.Hits DESC",
            new SqlParameter("@Filters", filterTable)
            {
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.FilterTags",
            }).AsNoTracking();
    }
}
