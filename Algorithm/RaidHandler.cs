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
            DiscordChannel raid_channel = null;
            Pokemon pokemon;
            DiscordEmoji emoji = DiscordEmoji.FromName((DiscordClient)client, Emoji.Warning);

            await message.DeleteAsync();

            Regex channel_patern = new Regex(@"<#\d{19}>");
            Regex emoji_patern = new Regex(@"<(:[a-z]+:)\d+>");

            Match channel_match = Regex.Match(message.Content, channel_patern.ToString(), RegexOptions.IgnoreCase);
            Match emoji_match = Regex.Match(message.Content, emoji_patern.ToString(), RegexOptions.IgnoreCase);

            if (channel_match.Success)
            {
                raid_channel = FindChannelById(client, channel_match.Value);
                //raid_channel.SendMessageAsync("Tvoje zpráva: " + message);
            }

            if (emoji_match.Success)
            {
                Regex emoji_name_patern = new Regex(@":[a-z]+:");
                Match emoji_name_match = Regex.Match(emoji_match.Value, emoji_name_patern.ToString(), RegexOptions.IgnoreCase);
                emoji = DiscordEmoji.FromName((DiscordClient)client, emoji_name_match.Value);

                await raid_channel.SendMessageAsync("Tvoje emoji: " + emoji).ConfigureAwait(false);
            }

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
