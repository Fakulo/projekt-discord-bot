using Microsoft.EntityFrameworkCore.Infrastructure;
using SQLiteNetExtensions.Attributes;
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
    public class GymLocationCell : BaseDateEntity
    {    
        public GymLocationCell()
        {
            
        }        

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Description("ID buňky v databázi.")]
        public int GymCellId { get; private set; }

        [Required]
        [DefaultValue("Buňka")]
        [StringLength(150, ErrorMessage = "Maximální délka Name je 150 znaků.")]
        [Description("Název buňky.")]
        public string Name { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Maximální délka IdCell14 je 100 znaků.")]
        [Description("ID buňky - level 14.")]
        public string IdCell14 { get; set; }

        [Range(0, 10, ErrorMessage = "Rozmezí počtu gymů v buňce je 0-10.")]
        [Description("Počet gymů v buňce.")]
        public int GymCount { get; set; }

        [Range(0, 10, ErrorMessage = "Rozmezí počtu pokestopů v buňce je 0-70.")]
        [Description("Počet pokestopů v buňce.")]
        public int PokestopCount { get; set; }

        [Range(0, 150, ErrorMessage = "Rozmezí počtu portálů v buňce je 0-150.")]
        [Description("Počet portálů v buňce.")]
        public int PortalCount { get; set; }

        [Required]
        [DefaultValue(NeedCheck.No)]
        [EnumDataType(typeof(NeedCheck))]
        [Description("Kontrola buňky.")]
        public NeedCheck NeedCheck { get; set; }

        [DefaultValue(" ")]
        [StringLength(200, ErrorMessage = "Maximální délka CheckedInfo je 200 znaků.")]
        [Description("Informace o zkontrolovaných bodech.")]
        public string CheckedInfo { get; set; }        

        /************************************************************/

        private List<Point> _points;
        private ILazyLoader LazyLoader { get; set; }
        private GymLocationCell(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }        

        [Description("List bodů v buňce.")]
        public virtual List<Point> Points
        {
            get => LazyLoader.Load(this, ref _points);
            set => _points = value;
        }
    }       
}
