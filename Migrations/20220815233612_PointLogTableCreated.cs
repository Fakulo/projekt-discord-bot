using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordBot.Migrations
{
    public partial class PointLogTableCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastUpdate",
                table: "Pokemons",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "LastUpdate",
                table: "Points",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "StatsUpdate",
                table: "Players",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "Points",
                table: "Players",
                newName: "TotalPoints");

            migrationBuilder.RenameColumn(
                name: "LastUpdate",
                table: "Players",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "LastUpdate",
                table: "GymLocationCells",
                newName: "UpdatedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "PlayersStats",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(DateTime.Now.Ticks, DateTimeKind.Local));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "PlayerPokemons",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "PointLogs",
                columns: table => new
                {
                    PointLogId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Amount = table.Column<int>(type: "INTEGER", nullable: false),
                    Category = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    AssignedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointLogs", x => x.PointLogId);
                    table.ForeignKey(
                        name: "FK_PointLogs_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PointLogs_PlayerId",
                table: "PointLogs",
                column: "PlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PointLogs");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "PlayersStats");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "PlayerPokemons");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Pokemons",
                newName: "LastUpdate");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Points",
                newName: "LastUpdate");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Players",
                newName: "StatsUpdate");

            migrationBuilder.RenameColumn(
                name: "TotalPoints",
                table: "Players",
                newName: "Points");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Players",
                newName: "LastUpdate");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "GymLocationCells",
                newName: "LastUpdate");
        }
    }
}
