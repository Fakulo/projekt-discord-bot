using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Models
{
    class Enums
    {
        public enum PointType
        {
            Pokestop,
            Gym,
            ExGym,
            Portal
        }

        public enum PokemonType
        {
            None,
            Normal,
            Fighting,
            Flying,
            Poison,
            Ground,
            Rock,
            Bug,
            Ghost,
            Steel,
            Fire,
            Water,
            Grass,
            Electric,
            Psychic,
            Ice,
            Dragon,
            Dark,
            Fairy
        }
        public enum PokemonForm
        {
            Normal,
            Alolan,
            Galarian,
            Hisuian
        }
        public enum Generation : int
        {
            Kanto = 1,
            Johto = 2,
            Hoenn = 3,
            Sinnoh = 4,
            Unova = 5,
            Kalos = 6,
            Alola = 7,
            Galar = 8,
            Paldea = 9
        }
    }
}
