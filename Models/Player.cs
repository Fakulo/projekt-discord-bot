using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DiscordBot.Models.Enums;

namespace DiscordBot.Models
{
    public class Player
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Description("ID uživatele v databázi.")]
        public int IdPlayer { get; private set; }

        [Required]
        [StringLength(100, ErrorMessage = "Maximální délka UserName je 150 znaků.")]
        [Description("Uživatelské jméno na serveru, které by mělo odpovídat přezdívce ve hře.")]
        public string UserName { get; set; }

        [Required]
        [StringLength(150, ErrorMessage = "Maximální délka UserId je 150 znaků.")]
        [Description("ID uživatele na Discordu.")]
        public string UserId { get; set; }

        [Required]
        [DefaultValue(1)]
        [MinLength(1, ErrorMessage = "Minilání level je 1.")]
        [MaxLength(50, ErrorMessage = "Maximální level je 50.")]
        [Description("Úroveň hráče ve hře.")]
        public int Level { get; set; }

        [Required]
        [DefaultValue(Team.None)]
        [EnumDataType(typeof(Team))]
        [Description("Tým hráče ve hře.")]
        public Team Team { get; set; }

        [Required]
        [DefaultValue(0)]
        [MinLength(0, ErrorMessage = "Minimální počet bodů je 0.")]
        [Description("Celkový počet bodů, které získal hráč na Discordu za aktivitu.")]
        public int Points { get; set; }

        [Required]
        [DefaultValue("")]
        [StringLength(30, ErrorMessage = "Maximální délka TrainerCode je 30 znaků.")]
        [Description("Kód pro přidání do přátel ve hře.")]
        public string TrainerCode { get; set; }

        [Required]
        [DefaultValue(WarningPhase.None)]
        [EnumDataType(typeof(WarningPhase))]
        [Description("Úroveň varování hráče na discordu.")]
        public WarningPhase Warning { get; set; }

        [DefaultValue(0)]
        [MinLength(0, ErrorMessage = "Minimální počet nachozených kilometrů je 0.")]
        [Description("Celkový počet nachozených kilometrů ve hře.")]
        public int DistanceWalked { get; set; }

        [DefaultValue(0)]
        [MinLength(0, ErrorMessage = "Minimální počet chycených pokémonů je 0.")]
        [Description("Celkový počet chycených pokémonů ve hře.")]
        public int PokemonCaught { get; set; }

        [DefaultValue(0)]
        [MinLength(0, ErrorMessage = "Minimální počet navštívených pokestopů je 0.")]
        [Description("Celkový počet navštívených pokestopů ve hře.")]
        public int PokeStopsVisited { get; set; }

        [DefaultValue(0)]
        [MinLength(0, ErrorMessage = "Minimální počet nasbíraných zkušeností je 0.")]
        [Description("Celkový počet nasbíraných zkušeností ve hře.")]
        public int TotalXP { get; set; }

        [DataType(DataType.Date)]
        [Description("Datum založení účtu ve hře.")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Description("Datum a čas poslední aktualizace údajů ze hry.")]
        public DateTime StatsUpdate { get; set; }

        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Description("Datum a čas poslední aktualizace údajů.")]
        public DateTime LastUpdate { get; set; }
    }
}
