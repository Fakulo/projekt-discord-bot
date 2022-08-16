using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
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
    public class PlayerStat : BaseDateEntity
    {
        public PlayerStat()
        {

        }       

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Description("ID uživatele v databázi.")]
        public int PlayerStatId { get; private set; }               

        [DefaultValue(0)]
        [Range(0, int.MaxValue, ErrorMessage = "Minimální počet nachozených kilometrů je 0.")]
        [Description("Celkový počet nachozených kilometrů ve hře.")]
        public int DistanceWalked { get; set; }

        [DefaultValue(0)]
        [Range(0, int.MaxValue, ErrorMessage = "Minimální počet chycených pokémonů je 0.")]
        [Description("Celkový počet chycených pokémonů ve hře.")]
        public int PokemonCaught { get; set; }

        [DefaultValue(0)]
        [Range(0, int.MaxValue, ErrorMessage = "Minimální počet navštívených pokestopů je 0.")]
        [Description("Celkový počet navštívených pokestopů ve hře.")]
        public int PokeStopsVisited { get; set; }

        [DefaultValue(0)]
        [Range(0, int.MaxValue, ErrorMessage = "Minimální počet nasbíraných zkušeností je 0.")]
        [Description("Celkový počet nasbíraných zkušeností ve hře.")]
        public int TotalXP { get; set; }

        [DataType(DataType.DateTime)]
        [Description("Datum založení účtu ve hře.")]
        public DateTime StartDate { get; set; }

        /************************************************************/

        private List<PlayerPokemon> _playersPokemons;
        private Player _player;
        private ILazyLoader LazyLoader { get; set; }
        private PlayerStat(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        [Description("List pokemonu v seznamu hráče.")]
        public virtual List<PlayerPokemon> PlayersPokemons
        {
            get => LazyLoader.Load(this, ref _playersPokemons);
            set => _playersPokemons = value;
        }

        [ForeignKey("Player")]
        public int PlayerId { get; set; }

        [IgnoreDataMember]
        public virtual Player Player
        {
            get => LazyLoader.Load(this, ref _player);
            set => _player = value;
        }
    }
}
