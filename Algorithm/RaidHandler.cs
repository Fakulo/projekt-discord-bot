using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBot.Models;
using System.Text.RegularExpressions;
using DiscordBot.Database;
using static DiscordBot.Models.Enums;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;

namespace DiscordBot.Algorithm
{
    public class RaidHandler
    {
        private static readonly ulong id_report_raids = 1004872574536269985;

        public RaidHandler()
        {

        }

        public async Task ProcessMessage(MessageCreateEventArgs e)
        {
            DiscordClient c = (DiscordClient)e.Client;
            var report_channel = await c.GetChannelAsync(1004872574536269985);
            if (e.Channel.Id == report_channel.Id)
            {
                await RaidReportAnalyzer(e.Message, (DiscordClient)e.Client, e.Channel, e.Author);
            }
            
        }

        public async Task ProcessReactionAdded(MessageReactionAddEventArgs e)
        {
            if (e.Channel.Id == 1004872679691669656)
            {
                await ReactionAddedAnalyzer(e.Message, (DiscordClient)e.Client, e.Channel, (DiscordMember)e.User, e.Emoji);
            }
            
        }
        public async Task ProcessReactionRemoved(MessageReactionRemoveEventArgs e)
        {
            if (e.Channel.Id == 1004872679691669656)
            {
                await ReactionRemovedAnalyzer(e.Message, (DiscordClient)e.Client, e.Channel, (DiscordMember)e.User, e.Emoji);
            }
        }


        private async Task RaidReportAnalyzer(DiscordMessage message, DiscordClient client, DiscordChannel report_channel, DiscordUser author)
        {
            //"23:05 <:pokestop:846823767565008926> Detske hriste <# 10048 72679 69166 9656>"
            DateTime time;
            int time_remaining;
            DiscordMessage discord_message = message;
            DiscordChannel raid_channel = null;
            Pokemon pokemon;
            DiscordEmoji emoji = DiscordEmoji.FromName((DiscordClient)client, Emoji.Warning);

            // Najít kanál
            var step_channel = FindChannelInText(message.Content.ToLower(), client);

            // Najít emoji
            var step_emoji = FindEmojiInText(step_channel.new_message, client);

            // Najít čas
            var step_time = FindTimeInText(step_emoji.new_message, client);

            // Najít pokemona
            var step_pokemon = FindPokemonInText(step_time.new_message, client);

            // Najít gym
            var step_point = FindPointInText(step_pokemon.new_message, client);

            // Smazat zprávu
            await message.DeleteAsync();

            // Vypsat upravenou zprávu



            StringBuilder sb = new();
            if (step_channel.status == FindStatus.ChannelFound) sb.AppendLine("Kanál: " + step_channel.channel.Name);
            if (step_emoji.status == FindStatus.EmojiFound) sb.AppendLine("Emoji: " + step_emoji.emoji);
            if (step_time.status == FindStatus.HatchTimeFound && step_time.hatch_time != TimeOnly.MinValue) sb.AppendLine("Čas zahájení: " + step_time.hatch_time.ToShortTimeString());
            if (step_time.status == FindStatus.EndTimeFound && step_time.end_time != TimeOnly.MinValue) sb.AppendLine("Čas konce: " + step_time.end_time.ToShortTimeString());
            if (step_time.status == FindStatus.EndTimeFound && step_time.end_time != TimeOnly.MinValue) sb.AppendLine("Zbývá: " + GetRemaininTime(step_time.end_time) + " minut.");
            if (step_pokemon.status == FindStatus.PokemonFound) sb.AppendLine("Pokemon: " + step_pokemon.pokemons.Keys.First().Name);
            if (step_pokemon.status == FindStatus.MultiplePokemonFound) sb.AppendLine("Pokemon!!: " + step_pokemon.pokemons.Keys.First().Name);
            if (step_point.status == FindStatus.PointFound) sb.AppendLine("Point: " + step_point.points.Keys.First().Name);
            if (step_point.status == FindStatus.MultiplePointsFound) sb.AppendLine("Point!!: " + step_point.points.Keys.First().Name);

            DiscordEmbed embed = new DiscordEmbedBuilder();
            DiscordEmbed embed_info = new DiscordEmbedBuilder();
            if (step_time.status == FindStatus.HatchTimeFound)
            {
                embed = SendBoxHandler.SendBoxReportRaid(client, author, step_point.points.Keys.First(), step_pokemon.pokemons.Keys.First(), step_channel.channel, step_time.hatch_time, step_time.status , step_emoji.emoji);
                embed_info = SendBoxHandler.SendBoxReportRaidInfo(client, author, step_point.points.Keys.First(), step_pokemon.pokemons.Keys.First(), step_channel.channel, step_time.hatch_time, step_time.status, step_emoji.emoji);
            }
            if (step_time.status == FindStatus.EndTimeFound)
            {
                embed = SendBoxHandler.SendBoxReportRaid(client, author, step_point.points.Keys.First(), step_pokemon.pokemons.Keys.First(), step_channel.channel, step_time.end_time, step_time.status, step_emoji.emoji);
                embed_info = SendBoxHandler.SendBoxReportRaidInfo(client, author, step_point.points.Keys.First(), step_pokemon.pokemons.Keys.First(), step_channel.channel, step_time.end_time, step_time.status, step_emoji.emoji);
            }


           
            


            var mes = await step_channel.channel.SendMessageAsync("", false, embed).ConfigureAwait(false);
            var mesInfo = await client.GetChannelAsync(id_report_raids).Result.SendMessageAsync("", false, embed_info).ConfigureAwait(false);
            //Console.WriteLine("Zbylo: " + step_point.new_message.ToString());

            // Vytvoření emoji
            var green_check_emoji = DiscordEmoji.FromName(client, Emoji.GreenCheck);
            var invite_emoji = DiscordEmoji.FromName(client, Emoji.Invite);
            var cross_emoji = DiscordEmoji.FromName(client, Emoji.RedCross);
            var one_emoji = DiscordEmoji.FromName(client, Emoji.One);
            var two_emoji = DiscordEmoji.FromName(client, Emoji.Two);
            var three_emoji = DiscordEmoji.FromName(client, Emoji.Three);
            var four_emoji = DiscordEmoji.FromName(client, Emoji.Four);
            var thumbs_up_emoji = DiscordEmoji.FromName(client, Emoji.ThumbsUp);

            // Vytvoření reakcí
            await mes.CreateReactionAsync(green_check_emoji);
            await mes.CreateReactionAsync(invite_emoji);
            await mes.CreateReactionAsync(one_emoji);
            await mes.CreateReactionAsync(two_emoji);
            await mes.CreateReactionAsync(three_emoji);
            await mes.CreateReactionAsync(four_emoji);
            await mes.CreateReactionAsync(cross_emoji);
            await mes.CreateReactionAsync(thumbs_up_emoji);
        }

        private (FindStatus status, string new_message, DiscordChannel channel) FindChannelInText(string message, DiscordClient client)
        {
            Regex channel_patern = new(@"<#\d{19}>");
            Match channel_match = Regex.Match(message, channel_patern.ToString(), RegexOptions.IgnoreCase);
            if (channel_match.Success)
            {
                DiscordChannel raid_channel = FindChannelById((DiscordClient)client, channel_match.Value);
                string new_message = message.Replace(channel_match.ToString(), "").Replace("  ", " ").Trim();

                return (FindStatus.ChannelFound, new_message, raid_channel);
            }
            return(FindStatus.NotFound, message, null);
        }

        private (FindStatus status, string new_message, DiscordEmoji emoji) FindEmojiInText(string message, DiscordClient client)
        {
            Regex emoji_patern = new(@"<(:[a-z]+:)\d+>");
            Match emoji_match = Regex.Match(message, emoji_patern.ToString(), RegexOptions.IgnoreCase);
            if (emoji_match.Success)
            {
                Regex emoji_name_patern = new(@":[a-z]+:");
                Match emoji_name_match = Regex.Match(emoji_match.Value, emoji_name_patern.ToString(), RegexOptions.IgnoreCase);
                DiscordEmoji emoji = DiscordEmoji.FromName((DiscordClient)client, emoji_name_match.Value);
                string new_message = message.Replace(emoji_match.ToString(), "").Replace("  ", " ").Trim();

                return (FindStatus.EmojiFound, new_message, emoji);                
            }

            emoji_patern = new(@"(\u00a9|\u00ae|[\u2000-\u3300]|\ud83c[\ud000-\udfff]|\ud83d[\ud000-\udfff]|\ud83e[\ud000-\udfff])");
            emoji_match = Regex.Match(message.ToString(), emoji_patern.ToString(), RegexOptions.IgnoreCase);
            if (emoji_match.Success)
            {                
                //Match emoji_name_match = Regex.Match(emoji_match.Value, emoji_match.ToString(), RegexOptions.IgnoreCase);
                DiscordEmoji emoji = DiscordEmoji.FromUnicode((DiscordClient)client, emoji_match.Value);
                string new_message = message.Replace(emoji_match.ToString(), "").Replace("  ", " ").Trim();

                return (FindStatus.EmojiFound, new_message, emoji);
            }
            return (FindStatus.NotFound, message, null);
        }
        
        private (FindStatus status, string new_message, TimeOnly hatch_time, TimeOnly end_time) FindTimeInText(string message, DiscordClient client)
        {
            Regex time_patern = new(@"[\d]{1,2}:[\d]{2}");
            Match time_match = Regex.Match(message, time_patern.ToString(), RegexOptions.IgnoreCase);
            if (time_match.Success)
            {
                TimeOnly hatch_time = TimeOnly.Parse(time_match.Value); 
                string new_message = message.Replace(time_match.ToString(), "").Replace("  ", " ").Trim();
                // TODO: Přidat možnost upravovat čas délky raidu
                TimeOnly end_time_calculated = hatch_time.AddMinutes(45);
                TimeOnly time_now = new(DateTime.Now.Hour, DateTime.Now.Minute);
                if (hatch_time < time_now)
                {
                    return (FindStatus.EndTimeFound, new_message, hatch_time, end_time_calculated);
                }
                else
                {
                    return (FindStatus.HatchTimeFound, new_message, hatch_time, end_time_calculated);
                }
                
            }
            time_patern = new(@"[\d]{1,2}[\s]{0,1}min[\S]{0,5}");
            time_match = Regex.Match(message, time_patern.ToString(), RegexOptions.IgnoreCase);
            if (time_match.Success)
            {
                Regex before_time_patern = new(@"[A-Za-z]{0,1}(eště|este|estě|ešte)", RegexOptions.IgnoreCase);
                string new_message = message.Replace(time_match.ToString(), "").Replace("  ", " ").Trim();
                new_message = Regex.Replace(new_message, before_time_patern.ToString(), "", RegexOptions.IgnoreCase);
                Regex time_number_patern = new(@"[\d]{1,2}");
                Match time_number_match = Regex.Match(time_match.Value, time_number_patern.ToString(), RegexOptions.IgnoreCase);


                TimeOnly end_time = new(DateTime.Now.Hour, DateTime.Now.Minute);
                int hours = int.Parse(time_number_match.Value) / 60;
                int minutes = int.Parse(time_number_match.Value) % 60;
                end_time = end_time.AddHours(hours).AddMinutes(minutes);


                return (FindStatus.EndTimeFound, new_message, TimeOnly.MinValue, end_time);
            }
            return (FindStatus.NotFound, message, TimeOnly.MinValue, TimeOnly.MinValue);
        }

        private (FindStatus status, string new_message, Dictionary<Pokemon, int> pokemons) FindPokemonInText(string message, DiscordClient client)
        {
            // TODO: Odchytávat např. slovo MEGA (HAT/EVENT), aby se daly odlišit kategorie
            using var context = new PogoContext();
            string[] messages = message.ToLower().Trim().Split(" ");
            Dictionary<Pokemon, int> pokemons = new();
            foreach (var word in messages)
            {
                List<Pokemon> temp_pokemons = context.Pokemons.Where(p => p.Name.ToLower().Contains(word)).ToList();
                foreach (var point in temp_pokemons)
                {
                    if (pokemons.ContainsKey(point))
                    {
                        pokemons[point] += 1;
                    }
                    else
                    {
                        pokemons.Add(point, 1);
                    }
                }
            }
            if (pokemons.Count != 0)
            {
                pokemons = pokemons.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                string new_message = message.Replace(pokemons.Keys.First().Name.ToLower(), "").Replace("  ", " ").Trim();
                if (pokemons.Count == 1)
                { 
                    return (FindStatus.PokemonFound, new_message, pokemons);
                }
                else
                {
                    return (FindStatus.MultiplePokemonFound, new_message, pokemons);
                }
            }
            return (FindStatus.NotFound, message, pokemons);
        }

        private (FindStatus status, string new_message, Dictionary<Point, int> points) FindPointInText(string message, DiscordClient client)
        {
            using var context = new PogoContext();
            Regex pattern = new(@"^.(.*)$");
            string edited_message = message.ToLower().Replace(" - ", " ").Trim();
            if (edited_message.StartsWith("-")) { edited_message = edited_message.Remove(0,1);}

            if (edited_message.EndsWith("-")) { edited_message = edited_message.Remove(edited_message.Length - 1); }
            string[] messages = edited_message.ToLower().Replace(" - ", " ").Trim().Split(" ");
            Dictionary<Point,int> points = new();
            foreach (var word in messages)
            {
                List<Point> temp_points = context.Points.Where(p => p.Name.ToLower().Contains(word)).ToList();
                foreach (var point in temp_points)
                {
                    if (points.ContainsKey(point))
                    {
                        points[point] += 1;
                    }
                    else
                    {
                        points.Add(point, 1);
                    }
                }                
            }
            if (points.Count != 0)
            {
                points = points.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                string new_message = message.Replace(points.Keys.First().Name.ToLower(), "").Replace("  ", " ").Trim();
                if (points.Count == 1)
                {
                    return (FindStatus.PointFound, new_message, points);
                }
                else
                {
                    return (FindStatus.MultiplePointsFound, new_message, points);
                }                
            }
            return (FindStatus.NotFound, message, points);
        }

        private int GetRemaininTime(TimeOnly end_time)
        {
            int remain_minutes = (end_time.Hour - DateTime.Now.Hour) * 60 + (end_time.Minute - DateTime.Now.Minute);
            if(remain_minutes < 0) { remain_minutes = 0; }
            return remain_minutes;
        }

        private async Task ReactionAddedAnalyzer(DiscordMessage message, DiscordClient client, DiscordChannel channel, DiscordMember user, DiscordEmoji emoji)
        {
            var guild = message.Channel.Guild;
            message = await channel.GetMessageAsync(message.Id);
            var embed = message.Embeds[0];
            DiscordEmbedBuilder new_embed = emoji.Name switch
            {
                "✅" => SendBoxHandler.EditSendBoxReportRaid(message, client, user, embed, RaidJoinStatus.Join, channel),
                "📩" => SendBoxHandler.EditSendBoxReportRaid(message, client, user, embed, RaidJoinStatus.Invite, channel),
                "❌" => SendBoxHandler.EditSendBoxReportRaid(message, client, user, embed, RaidJoinStatus.Leave, channel),
                "1⃣" => SendBoxHandler.EditSendBoxReportRaid(message, client, user, embed, RaidJoinStatus.Plus1, channel),
                "2⃣" => SendBoxHandler.EditSendBoxReportRaid(message, client, user, embed, RaidJoinStatus.Plus2, channel),
                "3⃣" => SendBoxHandler.EditSendBoxReportRaid(message, client, user, embed, RaidJoinStatus.Plus3, channel),
                "4⃣" => SendBoxHandler.EditSendBoxReportRaid(message, client, user, embed, RaidJoinStatus.Plus4, channel),
                "👍" => SendBoxHandler.EditSendBoxReportRaid(message, client, user, embed, RaidJoinStatus.Meet, channel),
                _ => SendBoxHandler.EditSendBoxReportRaid(message, client, user, embed, RaidJoinStatus.Leave, channel),
            };
            await message.ModifyAsync("", new_embed);
        }
        private async Task ReactionRemovedAnalyzer(DiscordMessage message, DiscordClient client, DiscordChannel channel, DiscordMember user, DiscordEmoji emoji)
        {
            var guild = message.Channel.Guild;
            var reactionName = emoji.GetDiscordName();
            message = await channel.GetMessageAsync(message.Id);
            var embed = message.Embeds[0];
            DiscordEmbedBuilder new_embed = emoji.Name switch
            {
                "1⃣" => SendBoxHandler.EditSendBoxReportRaid(message, client, user, embed, RaidJoinStatus.Minus1, channel),
                "2⃣" => SendBoxHandler.EditSendBoxReportRaid(message, client, user, embed, RaidJoinStatus.Minus2, channel),
                "3⃣" => SendBoxHandler.EditSendBoxReportRaid(message, client, user, embed, RaidJoinStatus.Minus3, channel),
                "4⃣" => SendBoxHandler.EditSendBoxReportRaid(message, client, user, embed, RaidJoinStatus.Minus4, channel),
                _ => SendBoxHandler.EditSendBoxReportRaid(message, client, user, embed, RaidJoinStatus.Leave, channel),
            };
            await message.ModifyAsync("", new_embed);
        }

        private DiscordChannel FindChannelById(BaseDiscordClient client, string id)
        {
            List<DiscordGuild> guilds = (List<DiscordGuild>)client.Guilds.Values.ToList();
            List<DiscordChannel> channels;

            for (int i = 0; i < guilds.Count; i++)
            {
                int numchannels = guilds[i].Channels.Count;
                channels = new List<DiscordChannel>(guilds[i].Channels.ToArray());

                for (int j = 0; j < numchannels; j++)
                {
                    if (channels[j].Mention == id)
                    {
                        return channels[j];
                    }
                }                                
            }
            return null;
        }
    }
}
