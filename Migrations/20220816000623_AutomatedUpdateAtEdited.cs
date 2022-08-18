using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordBot.Migrations
{
    public partial class AutomatedUpdateAtEdited : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "PointLogs",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "strftime('%Y-%m-%d %H:%M:%S', datetime('now'))",
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "PlayersStats",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "strftime('%Y-%m-%d %H:%M:%S', datetime('now'))",
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Players",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "strftime('%Y-%m-%d %H:%M:%S', datetime('now'))",
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Players",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "strftime('%Y-%m-%d %H:%M:%S', datetime('now'))",
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "PlayerPokemons",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "strftime('%Y-%m-%d %H:%M:%S', datetime('now'))",
                oldClrType: typeof(DateTime),
                oldType: "TEXT");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "PointLogs",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "strftime('%Y-%m-%d %H:%M:%S', datetime('now'))");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "PlayersStats",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "strftime('%Y-%m-%d %H:%M:%S', datetime('now'))");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Players",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "strftime('%Y-%m-%d %H:%M:%S', datetime('now'))");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Players",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "strftime('%Y-%m-%d %H:%M:%S', datetime('now'))");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "PlayerPokemons",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "strftime('%Y-%m-%d %H:%M:%S', datetime('now'))");
        }
    }
}
