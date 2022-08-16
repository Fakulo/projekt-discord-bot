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
using static DiscordBot.Models.Enums;

namespace DiscordBot.Models
{
    public class PointLog
    {
        public PointLog()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Description("ID záznamu v databázi.")]
        public int PointLogId { get; private set; }

        [Required]
        [Description("Počet přidělených bodů.")]
        public int Amount { get; set; }

        [DefaultValue(PointsCategory.Other)]
        [EnumDataType(typeof(PointsCategory))]
        [Description("Kategorie přidělení.")]
        public PointsCategory Category { get; set; }

        [StringLength(200, ErrorMessage = "Maximální délka Description je 200 znaků.")]
        [DefaultValue("N/A")]
        [Description("Popis přidělených bodů.")]
        public string Description { get; set; }

        [StringLength(100, ErrorMessage = "Maximální délka AssignedBy je 100 znaků.")]
        [DefaultValue("N/A")]
        [Description("Uživatel nebo bot, který přidělil body.")]
        public string AssignedBy { get; set; }

        [DataType(DataType.DateTime)]
        [Description("Datum a čas vytvoření záznamu.")]
        public DateTime CreatedAt { get; set; }

        /************************************************************/

        private Player _player;
        private ILazyLoader LazyLoader { get; set; }
        private PointLog(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
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
