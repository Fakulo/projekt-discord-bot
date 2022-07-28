using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using DiscordBot.Models;

namespace DiscordBot.Database
{
    public class PogoContext : DbContext
    {
        public DbSet<Point> Points { get; set; }

        public string DbPath { get; }

        /*public PogoContext()
        {
            var folder = AppDomain.CurrentDomain.BaseDirectory;            
            DbPath = System.IO.Path.Join(folder, "PogoLiberec.db");
        }*/

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=PogoLiberec.db");

        /*protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseMySQL("server=localhost;database=pogoliberec;user=root;password=");*/
    }

    /*public class Point
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int IdPoint { get; set; }

        [Required]
        [StringLength(150, ErrorMessage = "Maximální délka Name je 150 znaků.")]
        public string Name { get; set; }

        [Required]
        [EnumDataType(typeof(PointType))]
        [StringLength(50, ErrorMessage = "Maximální délka Type je 50 znaků.")]
        public string Type { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool ExGym { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Maximální délka IdCell17 je 100 znaků.")]
        public string IdCell17 { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool NeedCheck { get; set; }

        [DataType(DataType.Date)]
        public DateTime LastUpdate { get; set; }

    }
    public enum PointType
    {
        Pokestop,
        Gym,
        Portal
    }*/


}
