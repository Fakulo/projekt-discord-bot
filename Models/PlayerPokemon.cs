using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Models
{
    public class PlayerPokemon
    {
        public PlayerPokemon()
        {

        }        

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Description("ID záznamu v databázi.")]
        public int PlayerPokemonId { get; private set; }

        [DefaultValue(false)]
        [Description("Lucky pokemon.")]
        public bool Lucky { get; set; }

        [DefaultValue(false)]
        [Description("Shiny pokemon.")]
        public bool Shiny { get; set; }

        [DefaultValue(false)]
        [Description("PokemonStat na výměnu.")]
        public bool Tradable { get; set; }

        [DataType(DataType.DateTime)]
        [Description("Datum a čas poslední aktualizace údajů.")]
        public DateTime UpdatedAt { get; set; }

        /************************************************************/

        private Pokemon _pokemon;
        private PlayerStat _playerStat;
        private ILazyLoader LazyLoader { get; set; }
        private PlayerPokemon(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        [ForeignKey("Pokemon")]
        public int PokemonId { get; set; }

        [IgnoreDataMember]
        public Pokemon Pokemon
        {
            get => LazyLoader.Load(this, ref _pokemon);
            set => _pokemon = value;
        }

        [ForeignKey("PlayerStat")]
        public int PlayerStatId { get; set; }

        [IgnoreDataMember]
        public PlayerStat PlayerStat
        {
            get => LazyLoader.Load(this, ref _playerStat);
            set => _playerStat = value;
        }

    }
}
