using Google.Common.Geometry;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;
using static DiscordBot.Models.Enums;

namespace DiscordBot.Models
{
    public class Point : BaseDateEntity
    {
        public Point()
        {

        }        

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Description("ID bodu v databázi.")]
        public int PointId { get; private set; }

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

        [Required]
        [EnumDataType(typeof(NeedCheck))]
        [DefaultValue(NeedCheck.No)]
        [Description("Kontrola buňky.")]
        public NeedCheck NeedCheck { get; set; }
        
        [StringLength(200, ErrorMessage = "Maximální délka CheckedInfo je 200 znaků.")]
        [DefaultValue("")]
        [Description("Informace o zkontrolovaném bodu.")]
        public string CheckedInfo { get; set; }        

        /************************************************************/

        private GymLocationCell _gymLocationCell;
        private ILazyLoader LazyLoader { get; set; }
        private Point(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        [ForeignKey("GymLocationCell")]
        public int GymLocationCellId { get; set; }

        [IgnoreDataMember]
        public GymLocationCell GymLocationCell
        {
            get => LazyLoader.Load(this, ref _gymLocationCell);
            set => _gymLocationCell = value;
        }

    }
}
