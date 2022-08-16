using Microsoft.EntityFrameworkCore.Infrastructure;
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
        public Player()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Description("ID uživatele v databázi.")]
        public int PlayerId { get; private set; }

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
        [MinLength(1, ErrorMessage = "Minimálání level je 1.")]
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
        public int TotalPoints { get; set; }

        [DefaultValue("N/A")]
        [StringLength(30, ErrorMessage = "Maximální délka TrainerCode je 30 znaků.")]
        [Description("Kód pro přidání do přátel ve hře.")]
        public string TrainerCode { get; set; }

        [DefaultValue("N/A")]
        [StringLength(70, ErrorMessage = "Maximální délka HomeTerritory je 70 znaků.")]
        [Description("Domácí území hráče.")]
        public string HomeTerritory { get; set; }

        [Required]
        [DefaultValue(WarningPhase.None)]
        [EnumDataType(typeof(WarningPhase))]
        [Description("Úroveň varování hráče na discordu.")]
        public WarningPhase Warning { get; set; }           

        [DataType(DataType.DateTime)]
        [Description("Datum a čas poslední aktualizace údajů.")]
        public DateTime UpdatedAt { get; set; }

        [DataType(DataType.DateTime)]
        [Description("Datum a čas vytvoření hráče.")]
        public DateTime CreatedAt { get; set; }

        /************************************************************/

        private List<PlayerStat> _playersStats;
        private List<PointLog> _pointLogs;
        private ILazyLoader LazyLoader { get; set; }
        private Player(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        [Description("List seznamu hráče se statistikama.")]
        public virtual List<PlayerStat> PlayersStats
        {
            get => LazyLoader.Load(this, ref _playersStats);
            set => _playersStats = value;
        }

        [Description("List přidělených bodů uživateli.")]
        public virtual List<PointLog> PointLogs
        {
            get => LazyLoader.Load(this, ref _pointLogs);
            set => _pointLogs = value;
        }
    }
}
