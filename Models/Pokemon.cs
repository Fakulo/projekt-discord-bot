using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DiscordBot.Models
{
    public class Pokemon
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int IdPokemon { get; private set; }

        [Required]
        public int PokedexId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Maximální délka Name je 100 znaků.")]
        public string Name { get; set; }

        [Required]
        [EnumDataType(typeof(Enums.PokemonForm))]
        [StringLength(50, ErrorMessage = "Maximální délka PokemonForm je 50 znaků.")]
        public string Form { get; set; }

        [Url]
        [DataType(DataType.ImageUrl)]
        [StringLength(150, ErrorMessage = "Maximální délka ImageUrl je 150 znaků.")]
        public string ImageUrl { get; set; }

        [Required]
        [EnumDataType(typeof(Enums.PokemonType))]
        [StringLength(50, ErrorMessage = "Maximální délka PokemonType1 je 50 znaků.")]
        public string Type1 { get; set; }

        [Required]
        [EnumDataType(typeof(Enums.PokemonType))]
        [StringLength(50, ErrorMessage = "Maximální délka PokemonType2 je 50 znaků.")]
        public string Type2 { get; set; }

        [Required]
        [EnumDataType(typeof(Enums.Generation))]
        [StringLength(50, ErrorMessage = "Maximální délka Generation je 50 znaků.")]
        public string Generation { get; set; }

        [DefaultValue("")]
        [StringLength(100, ErrorMessage = "Maximální délka Event je 100 znaků.")]
        public string Event { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool Shiny { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool Tradable { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool Legendary { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool Mythical { get; set; }

        [DefaultValue("N/A")]
        public string Release { get; set; }        

        [DataType(DataType.Date)]
        public DateTime LastUpdate { get; set; }
    }
}
