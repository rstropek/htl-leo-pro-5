namespace ShareForFuture.Data.Migrations;

using Microsoft.EntityFrameworkCore.Migrations;

public partial class CreateType
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("CREATE TYPE FilterTags AS TABLE ( Filter NVARCHAR(MAX) )");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("DROP TYPE FilterTags");
    }
}
