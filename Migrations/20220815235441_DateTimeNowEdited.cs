using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscordBot.Migrations
{
    public partial class DateTimeNowEdited : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Pokemons",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "strftime('%Y-%m-%d %H:%M:%S', datetime('now'))",
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Points",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "strftime('%Y-%m-%d %H:%M:%S', datetime('now'))",
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "getdate()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Pokemons",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "strftime('%Y-%m-%d %H:%M:%S', datetime('now'))");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Points",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "strftime('%Y-%m-%d %H:%M:%S', datetime('now'))");
        }
    }
}
