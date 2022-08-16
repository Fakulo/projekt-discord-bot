using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordBot.Migrations
{
    public partial class AutomatedUpdateAtEdited5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Pokemons",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "strftime('%Y-%m-%d %H:%M:%S', datetime('now'))");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Pokemons",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "PointLogs",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "strftime('%Y-%m-%d %H:%M:%S', datetime('now'))");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "PointLogs",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "PlayersStats",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "strftime('%Y-%m-%d %H:%M:%S', datetime('now'))");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "PlayersStats",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Players",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "strftime('%Y-%m-%d %H:%M:%S', datetime('now'))");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Players",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "strftime('%Y-%m-%d %H:%M:%S', datetime('now'))");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "PlayerPokemons",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "strftime('%Y-%m-%d %H:%M:%S', datetime('now'))");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "PlayerPokemons",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "CriminalRecords",
                columns: table => new
                {
                    CriminalRecordId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Info = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    ChatEvidence = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CriminalRecords", x => x.CriminalRecordId);
                    table.ForeignKey(
                        name: "FK_CriminalRecords_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "Pokemons",
                columns: new[] { "PokemonId", "CreatedAt", "Event", "Form", "Generation", "ImageUrl", "Legendary", "Mythical", "Name", "PokedexNumber", "Regional", "RegionalArea", "Release", "Shiny", "Tradable", "Type1", "Type2", "UpdatedAt" },
                values: new object[] { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, 1, "https://archives.bulbagarden.net/media/upload/thumb/2/21/001Bulbasaur.png/250px-001Bulbasaur.png", false, false, "Bulbasaur", 1, false, "Not regional.", "Červenec 2016", true, true, 12, 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.CreateIndex(
                name: "IX_CriminalRecords_PlayerId",
                table: "CriminalRecords",
                column: "PlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CriminalRecords");

            migrationBuilder.DeleteData(
                table: "Pokemons",
                keyColumn: "PokemonId",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Pokemons");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "PointLogs");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PlayersStats");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PlayerPokemons");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Pokemons",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "strftime('%Y-%m-%d %H:%M:%S', datetime('now'))",
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "PointLogs",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "strftime('%Y-%m-%d %H:%M:%S', datetime('now'))",
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "PlayersStats",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "strftime('%Y-%m-%d %H:%M:%S', datetime('now'))",
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Players",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "strftime('%Y-%m-%d %H:%M:%S', datetime('now'))",
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Players",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "strftime('%Y-%m-%d %H:%M:%S', datetime('now'))",
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "PlayerPokemons",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "strftime('%Y-%m-%d %H:%M:%S', datetime('now'))",
                oldClrType: typeof(DateTime),
                oldType: "TEXT");
        }
    }
}
