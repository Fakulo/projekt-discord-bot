using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static DiscordBot.Models.Enums;

namespace DiscordBot.Models
{
    public class Pokemon
    {
        public Pokemon()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Description("ID pokemona v databázi.")]
        public int PokemonId { get; private set; }

        [Required]
        [Description("Číslo pokemona v pokedexu.")]
        public int PokedexNumber { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Maximální délka Name je 100 znaků.")]
        [Description("Jméno pokemona.")]
        public string Name { get; set; }

        [Required]
        [EnumDataType(typeof(PokemonForm))]
        [Description("Forma pokemona.")]
        public PokemonForm Form { get; set; }

        [Url]
        [DataType(DataType.ImageUrl)]
        [StringLength(150, ErrorMessage = "Maximální délka ImageUrl je 150 znaků.")]
        [Description("Odkaz na obrázek pokemona.")]
        public string ImageUrl { get; set; }

        [Required]
        [EnumDataType(typeof(PokemonType))]
        [Description("Typ pokemona 1.")]
        public PokemonType Type1 { get; set; }

        [Required]
        [EnumDataType(typeof(PokemonType))]
        [Description("Typ pokemona 2.")]
        public PokemonType Type2 { get; set; }

        [Required]
        [EnumDataType(typeof(Generation))]       
        [Description("Generace pokemona.")]
        public Generation Generation { get; set; }

        [DefaultValue("")]
        [StringLength(100, ErrorMessage = "Maximální délka Event je 100 znaků.")]
        [Description("PokemonStat je z eventu.")]
        public string Event { get; set; }

        [Required]
        [DefaultValue(false)]
        [Description("PokemonStat může být shiny.")]
        public bool Shiny { get; set; }

        [Required]
        [DefaultValue(true)]
        [Description("Lze pokemona vyměnit.")]
        public bool Tradable { get; set; }

        [Required]
        [DefaultValue(false)]
        [Description("Legendary pokemon.")]
        public bool Legendary { get; set; }

        [Required]
        [DefaultValue(false)]
        [Description("Mythical pokemon.")]
        public bool Mythical { get; set; }

        [Required]
        [DefaultValue(false)]
        [Description("Regional pokemon.")]
        public bool Regional { get; set; }

        [DefaultValue("N/A")]
        [StringLength(200, ErrorMessage = "Maximální délka RegionalArea je 200 znaků.")]
        [Description("Regional pokemon.")]
        public string RegionalArea { get; set; }

        [DefaultValue("N/A")]
        [StringLength(50, ErrorMessage = "Maximální délka Release je 50 znaků.")]
        [Description("Datum uvedení pokemona do hry.")]
        public string Release { get; set; }        

        [DataType(DataType.DateTime)]
        [Description("Datum a čas poslední aktualizace údajů.")]
        public DateTime UpdatedAt
        {
            get
            {
                return this.dateCreated.HasValue
                   ? this.dateCreated.Value
                   : DateTime.Now;
            }

            set { this.dateCreated = value; }
        }

        /************************************************************/

        private DateTime? dateCreated = null;

        private List<PlayerPokemon> _playersPokemons;
        private ILazyLoader LazyLoader { get; set; }
        private Pokemon(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        [Description("List pokemonu v seznamu hráče.")]
        public virtual List<PlayerPokemon> PlayersPokemons
        {
            get => LazyLoader.Load(this, ref _playersPokemons);
            set => _playersPokemons = value;
        }
    }
}
