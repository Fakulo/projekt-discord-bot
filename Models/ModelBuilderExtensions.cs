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
                }
            );            
        }
    }
}
