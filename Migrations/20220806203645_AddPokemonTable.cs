using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordBot.Migrations
{
    public partial class AddPokemonTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        { 
            migrationBuilder.DropTable(
                name: "Pokemons");
        }
    }
}
