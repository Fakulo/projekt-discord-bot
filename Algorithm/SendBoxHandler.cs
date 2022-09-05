using DiscordBot.Database;
using DiscordBot.Models;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static DiscordBot.Models.Enums;
using static DSharpPlus.Entities.DiscordEmbedBuilder;

namespace DiscordBot.Algorithm
{
    class SendBoxHandler
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
        /// MessageBox bez reakcí, který zobrazí zprávu.
        /// </summary>
        /// <param name="chnl">Kanál, kde se MessageBox zobrazí</param>
        /// <param name="title">Nadpis MessageBoxu</param>
        /// <param name="description">Text v MessageBoxu</param>
        /// <param name="color">Barva MessageBoxu</param>
        internal static async void SendBoxMessage(DiscordChannel channel, string title, string description, DiscordColor color)
        {
            var embed = new DiscordEmbedBuilder
            {
                Title = title,
                Description = description,
                Color = color,
                Timestamp = DateTime.Now
            };
            await channel.SendMessageAsync("", false, embed).ConfigureAwait(false);
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

        /*internal static DiscordEmbedBuilder SendBoxReportRaid(BaseDiscordClient client, DiscordUser user, DiscordEmoji emoji, Point point, Pokemon pokemon, DiscordChannel channel, TimeOnly hatch_time)
        {
            return SendBoxReportRaid(client, user, emoji, point, pokemon, channel, hatch_time);
        }*/

        internal static DiscordEmbedBuilder SendBoxReportRaid(BaseDiscordClient client, DiscordUser user, Point point, Pokemon pokemon, DiscordChannel channel, TimeOnly time, FindStatus status_time, DiscordEmoji? emoji)
        {   
            if(emoji is null)
            {
                emoji = Emoji.GetEmoji((DiscordClient)client, Emoji.Question);
            }
            string text_time = string.Empty;
            if (status_time == FindStatus.HatchTimeFound) text_time = "Čas líhnutí: " + time.ToShortTimeString();            
            if (status_time == FindStatus.EndTimeFound) text_time = "Zbývá: " + GetRemaininTime(time).ToString() + " minut. Konec " + time.ToShortTimeString();

            var embed = new DiscordEmbedBuilder
            {
                Title = emoji + " " + pokemon.Name,
                Description = Emoji.GetEmoji((DiscordClient)client, point.Type) + " " + point.Name,
                Color = DiscordColor.Green,
                ThumbnailUrl = new(pokemon.ImageUrl),
                Author = new EmbedAuthor
                {
                    Name = text_time,
                    Url = pokemon.ImageUrl
                },
                Footer = new EmbedFooter
                {
                    Text = "Raid nahlásil " + user.Username
                },                
                Timestamp = DateTime.Now,
                Url = "https://www.google.cz/maps/place/" + point.Latitude.ToString().Replace(",", ".") + "," + point.Longitude.ToString().Replace(",", ".")
            }
            .AddField("Kanál pro komunikaci:",channel.Mention, true)
            .AddField("Počet hráčů:","0", true)
            .AddField("Chce se účastnit: " + DiscordEmoji.FromName((DiscordClient)client, Emoji.GreenCheck), "Zatím nikdo")
            .AddField("Chce pozvat: " + DiscordEmoji.FromName((DiscordClient)client, Emoji.Invite), "Zatím nikdo");

            return embed;
        }

        internal static DiscordEmbedBuilder SendBoxReportRaidInfo(BaseDiscordClient client, DiscordUser user, Point point, Pokemon pokemon, DiscordChannel channel, TimeOnly time, FindStatus status_time, DiscordEmoji? emoji = null)
        {
            if (emoji is null)
            {
                emoji = Emoji.GetEmoji((DiscordClient)client, Emoji.Question);
            }

            string text_time = string.Empty;
            if (status_time == FindStatus.HatchTimeFound) text_time = "Čas líhnutí: " + time.ToShortTimeString();
            if (status_time == FindStatus.EndTimeFound) text_time = "Zbývá: " + GetRemaininTime(time).ToString() + " minut. Konec " + time.ToShortTimeString();

            var embed = new DiscordEmbedBuilder
            {
                Title = emoji + " " + pokemon.Name,
                Description = Emoji.GetEmoji((DiscordClient)client, point.Type) + " " + point.Name,
                Color = DiscordColor.Green,
                ThumbnailUrl = new(pokemon.ImageUrl),
                Author = new EmbedAuthor
                {
                    Name = text_time,
                    Url = pokemon.ImageUrl
                },
                Footer = new EmbedFooter
                {
                    Text = "Raid nahlásil " + user.Username
                },
                Timestamp = DateTime.Now,
                Url = "https://www.google.cz/maps/place/" + point.Latitude.ToString().Replace(",", ".") + "," + point.Longitude.ToString().Replace(",", ".")
            }
            .AddField("Kanál pro komunikaci:", channel.Mention, true);
            return embed;
        }

        internal static DiscordEmbedBuilder EditSendBoxReportRaid(DiscordMessage message, BaseDiscordClient client, DiscordUser user, DiscordEmbed embed, RaidJoinStatus join_status, DiscordChannel? channel = null)
        {            
            DiscordEmbedField field_channel = embed.Fields[0];
            DiscordEmbedField field_users_count = embed.Fields[1];
            DiscordEmbedField field_users_raid = embed.Fields[2];
            DiscordEmbedField field_users_invite = embed.Fields[3];
            string result_raid = field_users_raid.Value;
            string result_invite = field_users_invite.Value;

            if (join_status == RaidJoinStatus.Join)
            {
                if (field_users_raid.Value.Contains("Zatím nikdo"))
                {
                    field_users_raid.Value = field_users_raid.Value.Replace("Zatím nikdo", "").Replace("\n\n", "\n");
                    result_raid = field_users_raid.Value;
                }
                if (!field_users_invite.Value.Contains(user.Mention))
                {
                    if (!field_users_raid.Value.Contains(user.Mention))
                    {                        
                        result_raid = field_users_raid.Value + "\n" + user.Mention;
                        field_users_count.Value = (int.Parse(field_users_count.Value) + 1).ToString();
                    }                    
                }
                else
                {
                    result_invite = field_users_invite.Value.Replace(user.Mention, "").Replace("\n\n", "\n");
                    result_raid = field_users_raid.Value + "\n" + user.Mention;
                }
            }

            if (join_status == RaidJoinStatus.Invite)
            {
                if (field_users_invite.Value.Contains("Zatím nikdo"))
                {
                    field_users_invite.Value = field_users_invite.Value.Replace("Zatím nikdo", "").Replace("\n\n", "\n");
                    result_invite = field_users_invite.Value;
                }
                if (!field_users_raid.Value.Contains(user.Mention))
                {
                    if (!field_users_invite.Value.Contains(user.Mention))
                    {                        
                        result_invite = field_users_invite.Value + "\n" + user.Mention;
                        field_users_count.Value = (int.Parse(field_users_count.Value) + 1).ToString();
                    }                    
                }else
                {                    
                    result_raid = field_users_raid.Value.Replace(user.Mention, "").Replace("\n\n", "\n");
                    result_invite = field_users_invite.Value + "\n" + user.Mention;
                }   
            }

            if (join_status == RaidJoinStatus.Leave)
            {
                if (field_users_raid.Value.Contains(user.Mention))
                {
                    result_raid = field_users_raid.Value.Replace(user.Mention, "").Replace("\n\n", "\n");
                    field_users_count.Value = (int.Parse(field_users_count.Value) - 1).ToString();
                }
                if (field_users_invite.Value.Contains(user.Mention))
                {
                    result_invite = field_users_invite.Value.Replace(user.Mention, "").Replace("\n\n", "\n");
                    field_users_count.Value = (int.Parse(field_users_count.Value) - 1).ToString();
                }
                message.DeleteReactionAsync(DiscordEmoji.FromName((DiscordClient)client, Emoji.GreenCheck), user);
                message.DeleteReactionAsync(DiscordEmoji.FromName((DiscordClient)client, Emoji.Invite), user);
                message.DeleteReactionAsync(DiscordEmoji.FromName((DiscordClient)client, Emoji.One), user);
                message.DeleteReactionAsync(DiscordEmoji.FromName((DiscordClient)client, Emoji.Two), user);
                message.DeleteReactionAsync(DiscordEmoji.FromName((DiscordClient)client, Emoji.Three), user);
                message.DeleteReactionAsync(DiscordEmoji.FromName((DiscordClient)client, Emoji.Four), user);
            }

            switch (join_status)
            {                
                case RaidJoinStatus.Plus1:
                    field_users_count.Value = (int.Parse(field_users_count.Value) + 1).ToString();
                    break;
                case RaidJoinStatus.Plus2:
                    field_users_count.Value = (int.Parse(field_users_count.Value) + 2).ToString();
                    break;
                case RaidJoinStatus.Plus3:
                    field_users_count.Value = (int.Parse(field_users_count.Value) + 3).ToString();
                    break;
                case RaidJoinStatus.Plus4:
                    field_users_count.Value = (int.Parse(field_users_count.Value) + 4).ToString();
                    break;
                case RaidJoinStatus.Minus1:
                    field_users_count.Value = (int.Parse(field_users_count.Value) - 1).ToString();
                    break;
                case RaidJoinStatus.Minus2:
                    field_users_count.Value = (int.Parse(field_users_count.Value) - 2).ToString();
                    break;
                case RaidJoinStatus.Minus3:
                    field_users_count.Value = (int.Parse(field_users_count.Value) - 3).ToString();
                    break;
                case RaidJoinStatus.Minus4:
                    field_users_count.Value = (int.Parse(field_users_count.Value) - 4).ToString();
                    break;
                case RaidJoinStatus.Meet:
                    SetMeetTime(message, (DiscordClient)client, user, embed, channel);
                    break;
                default:
                    break;
            }

            var new_embed = new DiscordEmbedBuilder
            {
                Title = embed.Title,
                Description = embed.Description,
                Color = embed.Color,
                ThumbnailUrl = embed.Thumbnail.Url.ToString(),
                Author = new EmbedAuthor
                {
                    Name = embed.Author.Name,
                    Url = embed.Author.Url.ToString()
                },
                Footer = new EmbedFooter
                {
                    Text = embed.Footer.Text.ToString()
                },
                Timestamp = embed.Timestamp,
                Url = embed.Url.ToString()
            }
            .AddField(field_channel.Name, field_channel.Value,field_channel.Inline)
            .AddField(field_users_count.Name, String.IsNullOrEmpty(result_raid) && String.IsNullOrEmpty(result_invite) ? "0" : field_users_count.Value.ToString(), field_users_count.Inline)
            .AddField(field_users_raid.Name, String.IsNullOrEmpty(result_raid) ? "Zatím nikdo" : result_raid, field_users_raid.Inline)
            .AddField(field_users_invite.Name, String.IsNullOrEmpty(result_invite) ? "Zatím nikdo" : result_invite, field_users_invite.Inline);

            return new_embed;
        }

        private static async void SetMeetTime(DiscordMessage message, DiscordClient client, DiscordUser user, DiscordEmbed embed, DiscordChannel? channel = null)
        {
            var mmeess = await channel.SendMessageAsync("Napiš čas srazu:").ConfigureAwait(false);
            var interactivity = InteractivityExtension.GetInteractivityModule(client);
            var meetResult = await interactivity.WaitForMessageAsync(x => x.Channel.Id == channel.Id && x.Author.Id == user.Id, TimeSpan.FromSeconds(30));
            Regex time_patern = new(@"[\d]{1,2}:[\d]{2}");

            if (meetResult == null)
            {
                await mmeess.DeleteAsync();
                await channel.SendMessageAsync("Sraz NENASTAVEN").ConfigureAwait(false);
            }            
            else if (meetResult.Message.Content.Length > 0)
            {
                Match time_match = Regex.Match(meetResult.Message.Content, time_patern.ToString(), RegexOptions.IgnoreCase);
                if (time_match.Success)
                {
                    await mmeess.DeleteAsync();
                    //await meetResult.Message.DeleteAsync();
                    var meet_channel = await client.GetChannelAsync(1004872638746857502);
                    var new_embed = new DiscordEmbedBuilder
                    {
                        Title = embed.Title,
                        Description = embed.Description,
                        Color = DiscordColor.Azure,
                        ThumbnailUrl = embed.Thumbnail.Url.ToString(),
                        Author = new EmbedAuthor
                        {
                            Name = "SRAZ V " + meetResult.Message.Content,
                            Url = embed.Author.Url.ToString()
                        },
                        Footer = new EmbedFooter
                        {
                            Text = "Sraz nahlásil " + user.Username
                        },
                        Timestamp = DateTime.Now,
                        Url = embed.Url.ToString()
                    };
                    await meet_channel.SendMessageAsync("", false, new_embed).ConfigureAwait(false);
                }
                else
                {
                    await channel.SendMessageAsync("Čas musí mít formát hh:mm.").ConfigureAwait(false);
                }
            }
        }

        internal static DiscordEmbedBuilder SendBoxReportRaid(BaseDiscordClient client, DiscordUser user, DiscordEmoji emoji, Point point, Pokemon pokemon, DiscordChannel channel, TimeOnly end_time, int remaining_time)
        {
            if (emoji == null)
            {
                emoji = Emoji.GetEmoji((DiscordClient)client, Emoji.Question);
            }
            var embed = new DiscordEmbedBuilder
            {
                Title = emoji + " " + pokemon.Name,
                Description = Emoji.GetEmoji((DiscordClient)client, point.Type) + " " + point.Name,
                Color = DiscordColor.Green,
                ThumbnailUrl = new(pokemon.ImageUrl),
                Author = new EmbedAuthor
                {
                    Name = "Raid nahlásil: " + user.Username
                },
                Timestamp = DateTime.Now,
                Url = "https://www.google.cz/maps/place/" + point.Latitude.ToString().Replace(",", ".") + "," + point.Longitude.ToString().Replace(",", ".")
            };
            return embed;
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
        private static int GetRemaininTime(TimeOnly end_time)
        {
            int remain_minutes = (end_time.Hour - DateTime.Now.Hour) * 60 + (end_time.Minute - DateTime.Now.Minute);
            if (remain_minutes < 0) { remain_minutes = 0; }
            return remain_minutes;
        }
    }
}
