using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using DiscordBot.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;

namespace DiscordBot.Database
{
    public class PogoContext : DbContext
    {
        public DbSet<Point> Points { get; set; }
        public DbSet<Pokemon> Pokemons { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<GymLocationCell> GymLocationCells { get; set; }
        public DbSet<PlayerPokemon> PlayerPokemons { get; set; }
        public DbSet<PlayerStat> PlayersStats { get; set; }
        public DbSet<PointLog> PointLogs { get; set; }


        public string DbPath { get; }

        /*public PogoContext()
        {
            var folder = AppDomain.CurrentDomain.BaseDirectory;            
            DbPath = System.IO.Path.Join(folder, "PogoLiberec.db");
        }*/
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GymLocationCell>()
                .HasMany(p => p.Points)
                .WithOne(g => g.GymLocationCell)
                .HasForeignKey(fk => fk.GymLocationCellId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Player>()
                .HasMany(p => p.PlayersStats)
                .WithOne(g => g.Player)
                .HasForeignKey(fk => fk.PlayerId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Player>()
                .HasMany(p => p.PointLogs)
                .WithOne(g => g.Player)
                .HasForeignKey(fk => fk.PlayerId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<PlayerPokemon>().HasKey(sc => new { sc.PokemonId, sc.PlayerStatId });

            modelBuilder.Entity<PlayerPokemon>()
                .HasOne<Pokemon>(sc => sc.Pokemon)
                .WithMany(s => s.PlayersPokemons)
                .HasForeignKey(fk => fk.PokemonId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<PlayerPokemon>()
                .HasOne<PlayerStat>(sc => sc.PlayerStat)
                .WithMany(s => s.PlayersPokemons)
                .HasForeignKey(fk => fk.PlayerStatId)
                .OnDelete(DeleteBehavior.SetNull);

           /* modelBuilder.Entity<GymLocationCell>()
            .Property(b => b.UpdatedAt)
            .HasDefaultValueSql("strftime('%Y-%m-%d %H:%M:%S', datetime('now'))");*/

            modelBuilder.Entity<Player>()
            .Property(b => b.UpdatedAt)
            .HasDefaultValueSql("strftime('%Y-%m-%d %H:%M:%S', datetime('now'))");

            modelBuilder.Entity<Player>()
            .Property(b => b.CreatedAt)
            .HasDefaultValueSql("strftime('%Y-%m-%d %H:%M:%S', datetime('now'))");

            modelBuilder.Entity<PlayerPokemon>()
            .Property(b => b.UpdatedAt)
            .HasDefaultValueSql("strftime('%Y-%m-%d %H:%M:%S', datetime('now'))");

            modelBuilder.Entity<PlayerStat>()
            .Property(b => b.UpdatedAt)
            .HasDefaultValueSql("strftime('%Y-%m-%d %H:%M:%S', datetime('now'))");

           /* modelBuilder.Entity<Point>()
            .Property(b => b.UpdatedAt)
            .HasDefaultValueSql("strftime('%Y-%m-%d %H:%M:%S', datetime('now'))");*/

            modelBuilder.Entity<PointLog>()
            .Property(b => b.CreatedAt)
            .HasDefaultValueSql("strftime('%Y-%m-%d %H:%M:%S', datetime('now'))");

            modelBuilder.Entity<Pokemon>()
            .Property(b => b.UpdatedAt)
            .HasDefaultValueSql("strftime('%Y-%m-%d %H:%M:%S', datetime('now'))");
        }
        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            AddTimestamps();
            return await base.SaveChangesAsync();
        }

        private void AddTimestamps()
        {
            var entries = ChangeTracker
               .Entries()
               .Where(e => e.Entity is BaseDateEntity && (
                       e.State == EntityState.Added
                       || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                string format = "yyyy-MM-dd HH:mm:ss";
                CultureInfo provider = new("en-GB");

                ((BaseDateEntity)entityEntry.Entity).UpdatedAt = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), format, provider);

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseDateEntity)entityEntry.Entity).CreatedAt = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), format, provider);
                }
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=D:\\Tomáš\\Documents\\Visual Studio 2019\\Projekty\\DiscordBot\\bin\\Debug\\net6.0\\PogoLiberec.db");

        /*protected override void OnConfiguring(DbContextOptionsBuilder options)
         * bin\\Debug\\net6.0\\
            => options.UseMySQL("server=localhost;database=pogoliberec;user=root;password=");*/
    }


}
