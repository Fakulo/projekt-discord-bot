using Google.Common.Geometry;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static DiscordBot.Models.Enums;

namespace DiscordBot.Models
{
    public class Point
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Description("ID bodu v databázi.")]
        public int IdPoint { get; private set; }

        [Required]
        [Description("Zeměpisná šířka bodu.")]
        public double Latitude { get; set; }

        [Required]
        [Description("Zeměpisná délka bodu.")]
        public double Longitude { get; set; }

        [Required]
        [StringLength(150, ErrorMessage = "Maximální délka Name je 150 znaků.")]
        [Description("Název bodu.")]
        public string Name { get; set; }

        [Required]
        [EnumDataType(typeof(Enum))]
        [StringLength(50, ErrorMessage = "Maximální délka Type je 50 znaků.")]
        [Description("Kontrola buňky.")]
        public PointType Type { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Maximální délka IdCell14 je 100 znaků.")]
        [Description("ID buňky - level 14.")]
        public string IdCell14 { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Maximální délka IdCell17 je 100 znaků.")]
        [Description("ID buňky - level 17.")]
        public string IdCell17 { get; set; }        

        [Required]
        [DefaultValue(false)]
        [Description("Kontrola buňky.")]
        public bool NeedCheck { get; set; }

        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Description("Datum a čas poslední aktualizace údajů.")]
        public DateTime LastUpdate { get; set; }     

    }
    //public enum PointType
    //{
    //    Pokestop,
    //    Gym,
    //    ExGym,
    //    Portal
    //}
}
