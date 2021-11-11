using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgeOfEmpiresTrainer.Api.Migrations
{
    public partial class Model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumberOfAIPlayers = table.Column<byte>(type: "tinyint", nullable: false),
                    GameStatus = table.Column<int>(type: "int", nullable: false),
                    CivilizationLevel = table.Column<byte>(type: "tinyint", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameResults", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AIPlayers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Civilization = table.Column<int>(type: "int", nullable: false),
                    DifficultyLevel = table.Column<int>(type: "int", nullable: false),
                    Defeated = table.Column<bool>(type: "bit", nullable: false),
                    GameResultId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AIPlayers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AIPlayers_GameResults_GameResultId",
                        column: x => x.GameResultId,
                        principalTable: "GameResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AIPlayers_GameResultId",
                table: "AIPlayers",
                column: "GameResultId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AIPlayers");

            migrationBuilder.DropTable(
                name: "GameResults");
        }
    }
}
