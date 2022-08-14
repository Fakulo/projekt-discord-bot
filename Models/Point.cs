using Google.Common.Geometry;
using Microsoft.EntityFrameworkCore.Infrastructure;
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

        public Point()
        {

        }

        private GymCell _gymCell;
        private ILazyLoader LazyLoader { get; set; }
        private Point(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

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
        [EnumDataType(typeof(PointType))]
        [Description("Typ bodu.")]
        public PointType Type { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Maximální délka IdCell14 je 100 znaků.")]
        [Description("ID buňky - level 14.")]
        public string IdCell14 { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Maximální délka IdCell17 je 100 znaků.")]
        [Description("ID buňky - level 17.")]
        public string IdCell17 { get; set; }

        public GymCell GymCell
        {
            get => LazyLoader.Load(this, ref _gymCell);
            set => _gymCell = value;
        }

        [Required]
        [DefaultValue(false)]
        [Description("Kontrola buňky.")]
        public bool NeedCheck { get; set; }

        [DataType(DataType.DateTime)]
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
