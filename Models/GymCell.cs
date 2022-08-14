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

namespace DiscordBot.Models
{
    public class GymCell
    { 
    
        public GymCell()
        {
            
        }
        private List<Point> _points;

        private GymCell(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        private ILazyLoader LazyLoader { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Description("ID buňky v databázi.")]
        public int IdGymCell { get; private set; }

        [Required]
        [DefaultValue("Buňka")]
        [StringLength(150, ErrorMessage = "Maximální délka Name je 150 znaků.")]
        [Description("Název buňky.")]
        public string Name { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Maximální délka IdCell14 je 100 znaků.")]
        [Description("ID buňky - level 14.")]
        public string IdCell14 { get; set; }

        /*[TextBlob(nameof(PointsBloobbed))]
        [Description("List bodů v buňce.")]
        public List<Point> Points { get; set; }

        public string PointsBloobbed { get; set; }*/

        [Description("List bodů v buňce.")]
        public virtual List<Point> Points
        {
            get => LazyLoader.Load(this, ref _points);
            set => _points = value;
        }

        [MinLength(0, ErrorMessage = "Minilání počet gymů v buňce je 0.")]
        [MaxLength(10, ErrorMessage = "Maximální počet gymů v buňce je 10.")]
        [Description("Počet gymů v buňce.")]
        public int GymCount { get; set; }

        [MinLength(0, ErrorMessage = "Minilání počet pokestopů v buňce je 0.")]
        [MaxLength(70, ErrorMessage = "Maximální počet pokestopů v buňce je 70.")]
        [Description("Počet pokestopů v buňce.")]
        public int PokestopCount { get; set; }

        [MinLength(0, ErrorMessage = "Minilání počet portálů v buňce je 0.")]
        [MaxLength(100, ErrorMessage = "Maximální počet portálů v buňce je 100.")]
        [Description("Počet portálů v buňce.")]
        public int PortalCount { get; set; }

        [Required]
        [DefaultValue(false)]
        [Description("Kontrola buňky.")]
        public bool NeedCheck { get; set; }

        [DataType(DataType.DateTime)]
        [Description("Datum a čas poslední aktualizace údajů.")]
        public DateTime LastUpdate { get; set; }
    }       
}
