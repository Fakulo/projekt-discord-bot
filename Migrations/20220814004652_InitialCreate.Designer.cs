// <auto-generated />
using System;
using DiscordBot.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DiscordBot.Migrations
{
    [DbContext(typeof(PogoContext))]
    [Migration("20220814004652_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.7");

            modelBuilder.Entity("DiscordBot.Models.GymCell", b =>
                {
                    b.Property<int>("IdGymCell")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("GymCount")
                        .HasMaxLength(10)
                        .HasColumnType("INTEGER");

                    b.Property<string>("IdCell14")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("TEXT");

                    b.Property<bool>("NeedCheck")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PokestopCount")
                        .HasMaxLength(70)
                        .HasColumnType("INTEGER");

                    b.Property<int>("PortalCount")
                        .HasMaxLength(100)
                        .HasColumnType("INTEGER");

                    b.HasKey("IdGymCell");

                    b.ToTable("GymsInCells");
                });

            modelBuilder.Entity("DiscordBot.Models.Player", b =>
                {
                    b.Property<int>("IdPlayer")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DistanceWalked")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastUpdate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("TEXT");

                    b.Property<int>("Level")
                        .HasMaxLength(50)
                        .HasColumnType("INTEGER");

                    b.Property<int>("Points")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PokeStopsVisited")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PokemonCaught")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StatsUpdate")
                        .HasColumnType("TEXT");

                    b.Property<int>("Team")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TotalXP")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TrainerCode")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<int>("Warning")
                        .HasColumnType("INTEGER");

                    b.HasKey("IdPlayer");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("DiscordBot.Models.Point", b =>
                {
                    b.Property<int>("IdPoint")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("GymCellIdGymCell")
                        .HasColumnType("INTEGER");

                    b.Property<string>("IdCell14")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("IdCell17")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("TEXT");

                    b.Property<double>("Latitude")
                        .HasColumnType("REAL");

                    b.Property<double>("Longitude")
                        .HasColumnType("REAL");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("TEXT");

                    b.Property<bool>("NeedCheck")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("IdPoint");

                    b.HasIndex("GymCellIdGymCell");

                    b.ToTable("Points");
                });

            modelBuilder.Entity("DiscordBot.Models.PokemonStat", b =>
                {
                    b.Property<int>("IdPokemon")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Event")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<int>("Form")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Generation")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(150)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastUpdate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Legendary")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Mythical")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<int>("PokedexId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Release")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Shiny")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Tradable")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type1")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type2")
                        .HasColumnType("INTEGER");

                    b.HasKey("IdPokemon");

                    b.ToTable("Pokemons");
                });

            modelBuilder.Entity("DiscordBot.Models.Point", b =>
                {
                    b.HasOne("DiscordBot.Models.GymCell", "GymCell")
                        .WithMany("Points")
                        .HasForeignKey("GymCellIdGymCell")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("GymCell");
                });

            modelBuilder.Entity("DiscordBot.Models.GymCell", b =>
                {
                    b.Navigation("Points");
                });
#pragma warning restore 612, 618
        }
    }
}
