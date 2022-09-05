using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordBot.Migrations
{
    public partial class AutomatedUpdateAtEdited7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Pokemons",
                keyColumn: "PokemonId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2022, 9, 5, 1, 15, 35, 0, DateTimeKind.Unspecified), new DateTime(2022, 9, 5, 1, 15, 35, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Pokemons",
                keyColumn: "PokemonId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2022, 9, 5, 1, 15, 35, 0, DateTimeKind.Unspecified), new DateTime(2022, 9, 5, 1, 15, 35, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Pokemons",
                keyColumn: "PokemonId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2022, 9, 5, 1, 15, 35, 0, DateTimeKind.Unspecified), new DateTime(2022, 9, 5, 1, 15, 35, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Pokemons",
                columns: new[] { "PokemonId", "CreatedAt", "Event", "Form", "Generation", "ImageUrl", "Legendary", "MegaEvolution", "Mythical", "Name", "PokedexNumber", "Regional", "RegionalArea", "Release", "Shiny", "Tradable", "Type1", "Type2", "UpdatedAt" },
                values: new object[] { 4, new DateTime(2022, 9, 5, 1, 15, 35, 0, DateTimeKind.Unspecified), " ", 0, 2, "https://archives.bulbagarden.net/media/upload/thumb/4/44/249Lugia.png/250px-249Lugia.png", true, false, false, "Lugia", 249, false, "N/A", "N/A", true, true, 14, 3, new DateTime(2022, 9, 5, 1, 15, 35, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Pokemons",
                columns: new[] { "PokemonId", "CreatedAt", "Event", "Form", "Generation", "ImageUrl", "Legendary", "MegaEvolution", "Mythical", "Name", "PokedexNumber", "Regional", "RegionalArea", "Release", "Shiny", "Tradable", "Type1", "Type2", "UpdatedAt" },
                values: new object[] { 5, new DateTime(2022, 9, 5, 1, 15, 35, 0, DateTimeKind.Unspecified), " ", 0, 2, "https://archives.bulbagarden.net/media/upload/thumb/6/67/250Ho-Oh.png/250px-250Ho-Oh.png", true, false, false, "Ho-Oh", 250, false, "N/A", "N/A", true, true, 10, 3, new DateTime(2022, 9, 5, 1, 15, 35, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Pokemons",
                columns: new[] { "PokemonId", "CreatedAt", "Event", "Form", "Generation", "ImageUrl", "Legendary", "MegaEvolution", "Mythical", "Name", "PokedexNumber", "Regional", "RegionalArea", "Release", "Shiny", "Tradable", "Type1", "Type2", "UpdatedAt" },
                values: new object[] { 6, new DateTime(2022, 9, 5, 1, 15, 35, 0, DateTimeKind.Unspecified), " ", 0, 3, "https://archives.bulbagarden.net/media/upload/thumb/e/e7/386Deoxys.png/250px-386Deoxys.png", true, false, false, "Deoxys (Normal)", 386, false, "N/A", "N/A", true, true, 14, 0, new DateTime(2022, 9, 5, 1, 15, 35, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Pokemons",
                columns: new[] { "PokemonId", "CreatedAt", "Event", "Form", "Generation", "ImageUrl", "Legendary", "MegaEvolution", "Mythical", "Name", "PokedexNumber", "Regional", "RegionalArea", "Release", "Shiny", "Tradable", "Type1", "Type2", "UpdatedAt" },
                values: new object[] { 7, new DateTime(2022, 9, 5, 1, 15, 35, 0, DateTimeKind.Unspecified), " ", 0, 3, "https://archives.bulbagarden.net/media/upload/thumb/d/d8/386Deoxys-Attack.png/110px-386Deoxys-Attack.png", true, false, false, "Deoxys (Attack)", 386, false, "N/A", "N/A", true, true, 14, 0, new DateTime(2022, 9, 5, 1, 15, 35, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Pokemons",
                columns: new[] { "PokemonId", "CreatedAt", "Event", "Form", "Generation", "ImageUrl", "Legendary", "MegaEvolution", "Mythical", "Name", "PokedexNumber", "Regional", "RegionalArea", "Release", "Shiny", "Tradable", "Type1", "Type2", "UpdatedAt" },
                values: new object[] { 8, new DateTime(2022, 9, 5, 1, 15, 35, 0, DateTimeKind.Unspecified), " ", 0, 3, "https://archives.bulbagarden.net/media/upload/thumb/c/cc/386Deoxys-Defense.png/110px-386Deoxys-Defense.png", true, false, false, "Deoxys (Defense)", 386, false, "N/A", "N/A", true, true, 14, 0, new DateTime(2022, 9, 5, 1, 15, 35, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Pokemons",
                columns: new[] { "PokemonId", "CreatedAt", "Event", "Form", "Generation", "ImageUrl", "Legendary", "MegaEvolution", "Mythical", "Name", "PokedexNumber", "Regional", "RegionalArea", "Release", "Shiny", "Tradable", "Type1", "Type2", "UpdatedAt" },
                values: new object[] { 9, new DateTime(2022, 9, 5, 1, 15, 35, 0, DateTimeKind.Unspecified), " ", 0, 3, "https://archives.bulbagarden.net/media/upload/thumb/2/2b/386Deoxys-Speed.png/110px-386Deoxys-Speed.png", true, false, false, "Deoxys (Speed)", 386, false, "N/A", "N/A", true, true, 14, 0, new DateTime(2022, 9, 5, 1, 15, 35, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Pokemons",
                columns: new[] { "PokemonId", "CreatedAt", "Event", "Form", "Generation", "ImageUrl", "Legendary", "MegaEvolution", "Mythical", "Name", "PokedexNumber", "Regional", "RegionalArea", "Release", "Shiny", "Tradable", "Type1", "Type2", "UpdatedAt" },
                values: new object[] { 10, new DateTime(2022, 9, 5, 1, 15, 35, 0, DateTimeKind.Unspecified), " ", 0, 1, "https://archives.bulbagarden.net/media/upload/thumb/4/4e/144Articuno.png/250px-144Articuno.png", true, false, false, "Articuno", 144, false, "N/A", "N/A", true, true, 15, 3, new DateTime(2022, 9, 5, 1, 15, 35, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Pokemons",
                columns: new[] { "PokemonId", "CreatedAt", "Event", "Form", "Generation", "ImageUrl", "Legendary", "MegaEvolution", "Mythical", "Name", "PokedexNumber", "Regional", "RegionalArea", "Release", "Shiny", "Tradable", "Type1", "Type2", "UpdatedAt" },
                values: new object[] { 11, new DateTime(2022, 9, 5, 1, 15, 35, 0, DateTimeKind.Unspecified), " ", 0, 1, "https://archives.bulbagarden.net/media/upload/thumb/e/e3/145Zapdos.png/250px-145Zapdos.png", true, false, false, "Zapdos", 145, false, "N/A", "N/A", true, true, 13, 3, new DateTime(2022, 9, 5, 1, 15, 35, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Pokemons",
                columns: new[] { "PokemonId", "CreatedAt", "Event", "Form", "Generation", "ImageUrl", "Legendary", "MegaEvolution", "Mythical", "Name", "PokedexNumber", "Regional", "RegionalArea", "Release", "Shiny", "Tradable", "Type1", "Type2", "UpdatedAt" },
                values: new object[] { 12, new DateTime(2022, 9, 5, 1, 15, 35, 0, DateTimeKind.Unspecified), " ", 0, 1, "https://archives.bulbagarden.net/media/upload/thumb/1/1b/146Moltres.png/250px-146Moltres.png", true, false, false, "Moltres", 146, false, "N/A", "N/A", true, true, 10, 0, new DateTime(2022, 9, 5, 1, 15, 35, 0, DateTimeKind.Unspecified) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Pokemons",
                keyColumn: "PokemonId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Pokemons",
                keyColumn: "PokemonId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Pokemons",
                keyColumn: "PokemonId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Pokemons",
                keyColumn: "PokemonId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Pokemons",
                keyColumn: "PokemonId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Pokemons",
                keyColumn: "PokemonId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Pokemons",
                keyColumn: "PokemonId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Pokemons",
                keyColumn: "PokemonId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Pokemons",
                keyColumn: "PokemonId",
                keyValue: 12);

            migrationBuilder.UpdateData(
                table: "Pokemons",
                keyColumn: "PokemonId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2022, 8, 16, 23, 58, 37, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 16, 23, 58, 37, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Pokemons",
                keyColumn: "PokemonId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2022, 8, 16, 23, 58, 37, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 16, 23, 58, 37, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Pokemons",
                keyColumn: "PokemonId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2022, 8, 16, 23, 58, 37, 0, DateTimeKind.Unspecified), new DateTime(2022, 8, 16, 23, 58, 37, 0, DateTimeKind.Unspecified) });
        }
    }
}
