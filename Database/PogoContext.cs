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
        public DbSet<Pokemon> Pokemons { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<GymCell> GymsInCells { get; set; }

        public string DbPath { get; }

        /*public PogoContext()
        {
            var folder = AppDomain.CurrentDomain.BaseDirectory;            
            DbPath = System.IO.Path.Join(folder, "PogoLiberec.db");
        }*/
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GymCell>()
                .HasMany(p => p.Points)
                .WithOne(g => g.GymCell)
                .OnDelete(DeleteBehavior.SetNull);

        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=D:\\Tomáš\\Documents\\Visual Studio 2019\\Projekty\\DiscordBot\\bin\\Debug\\net6.0\\PogoLiberec.db");

        /*protected override void OnConfiguring(DbContextOptionsBuilder options)
         * bin\\Debug\\net6.0\\
            => options.UseMySQL("server=localhost;database=pogoliberec;user=root;password=");*/
    }

}
