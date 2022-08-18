using DiscordBot.Database;
using DiscordBot.Models;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static DiscordBot.Models.Enums;

namespace DiscordBot.Algorithm
{
    class Methods
    {        
        internal static async void SendMessage(Task<DiscordChannel> chnl, string text)
        {
            await chnl.Result.SendMessageAsync(text).ConfigureAwait(false);
        }
        /// <summary>
        /// MessageBox bez reakcí, který zobrazí zprávu.
        /// </summary>
        /// <param name="chnl">Kanál, kde se MessageBox zobrazí</param>
        /// <param name="title">Nadpis MessageBoxu</param>
        /// <param name="description">Text v MessageBoxu</param>
        /// <param name="color">Barva MessageBoxu</param>
        internal static async void SendBoxMessage(Task<DiscordChannel> chnl, string title, string description, DiscordColor color)
        {
            var embed = new DiscordEmbedBuilder
            {
                Title = title,
                Description = description,
                Color = color,                
                Timestamp = DateTime.Now
            };
            await chnl.Result.SendMessageAsync("", false, embed).ConfigureAwait(false);
        }
        /// <summary>
        /// MessageBox s reakcí potvrdit nebo zamítnout, který zobrazí zprávu.
        /// </summary>
        /// <param name="ctx">CommandContext</param>
        /// <param name="chnl">Kanál, kde se MessageBox zobrazí</param>
        /// <param name="title">Nadpis MessageBoxu</param>
        /// <param name="description">Text v MessageBoxu</param>
        /// <param name="color">Barva MessageBoxu</param>
        internal static async void SendBoxMessage(CommandContext ctx, Task<DiscordChannel> chnl, string title, string description, DiscordColor color)
        {
            var embed = new DiscordEmbedBuilder
            {
                Title = title,
                Description = description,
                Color = color,
                Timestamp = DateTime.Now
            };
            await chnl.Result.SendMessageAsync("", false, embed).ConfigureAwait(false);

            //await msg.CreateReactionAsync(Emoji.GetEmoji(ctx, Emoji.GreenCheck.ToString()));
            //await msg.CreateReactionAsync(Emoji.GetEmoji(ctx, Emoji.RedCross.ToString()));
        }
        /// <summary>
        /// MessageBox bez reakcí, který zobrazí zprávu s odkazem na mapu bodu.
        /// </summary>
        /// <param name="chnl">Kanál, kde se MessageBox zobrazí</param>
        /// <param name="title">Nadpis MessageBoxu</param>
        /// <param name="description">Text v MessageBoxu</param>
        /// <param name="color">Barva MessageBoxu</param>
        /// <param name="latitude">Zeměpisná šířka bodu</param>
        /// <param name="longitude">Zeměpisná délka bodu</param>
        internal static async void SendBoxMessage(Task<DiscordChannel> chnl, string title, string description, DiscordColor color, double latitude, double longitude)
        {
            var embed = new DiscordEmbedBuilder
            {
                Title = title,
                Description = description,
                Color = color,
                Timestamp = DateTime.Now,
                Url = "https://www.google.cz/maps/place/" + latitude.ToString().Replace(",",".") + "," + longitude.ToString().Replace(",",".")
            };
            await chnl.Result.SendMessageAsync("", false, embed).ConfigureAwait(false);
        }
        /// <summary>
        /// MessageBox bez reakcí, který zobrazí zprávu s odkazem na mapu bodu.
        /// </summary>
        /// <param name="chnl">Kanál, kde se MessageBox zobrazí</param>
        /// <param name="title">Nadpis MessageBoxu</param>
        /// <param name="description">Text v MessageBoxu</param>
        /// <param name="color">Barva MessageBoxu</param>
        /// <param name="p">Bod</param>
        internal static async void SendBoxMessage(Task<DiscordChannel> chnl, string title, string description, DiscordColor color, Point p)
        {
            var embed = new DiscordEmbedBuilder
            {
                Title = title,
                Description = description,
                Color = color,
                Timestamp = DateTime.Now,
                Url = "https://www.google.cz/maps/place/" + p.Latitude.ToString().Replace(",", ".") + "," + p.Longitude.ToString().Replace(",", ".")
            };
            await chnl.Result.SendMessageAsync("", false, embed).ConfigureAwait(false);
        }

        /// <summary>
        /// MessageBox bez reakcí, který zobrazí zprávu s odkazem na mapu bodu.
        /// </summary>
        /// <param name="chnl">Kanál, kde se MessageBox zobrazí</param>
        /// <param name="title">Nadpis MessageBoxu</param>
        /// <param name="oldName">Původní název</param>
        /// <param name="newName">Nový název</param>
        /// <param name="p">Bod</param>
        internal static async void SendBoxMessage(CommandContext ctx, Task<DiscordChannel> chnl, string title, string oldName, string newName, Point p)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(title);
            sb.AppendLine();
            sb.Append(Emoji.GetEmoji(ctx, Emoji.RedCross));
            sb.AppendLine(" Původní název: " + oldName);
            sb.Append(Emoji.GetEmoji(ctx, Emoji.New));
            sb.AppendLine(" Nový název: " + newName);

            var embed = new DiscordEmbedBuilder
            {
                Title = Emoji.GetEmoji(ctx, p.Type) + " " + newName,
                Description = sb.ToString(),
                Color = DiscordColor.Green,
                Timestamp = DateTime.Now,
                Url = "https://www.google.cz/maps/place/" + p.Latitude.ToString().Replace(",", ".") + "," + p.Longitude.ToString().Replace(",", ".")
            };

            await chnl.Result.SendMessageAsync("", false, embed).ConfigureAwait(false);
        }

        /// <summary>
        /// MessageBox bez reakcí, který zobrazí zprávu s odkazem na mapu bodu.
        /// </summary>
        /// <param name="chnl">Kanál, kde se MessageBox zobrazí</param>
        /// <param name="title">Nadpis MessageBoxu</param>
        /// <param name="oldLat">Původní souřadnice</param>
        /// <param name="oldLong">Původní souřadnice</param>
        /// <param name="p">Nový bod</param>
        internal static async void SendBoxMessage(CommandContext ctx, Task<DiscordChannel> chnl, string title, double oldLat, double oldLong, Point p)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(title);
            sb.AppendLine();
            sb.Append(Emoji.GetEmoji(ctx, Emoji.RedCross));
            sb.AppendLine(" Původní souřadnice: " + oldLat + " " + oldLong);
            sb.Append(Emoji.GetEmoji(ctx, Emoji.New));
            sb.AppendLine(" Nové souřadnice: " + p.Latitude + " " + p.Longitude);

            var embed = new DiscordEmbedBuilder
            {
                Title = Emoji.GetEmoji(ctx, p.Type) + " " + p.Name,
                Description = sb.ToString(),
                Color = DiscordColor.Green,
                Timestamp = DateTime.Now,
                Url = "https://www.google.cz/maps/place/" + p.Latitude.ToString().Replace(",", ".") + "," + p.Longitude.ToString().Replace(",", ".")
            };

            await chnl.Result.SendMessageAsync("", false, embed).ConfigureAwait(false);
        }

        internal static Point CreatePoint(string name, PointType type, double lat, double lon, string idCell14, string idCell17, NeedCheck needCheck, DateTime dateTime)
        {
            Point point = new Point
            {
                Name = name,
                Type = type,
                Latitude = lat,
                Longitude = lon,
                IdCell14 = idCell14,
                IdCell17 = idCell17,
                NeedCheck = needCheck,
                UpdatedAt = dateTime
            };
            return point;
        }     
    }
}
