using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DiscordBot.Models.Enums;

namespace DiscordBot.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            string format = "yyyy-MM-dd HH:mm:ss";
            CultureInfo provider = new("en-US");
            var now = DateTime.ParseExact(DateTime.Now.ToString(format), format, provider);

            modelBuilder.Entity<Pokemon>().HasData(
                new Pokemon {
                    PokemonId = 1,
                    PokedexNumber = 1,
                    Name = "Bulbasaur",
                    Form = PokemonForm.Normal,
                    ImageUrl = "https://archives.bulbagarden.net/media/upload/thumb/2/21/001Bulbasaur.png/250px-001Bulbasaur.png",
                    Type1 = PokemonType.Grass,
                    Type2 = PokemonType.Poison,
                    Event = " ",
                    Generation = Generation.Kanto,
                    Shiny = true,
                    Tradable = true,
                    Legendary = false,
                    Mythical = false,
                    Regional = false,
                    RegionalArea = "N/A",
                    Release = "Červenec 2016",
                    CreatedAt = now,
                    UpdatedAt = now
                    
                },
                new Pokemon
                {
                    PokemonId = 2,
                    PokedexNumber = 2,
                    Name = "Ivysaur",
                    Form = PokemonForm.Normal,
                    ImageUrl = "https://archives.bulbagarden.net/media/upload/thumb/7/73/002Ivysaur.png/250px-002Ivysaur.png",
                    Type1 = PokemonType.Grass,
                    Type2 = PokemonType.Poison,
                    Event = " ",
                    Generation = Generation.Kanto,
                    Shiny = true,
                    Tradable = true,
                    Legendary = false,
                    Mythical = false,
                    Regional = false,
                    RegionalArea = "N/A",
                    Release = "Červenec 2016",
                    CreatedAt = now,
                    UpdatedAt = now
                },
                new Pokemon
                {
                    PokemonId = 3,
                    PokedexNumber = 3,
                    Name = "Venusaur",
                    Form = PokemonForm.Normal,
                    ImageUrl = "https://archives.bulbagarden.net/media/upload/thumb/a/ae/003Venusaur.png/250px-003Venusaur.png",
                    Type1 = PokemonType.Grass,
                    Type2 = PokemonType.Poison,
                    Event = " ",
                    Generation = Generation.Kanto,
                    Shiny = true,
                    Tradable = true,
                    Legendary = false,
                    Mythical = false,
                    Regional = false,
                    RegionalArea = "N/A",
                    Release = "Červenec 2016",
                    CreatedAt = now,
                    UpdatedAt = now
                },
                new Pokemon
                {
                    PokemonId = 4,
                    PokedexNumber = 249,
                    Name = "Lugia",
                    Form = PokemonForm.Normal,
                    ImageUrl = "https://archives.bulbagarden.net/media/upload/thumb/4/44/249Lugia.png/250px-249Lugia.png",
                    Type1 = PokemonType.Psychic,
                    Type2 = PokemonType.Flying,
                    Event = " ",
                    Generation = Generation.Johto,
                    Shiny = true,
                    Tradable = true,
                    Legendary = true,
                    Mythical = false,
                    Regional = false,
                    RegionalArea = "N/A",
                    Release = "N/A",
                    CreatedAt = now,
                    UpdatedAt = now
                },
                new Pokemon
                {
                    PokemonId = 5,
                    PokedexNumber = 250,
                    Name = "Ho-Oh",
                    Form = PokemonForm.Normal,
                    ImageUrl = "https://archives.bulbagarden.net/media/upload/thumb/6/67/250Ho-Oh.png/250px-250Ho-Oh.png",
                    Type1 = PokemonType.Fire,
                    Type2 = PokemonType.Flying,
                    Event = " ",
                    Generation = Generation.Johto,
                    Shiny = true,
                    Tradable = true,
                    Legendary = true,
                    Mythical = false,
                    Regional = false,
                    RegionalArea = "N/A",
                    Release = "N/A",
                    CreatedAt = now,
                    UpdatedAt = now
                },
                new Pokemon
                {
                    PokemonId = 6,
                    PokedexNumber = 386,
                    Name = "Deoxys (Normal)",
                    Form = PokemonForm.Normal,
                    ImageUrl = "https://archives.bulbagarden.net/media/upload/thumb/e/e7/386Deoxys.png/250px-386Deoxys.png",
                    Type1 = PokemonType.Psychic,
                    Type2 = PokemonType.None,
                    Event = " ",
                    Generation = Generation.Hoenn,
                    Shiny = true,
                    Tradable = true,
                    Legendary = true,
                    Mythical = false,
                    Regional = false,
                    RegionalArea = "N/A",
                    Release = "N/A",
                    CreatedAt = now,
                    UpdatedAt = now
                },
                new Pokemon
                {
                    PokemonId = 7,
                    PokedexNumber = 386,
                    Name = "Deoxys (Attack)",
                    Form = PokemonForm.Normal,
                    ImageUrl = "https://archives.bulbagarden.net/media/upload/thumb/d/d8/386Deoxys-Attack.png/110px-386Deoxys-Attack.png",
                    Type1 = PokemonType.Psychic,
                    Type2 = PokemonType.None,
                    Event = " ",
                    Generation = Generation.Hoenn,
                    Shiny = true,
                    Tradable = true,
                    Legendary = true,
                    Mythical = false,
                    Regional = false,
                    RegionalArea = "N/A",
                    Release = "N/A",
                    CreatedAt = now,
                    UpdatedAt = now
                },
                new Pokemon
                {
                    PokemonId = 8,
                    PokedexNumber = 386,
                    Name = "Deoxys (Defense)",
                    Form = PokemonForm.Normal,
                    ImageUrl = "https://archives.bulbagarden.net/media/upload/thumb/c/cc/386Deoxys-Defense.png/110px-386Deoxys-Defense.png",
                    Type1 = PokemonType.Psychic,
                    Type2 = PokemonType.None,
                    Event = " ",
                    Generation = Generation.Hoenn,
                    Shiny = true,
                    Tradable = true,
                    Legendary = true,
                    Mythical = false,
                    Regional = false,
                    RegionalArea = "N/A",
                    Release = "N/A",
                    CreatedAt = now,
                    UpdatedAt = now
                },
                new Pokemon
                {
                    PokemonId = 9,
                    PokedexNumber = 386,
                    Name = "Deoxys (Speed)",
                    Form = PokemonForm.Normal,
                    ImageUrl = "https://archives.bulbagarden.net/media/upload/thumb/2/2b/386Deoxys-Speed.png/110px-386Deoxys-Speed.png",
                    Type1 = PokemonType.Psychic,
                    Type2 = PokemonType.None,
                    Event = " ",
                    Generation = Generation.Hoenn,
                    Shiny = true,
                    Tradable = true,
                    Legendary = true,
                    Mythical = false,
                    Regional = false,
                    RegionalArea = "N/A",
                    Release = "N/A",
                    CreatedAt = now,
                    UpdatedAt = now
                },
                new Pokemon
                {
                    PokemonId = 10,
                    PokedexNumber = 144,
                    Name = "Articuno",
                    Form = PokemonForm.Normal,
                    ImageUrl = "https://archives.bulbagarden.net/media/upload/thumb/4/4e/144Articuno.png/250px-144Articuno.png",
                    Type1 = PokemonType.Ice,
                    Type2 = PokemonType.Flying,
                    Event = " ",
                    Generation = Generation.Kanto,
                    Shiny = true,
                    Tradable = true,
                    Legendary = true,
                    Mythical = false,
                    Regional = false,
                    RegionalArea = "N/A",
                    Release = "N/A",
                    CreatedAt = now,
                    UpdatedAt = now
                },
                new Pokemon
                {
                    PokemonId = 11,
                    PokedexNumber = 145,
                    Name = "Zapdos",
                    Form = PokemonForm.Normal,
                    ImageUrl = "https://archives.bulbagarden.net/media/upload/thumb/e/e3/145Zapdos.png/250px-145Zapdos.png",
                    Type1 = PokemonType.Electric,
                    Type2 = PokemonType.Flying,
                    Event = " ",
                    Generation = Generation.Kanto,
                    Shiny = true,
                    Tradable = true,
                    Legendary = true,
                    Mythical = false,
                    Regional = false,
                    RegionalArea = "N/A",
                    Release = "N/A",
                    CreatedAt = now,
                    UpdatedAt = now
                },
                new Pokemon
                {
                    PokemonId = 12,
                    PokedexNumber = 146,
                    Name = "Moltres",
                    Form = PokemonForm.Normal,
                    ImageUrl = "https://archives.bulbagarden.net/media/upload/thumb/1/1b/146Moltres.png/250px-146Moltres.png",
                    Type1 = PokemonType.Fire,
                    Type2 = PokemonType.None,
                    Event = " ",
                    Generation = Generation.Kanto,
                    Shiny = true,
                    Tradable = true,
                    Legendary = true,
                    Mythical = false,
                    Regional = false,
                    RegionalArea = "N/A",
                    Release = "N/A",
                    CreatedAt = now,
                    UpdatedAt = now
                }
            );            
        }
    }
}
