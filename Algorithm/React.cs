using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Algorithm
{
    class React
    {
        private readonly CommandContext ctx;
        private readonly Task<DiscordChannel> chnl;
        public React(CommandContext ctx, Task<DiscordChannel> chnl)
        {
            this.ctx = ctx;
            this.chnl = chnl;
        }       

        internal async void SendBoxMessageReact(string title, string description, DiscordColor color, List<DiscordEmoji> emojis, double latitude, double longitude)
        {
            var interactivity = ctx.Client.GetInteractivityModule();
            var embed = new DiscordEmbedBuilder
            {
                Title = title,
                Description = description,
                Color = color,
                Timestamp = DateTime.Now,
                Url = "https://www.google.cz/maps/place/" + latitude.ToString().Replace(",", ".") + "," + longitude.ToString().Replace(",", ".")

            };

            var msg = await chnl.Result.SendMessageAsync("", false, embed).ConfigureAwait(false);
            foreach (var emoji in emojis)
            {
               await msg.CreateReactionAsync(emoji);
            }

            
            var reactionResult = await interactivity.WaitForReactionAsync(
                 x => x == emojis[2] && ctx.Message == msg, ctx.User, TimeSpan.FromSeconds(30)).ConfigureAwait(false);

            if (reactionResult.Emoji == emojis[2])
            {
               await chnl.Result.SendMessageAsync(latitude + " - " + longitude).ConfigureAwait(false);
               await msg.DeleteAsync();
            }
            
        }
    }
}
