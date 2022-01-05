using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityCongestionCharge.Data.Migrations
{
    public partial class RemoveFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DrivenDuringRushHours",
                table: "Trips");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DrivenDuringRushHours",
                table: "Trips",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
