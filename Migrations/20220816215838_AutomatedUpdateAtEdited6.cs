using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordBot.Migrations
{
    public partial class AutomatedUpdateAtEdited6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MegaEvolution",
                table: "Pokemons",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Pokemons",
                keyColumn: "PokemonId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Event", "RegionalArea", "UpdatedAt" },
                values: new object[] { new DateTime(2022, 8, 16, 23, 58, 37, 0, DateTimeKind.Unspecified), " ", "N/A", new DateTime(2022, 8, 16, 23, 58, 37, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Pokemons",
                columns: new[] { "PokemonId", "CreatedAt", "Event", "Form", "Generation", "ImageUrl", "Legendary", "MegaEvolution", "Mythical", "Name", "PokedexNumber", "Regional", "RegionalArea", "Release", "Shiny", "Tradable", "Type1", "Type2", "UpdatedAt" },
                values: new object[] { 2, new DateTime(2022, 8, 16, 23, 58, 37, 0, DateTimeKind.Unspecified), " ", 0, 1, "https://archives.bulbagarden.net/media/upload/thumb/7/73/002Ivysaur.png/250px-002Ivysaur.png", false, false, false, "Ivysaur", 2, false, "N/A", "Červenec 2016", true, true, 12, 4, new DateTime(2022, 8, 16, 23, 58, 37, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Pokemons",
                columns: new[] { "PokemonId", "CreatedAt", "Event", "Form", "Generation", "ImageUrl", "Legendary", "MegaEvolution", "Mythical", "Name", "PokedexNumber", "Regional", "RegionalArea", "Release", "Shiny", "Tradable", "Type1", "Type2", "UpdatedAt" },
                values: new object[] { 3, new DateTime(2022, 8, 16, 23, 58, 37, 0, DateTimeKind.Unspecified), " ", 0, 1, "https://archives.bulbagarden.net/media/upload/thumb/a/ae/003Venusaur.png/250px-003Venusaur.png", false, false, false, "Venusaur", 3, false, "N/A", "Červenec 2016", true, true, 12, 4, new DateTime(2022, 8, 16, 23, 58, 37, 0, DateTimeKind.Unspecified) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Pokemons",
                keyColumn: "PokemonId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Pokemons",
                keyColumn: "PokemonId",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "MegaEvolution",
                table: "Pokemons");

            migrationBuilder.UpdateData(
                table: "Pokemons",
                keyColumn: "PokemonId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Event", "RegionalArea", "UpdatedAt" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Not regional.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}
