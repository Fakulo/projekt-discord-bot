using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordBot.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GymsInCells",
                columns: table => new
                {
                    IdGymCell = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    PointsBloobbed = table.Column<string>(type: "TEXT", nullable: true),
                    GymCount = table.Column<int>(type: "INTEGER", maxLength: 10, nullable: false),
                    PokestopCount = table.Column<int>(type: "INTEGER", maxLength: 70, nullable: false),
                    PortalCount = table.Column<int>(type: "INTEGER", maxLength: 100, nullable: false),
                    NeedCheck = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GymsInCells", x => x.IdGymCell);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    IdPlayer = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    UserId = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Level = table.Column<int>(type: "INTEGER", maxLength: 50, nullable: false),
                    Team = table.Column<int>(type: "INTEGER", nullable: false),
                    Points = table.Column<int>(type: "INTEGER", nullable: false),
                    TrainerCode = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    Warning = table.Column<int>(type: "INTEGER", nullable: false),
                    DistanceWalked = table.Column<int>(type: "INTEGER", nullable: false),
                    PokemonCaught = table.Column<int>(type: "INTEGER", nullable: false),
                    PokeStopsVisited = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalXP = table.Column<int>(type: "INTEGER", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    StatsUpdate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.IdPlayer);
                });

            migrationBuilder.CreateTable(
                name: "Pokemons",
                columns: table => new
                {
                    IdPokemon = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PokedexId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Form = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    Type1 = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Type2 = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Generation = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Event = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Shiny = table.Column<bool>(type: "INTEGER", nullable: false),
                    Tradable = table.Column<bool>(type: "INTEGER", nullable: false),
                    Legendary = table.Column<bool>(type: "INTEGER", nullable: false),
                    Mythical = table.Column<bool>(type: "INTEGER", nullable: false),
                    Release = table.Column<string>(type: "TEXT", nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pokemons", x => x.IdPokemon);
                });

            migrationBuilder.CreateTable(
                name: "Points",
                columns: table => new
                {
                    IdPoint = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Type = table.Column<int>(type: "INTEGER", maxLength: 50, nullable: false),
                    IdCell14 = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    IdCell17 = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    NeedCheck = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GymCellIdGymCell = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Points", x => x.IdPoint);
                    table.ForeignKey(
                        name: "FK_Points_GymsInCells_GymCellIdGymCell",
                        column: x => x.GymCellIdGymCell,
                        principalTable: "GymsInCells",
                        principalColumn: "IdGymCell");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Points_GymCellIdGymCell",
                table: "Points",
                column: "GymCellIdGymCell");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Points");

            migrationBuilder.DropTable(
                name: "Pokemons");

            migrationBuilder.DropTable(
                name: "GymsInCells");
        }
    }
}
