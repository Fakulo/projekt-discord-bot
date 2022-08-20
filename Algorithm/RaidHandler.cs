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

namespace DiscordBot.Algorithm
{
    public class RaidHandler
    {
        public RaidHandler()
        {

        }

        public async Task ProcessMessage(MessageCreateEventArgs e)
        {
            await RaidReportAnalyzer(e.Message, e.Client, e.Channel);

        }

        private async Task RaidReportAnalyzer(DiscordMessage message, BaseDiscordClient client, DiscordChannel report_channel)
        {
            //"23:05 <:pokestop:846823767565008926> Detske hriste <# 10048 72679 69166 9656>"
            DateTime time;
            int time_remaining;
            DiscordMessage discord_message = message;
            DiscordChannel raid_channel = null;
            Pokemon pokemon;
            DiscordEmoji emoji = DiscordEmoji.FromName((DiscordClient)client, Emoji.Warning);

            await message.DeleteAsync();

            // Najít kanál
            (bool is_found, string new_message, DiscordChannel channel) step_channel = FindChannelInText(message.Content, client);

            // Najít emoji
            (bool is_found, string new_message, DiscordEmoji emoji) step_emoji = FindEmojiInText(step_channel.new_message, client);

            // Najít čas
            (bool is_found, string new_message, TimeOnly hatch_time, TimeOnly end_time) step_time = FindTimeInText(step_emoji.new_message, client);

            // Najít pokemona
            (bool is_found, string new_message, Pokemon pokemon) step_pokemon = FindPokemonInText(step_time.new_message, client);

            // Najít gym
            (bool is_found, string new_message, Point point) step_point = FindPointInText(step_pokemon.new_message,client);

            StringBuilder sb = new();
            if (step_channel.is_found) sb.AppendLine("Kanál: " + step_channel.channel.Name);
            if (step_emoji.is_found) sb.AppendLine("Emoji: " + step_emoji.emoji);
            if (step_time.is_found && step_time.hatch_time != TimeOnly.MinValue) sb.AppendLine("Čas zahájení: " + step_time.hatch_time.ToShortTimeString());
            if (step_time.is_found && step_time.end_time != TimeOnly.MinValue) sb.AppendLine("Čas konce: " + step_time.end_time.ToShortTimeString());
            if (step_time.is_found && step_time.end_time != TimeOnly.MinValue)
            {                
                int remain_minutes = (step_time.end_time.Hour-DateTime.Now.Hour)*60 + (step_time.end_time.Minute-DateTime.Now.Minute);
                sb.AppendLine("Zbývá: " + remain_minutes + " minut.");
            }
            if (step_pokemon.is_found) sb.AppendLine("Pokemon: " + step_pokemon.pokemon.Name);
            if (step_point.is_found) sb.AppendLine("Point: " + step_point.point.Name);

            await step_channel.channel.SendMessageAsync(sb.ToString()).ConfigureAwait(false);
            await step_channel.channel.SendMessageAsync("Zbylo: " + step_time.new_message.ToString()).ConfigureAwait(false);


        }

        private (bool is_found, string new_message, DiscordChannel channel) FindChannelInText(string message, BaseDiscordClient client)
        {
            Regex channel_patern = new(@"<#\d{19}>");
            Match channel_match = Regex.Match(message, channel_patern.ToString(), RegexOptions.IgnoreCase);
            if (channel_match.Success)
            {
                DiscordChannel raid_channel = FindChannelById((DiscordClient)client, channel_match.Value);
                string new_message = message.Replace(channel_match.ToString(), "").Replace("  ", " ");

                return (true, new_message, raid_channel);
            }
            return(false, message, null);
        }

        private (bool is_found, string new_message, DiscordEmoji emoji) FindEmojiInText(string message, BaseDiscordClient client)
        {
            Regex emoji_patern = new(@"<(:[a-z]+:)\d+>");
            Match emoji_match = Regex.Match(message, emoji_patern.ToString(), RegexOptions.IgnoreCase);
            if (emoji_match.Success)
            {
                Regex emoji_name_patern = new(@":[a-z]+:");
                Match emoji_name_match = Regex.Match(emoji_match.Value, emoji_name_patern.ToString(), RegexOptions.IgnoreCase);
                DiscordEmoji emoji = DiscordEmoji.FromName((DiscordClient)client, emoji_name_match.Value);
                string new_message = message.Replace(emoji_match.ToString(), "").Replace("  ", " ");

                return (true, new_message, emoji);                
            }

            emoji_patern = new(@"(\u00a9|\u00ae|[\u2000-\u3300]|\ud83c[\ud000-\udfff]|\ud83d[\ud000-\udfff]|\ud83e[\ud000-\udfff])");
            emoji_match = Regex.Match(message.ToString(), emoji_patern.ToString(), RegexOptions.IgnoreCase);
            if (emoji_match.Success)
            {                
                //Match emoji_name_match = Regex.Match(emoji_match.Value, emoji_match.ToString(), RegexOptions.IgnoreCase);
                DiscordEmoji emoji = DiscordEmoji.FromUnicode((DiscordClient)client, emoji_match.Value);
                string new_message = message.Replace(emoji_match.ToString(), "").Replace("  ", " ");

                return (true, new_message, emoji);
            }
            return (false, message, null);
        }

        private (bool is_found, string new_message, TimeOnly hatch_time, TimeOnly end_time) FindTimeInText(string message, BaseDiscordClient client)
        {
            Regex time_patern = new(@"[\d]{1,2}:[\d]{2}");
            Match time_match = Regex.Match(message, time_patern.ToString(), RegexOptions.IgnoreCase);
            if (time_match.Success)
            {
                TimeOnly hatch_time = TimeOnly.Parse(time_match.Value); 
                string new_message = message.Replace(time_match.ToString(), "").Replace("  ", " ");

                return (true, new_message, hatch_time,TimeOnly.MinValue);
            }
            time_patern = new(@"[\d]{1,2}[\s]{0,1}min[\S]{0,5}");
            time_match = Regex.Match(message, time_patern.ToString(), RegexOptions.IgnoreCase);
            if (time_match.Success)
            {
                Regex before_time_patern = new(@"[A-Za-z]{0,1}(eště|este|estě|ešte)");
                string new_message = message.Replace(time_match.ToString(), "").Replace("  ", " ");
                new_message = Regex.Replace(new_message, before_time_patern.ToString(), "", RegexOptions.IgnoreCase);
                Regex time_number_patern = new(@"[\d]{1,2}");
                Match time_number_match = Regex.Match(time_match.Value, time_number_patern.ToString(), RegexOptions.IgnoreCase);


                TimeOnly end_time = new(DateTime.Now.Hour, DateTime.Now.Minute);
                int hours = int.Parse(time_number_match.Value) / 60;
                int minutes = int.Parse(time_number_match.Value) % 60;
                end_time = end_time.AddHours(hours).AddMinutes(minutes);


                return (true, new_message, TimeOnly.MinValue, end_time);
            }
            return (true, message, TimeOnly.MinValue, TimeOnly.MinValue);
        }

        private (bool is_found, string new_message, Pokemon pokemon) FindPokemonInText(string message, BaseDiscordClient client)
        {
            using var context = new PogoContext();
            string[] messages = message.ToLower().Trim().Split(" ");
            foreach (var word in messages)
            {
                Pokemon pokemon = context.Pokemons.Where(p => p.Name.ToLower().Contains(word)).FirstOrDefault();
                if (pokemon != null)
                {

                    return (true, message, pokemon);
                }
            }   
            return (false, message, null);
        }

        private (bool is_found, string new_message, Point point) FindPointInText(string message, BaseDiscordClient client)
        {
            using var context = new PogoContext();
            string[] messages = message.ToLower().Trim().Split(" ");
            foreach (var word in messages)
            {
                Point point = context.Points.Where(p => p.Name.ToLower().Contains(word)).FirstOrDefault();
                if (point != null)
                {

                    return (true, message, point);
                }
            }
            return (false, message, null);
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
