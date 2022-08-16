using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordBot.Migrations
{
    public partial class PlayerPokemonTableCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Points_GymsInCells_GymCellIdGymCell",
                table: "Points");

            migrationBuilder.DropTable(
                name: "GymsInCells");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pokemons",
                table: "Pokemons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Points",
                table: "Points");

            migrationBuilder.DropIndex(
                name: "IX_Points_GymCellIdGymCell",
                table: "Points");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Players",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "GymCellIdGymCell",
                table: "Points");

            migrationBuilder.DropColumn(
                name: "IdPlayer",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "DistanceWalked",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "PokeStopsVisited",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "PokemonCaught",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "PokedexId",
                table: "Pokemons",
                newName: "Regional");

            migrationBuilder.RenameColumn(
                name: "IdPokemon",
                table: "Pokemons",
                newName: "PokedexNumber");

            migrationBuilder.RenameColumn(
                name: "IdPoint",
                table: "Points",
                newName: "GymLocationCellId");

            migrationBuilder.RenameColumn(
                name: "TotalXP",
                table: "Players",
                newName: "PlayerId");

            migrationBuilder.AlterColumn<int>(
                name: "PokedexNumber",
                table: "Pokemons",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "PokemonId",
                table: "Pokemons",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<string>(
                name: "RegionalArea",
                table: "Pokemons",
                type: "TEXT",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GymLocationCellId",
                table: "Points",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "PointId",
                table: "Points",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<string>(
                name: "CheckedInfo",
                table: "Points",
                type: "TEXT",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TrainerCode",
                table: "Players",
                type: "TEXT",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<int>(
                name: "PlayerId",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<string>(
                name: "HomeTerritory",
                table: "Players",
                type: "TEXT",
                maxLength: 70,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pokemons",
                table: "Pokemons",
                column: "PokemonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Points",
                table: "Points",
                column: "PointId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Players",
                table: "Players",
                column: "PlayerId");

            migrationBuilder.CreateTable(
                name: "GymLocationCells",
                columns: table => new
                {
                    GymCellId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    IdCell14 = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    GymCount = table.Column<int>(type: "INTEGER", maxLength: 10, nullable: false),
                    PokestopCount = table.Column<int>(type: "INTEGER", maxLength: 70, nullable: false),
                    PortalCount = table.Column<int>(type: "INTEGER", maxLength: 100, nullable: false),
                    NeedCheck = table.Column<int>(type: "INTEGER", nullable: false),
                    CheckedInfo = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GymLocationCells", x => x.GymCellId);
                });

            migrationBuilder.CreateTable(
                name: "PlayersStats",
                columns: table => new
                {
                    PlayerStatId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DistanceWalked = table.Column<int>(type: "INTEGER", nullable: false),
                    PokemonCaught = table.Column<int>(type: "INTEGER", nullable: false),
                    PokeStopsVisited = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalXP = table.Column<int>(type: "INTEGER", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayersStats", x => x.PlayerStatId);
                    table.ForeignKey(
                        name: "FK_PlayersStats_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "PlayerPokemons",
                columns: table => new
                {
                    PokemonId = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerStatId = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerPokemonId = table.Column<int>(type: "INTEGER", nullable: false),
                    Lucky = table.Column<bool>(type: "INTEGER", nullable: false),
                    Shiny = table.Column<bool>(type: "INTEGER", nullable: false),
                    Tradable = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerPokemons", x => new { x.PokemonId, x.PlayerStatId });
                    table.ForeignKey(
                        name: "FK_PlayerPokemons_PlayersStats_PlayerStatId",
                        column: x => x.PlayerStatId,
                        principalTable: "PlayersStats",
                        principalColumn: "PlayerStatId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_PlayerPokemons_Pokemons_PokemonId",
                        column: x => x.PokemonId,
                        principalTable: "Pokemons",
                        principalColumn: "PokemonId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Points_GymLocationCellId",
                table: "Points",
                column: "GymLocationCellId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerPokemons_PlayerStatId",
                table: "PlayerPokemons",
                column: "PlayerStatId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayersStats_PlayerId",
                table: "PlayersStats",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Points_GymLocationCells_GymLocationCellId",
                table: "Points",
                column: "GymLocationCellId",
                principalTable: "GymLocationCells",
                principalColumn: "GymCellId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Points_GymLocationCells_GymLocationCellId",
                table: "Points");

            migrationBuilder.DropTable(
                name: "GymLocationCells");

            migrationBuilder.DropTable(
                name: "PlayerPokemons");

            migrationBuilder.DropTable(
                name: "PlayersStats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pokemons",
                table: "Pokemons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Points",
                table: "Points");

            migrationBuilder.DropIndex(
                name: "IX_Points_GymLocationCellId",
                table: "Points");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Players",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "PokemonId",
                table: "Pokemons");

            migrationBuilder.DropColumn(
                name: "RegionalArea",
                table: "Pokemons");

            migrationBuilder.DropColumn(
                name: "PointId",
                table: "Points");

            migrationBuilder.DropColumn(
                name: "CheckedInfo",
                table: "Points");

            migrationBuilder.DropColumn(
                name: "HomeTerritory",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "Regional",
                table: "Pokemons",
                newName: "PokedexId");

            migrationBuilder.RenameColumn(
                name: "PokedexNumber",
                table: "Pokemons",
                newName: "IdPokemon");

            migrationBuilder.RenameColumn(
                name: "GymLocationCellId",
                table: "Points",
                newName: "IdPoint");

            migrationBuilder.RenameColumn(
                name: "PlayerId",
                table: "Players",
                newName: "TotalXP");

            migrationBuilder.AlterColumn<int>(
                name: "IdPokemon",
                table: "Pokemons",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "IdPoint",
                table: "Points",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "GymCellIdGymCell",
                table: "Points",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TrainerCode",
                table: "Players",
                type: "TEXT",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TotalXP",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "IdPlayer",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "DistanceWalked",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PokeStopsVisited",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PokemonCaught",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Players",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pokemons",
                table: "Pokemons",
                column: "IdPokemon");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Points",
                table: "Points",
                column: "IdPoint");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Players",
                table: "Players",
                column: "IdPlayer");

            migrationBuilder.CreateTable(
                name: "GymsInCells",
                columns: table => new
                {
                    IdGymCell = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GymCount = table.Column<int>(type: "INTEGER", maxLength: 10, nullable: false),
                    IdCell14 = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    NeedCheck = table.Column<bool>(type: "INTEGER", nullable: false),
                    PokestopCount = table.Column<int>(type: "INTEGER", maxLength: 70, nullable: false),
                    PortalCount = table.Column<int>(type: "INTEGER", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GymsInCells", x => x.IdGymCell);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Points_GymCellIdGymCell",
                table: "Points",
                column: "GymCellIdGymCell");

            migrationBuilder.AddForeignKey(
                name: "FK_Points_GymsInCells_GymCellIdGymCell",
                table: "Points",
                column: "GymCellIdGymCell",
                principalTable: "GymsInCells",
                principalColumn: "IdGymCell",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
