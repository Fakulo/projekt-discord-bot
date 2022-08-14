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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Description("ID pokemona v databázi.")]
        public int IdPokemon { get; private set; }

        [Required]
        [Description("ID pokemona v pokedexu.")]
        public int PokedexId { get; set; }

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
        [Description("Pokemon je z eventu.")]
        public string Event { get; set; }

        [Required]
        [DefaultValue(false)]
        [Description("Pokemon může být shiny.")]
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

        [DefaultValue("N/A")]
        [Description("Datum uvedení pokemona do hry.")]
        public string Release { get; set; }        

        [DataType(DataType.DateTime)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Description("Datum a čas poslední aktualizace údajů.")]
        public DateTime LastUpdate { get; set; }
    }
}
