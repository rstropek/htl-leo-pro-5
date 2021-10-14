DECLARE @filter NVARCHAR(MAX)
SET @filter = '%chair%'

SELECT  *
FROM    Offerings o
WHERE   o.Title LIKE @filter
        OR o.[Description] LIKE @filter
        OR EXISTS (
            SELECT  1
            FROM    OfferingTags ot
                    INNER JOIN OfferingsTags ots ON ot.Id = ots.TagsId
            WHERE   ots.OfferingsId = o.Id
                    AND ot.Tag LIKE @filter
        )

/* ====================================================================== */

/* DROP TYPE FilterTags; 
CREATE TYPE FilterTags AS TABLE ( Filter NVARCHAR(MAX) ); */
DECLARE @Filters FilterTags
INSERT INTO @Filters VALUES ('%chair%');
INSERT INTO @Filters VALUES ('%plastic%');

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
ORDER BY fh.Hits DESC

