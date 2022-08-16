﻿// <auto-generated />
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
    [Migration("20220815225313_PlayerPokemonTableCreated")]
    partial class PlayerPokemonTableCreated
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.7");

            modelBuilder.Entity("DiscordBot.Models.GymLocationCell", b =>
                {
                    b.Property<int>("GymCellId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CheckedInfo")
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

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

                    b.Property<int>("NeedCheck")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PokestopCount")
                        .HasMaxLength(70)
                        .HasColumnType("INTEGER");

                    b.Property<int>("PortalCount")
                        .HasMaxLength(100)
                        .HasColumnType("INTEGER");

                    b.HasKey("GymCellId");

                    b.ToTable("GymLocationCells");
                });

            modelBuilder.Entity("DiscordBot.Models.Player", b =>
                {
                    b.Property<int>("PlayerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("HomeTerritory")
                        .HasMaxLength(70)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastUpdate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("TEXT");

                    b.Property<int>("Level")
                        .HasMaxLength(50)
                        .HasColumnType("INTEGER");

                    b.Property<int>("Points")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("StatsUpdate")
                        .HasColumnType("TEXT");

                    b.Property<int>("Team")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TrainerCode")
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

                    b.HasKey("PlayerId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("DiscordBot.Models.PlayerPokemon", b =>
                {
                    b.Property<int>("PokemonId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PlayerStatId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Lucky")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PlayerPokemonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Shiny")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Tradable")
                        .HasColumnType("INTEGER");

                    b.HasKey("PokemonId", "PlayerStatId");

                    b.HasIndex("PlayerStatId");

                    b.ToTable("PlayerPokemons");
                });

            modelBuilder.Entity("DiscordBot.Models.PlayerStat", b =>
                {
                    b.Property<int>("PlayerStatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DistanceWalked")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PlayerId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PokeStopsVisited")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PokemonCaught")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("TotalXP")
                        .HasColumnType("INTEGER");

                    b.HasKey("PlayerStatId");

                    b.HasIndex("PlayerId");

                    b.ToTable("PlayersStats");
                });

            modelBuilder.Entity("DiscordBot.Models.Point", b =>
                {
                    b.Property<int>("PointId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CheckedInfo")
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<int>("GymLocationCellId")
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

                    b.Property<int>("NeedCheck")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("PointId");

                    b.HasIndex("GymLocationCellId");

                    b.ToTable("Points");
                });

            modelBuilder.Entity("DiscordBot.Models.Pokemon", b =>
                {
                    b.Property<int>("PokemonId")
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

                    b.Property<int>("PokedexNumber")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Regional")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RegionalArea")
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<string>("Release")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<bool>("Shiny")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Tradable")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type1")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type2")
                        .HasColumnType("INTEGER");

                    b.HasKey("PokemonId");

                    b.ToTable("Pokemons");
                });

            modelBuilder.Entity("DiscordBot.Models.PlayerPokemon", b =>
                {
                    b.HasOne("DiscordBot.Models.PlayerStat", "PlayerStat")
                        .WithMany("PlayersPokemons")
                        .HasForeignKey("PlayerStatId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("DiscordBot.Models.Pokemon", "Pokemon")
                        .WithMany("PlayersPokemons")
                        .HasForeignKey("PokemonId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("PlayerStat");

                    b.Navigation("Pokemon");
                });

            modelBuilder.Entity("DiscordBot.Models.PlayerStat", b =>
                {
                    b.HasOne("DiscordBot.Models.Player", "Player")
                        .WithMany("PlayersStats")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("Player");
                });

            modelBuilder.Entity("DiscordBot.Models.Point", b =>
                {
                    b.HasOne("DiscordBot.Models.GymLocationCell", "GymLocationCell")
                        .WithMany("Points")
                        .HasForeignKey("GymLocationCellId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("GymLocationCell");
                });

            modelBuilder.Entity("DiscordBot.Models.GymLocationCell", b =>
                {
                    b.Navigation("Points");
                });

            modelBuilder.Entity("DiscordBot.Models.Player", b =>
                {
                    b.Navigation("PlayersStats");
                });

            modelBuilder.Entity("DiscordBot.Models.PlayerStat", b =>
                {
                    b.Navigation("PlayersPokemons");
                });

            modelBuilder.Entity("DiscordBot.Models.Pokemon", b =>
                {
                    b.Navigation("PlayersPokemons");
                });
#pragma warning restore 612, 618
        }
    }
}
