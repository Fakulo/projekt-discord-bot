using DiscordBot.Algorithm;
using DiscordBot.Database;
using DiscordBot.Models;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Dialogue.Steps
{
    public class ReactionStep : DialogueStepBase
    {
        private readonly Dictionary<DiscordEmoji, ReactionStepData> _options;

        private DiscordEmoji _selectedEmoji;

        public ReactionStep(string content, Dictionary<DiscordEmoji, ReactionStepData> options) : base(content)
        {
            _options = options;
        }

        public override IDialogueStep NextStep => _options[_selectedEmoji].NextStep;

        public Action<DiscordEmoji> OnValidResult { get; set; } = delegate { };

        public override async Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user)
        {
            var cancelEmoji = DiscordEmoji.FromName(client, Emoji.RedCross);
            var greenCheckEmoji = DiscordEmoji.FromName(client, Emoji.GreenCheck);
            var arrowChangeEmoji = DiscordEmoji.FromName(client, Emoji.ArrowChange);

            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = "Reaguj prosím.",
                Description = _contetnt,
                Color = DiscordColor.Black,
            };

            embedBuilder.AddField("Pro ukončení:", "dej :x:");

            var interactivity = client.GetInteractivityModule();

            while (true)
            {
                var embed = await channel.SendMessageAsync(embed: embedBuilder).ConfigureAwait(false);

                OnMessageAdded(embed);

                foreach(var emoji in _options.Keys)
                {
                    await embed.CreateReactionAsync(emoji).ConfigureAwait(false);
                }

                await embed.CreateReactionAsync(cancelEmoji).ConfigureAwait(false);

                var reactionResult = await interactivity.WaitForReactionAsync(
                    x => _options.ContainsKey(x) || x == cancelEmoji,
                    user,
                    TimeSpan.FromSeconds(60)).ConfigureAwait(false);

                

                _selectedEmoji = reactionResult.Emoji;

                OnValidResult(_selectedEmoji);

                if (reactionResult.Emoji == greenCheckEmoji)
                {
                    return false;

                }

                if (reactionResult.Emoji == arrowChangeEmoji)
                {
                    return false;
                }


                if (reactionResult.Emoji == cancelEmoji)
                {
                    return false;
                }

                return true;
            }
        }
    }

    public class ReactionStepData
    {
        public IDialogueStep NextStep { get; set; }

        public string Content { get; set; }
    }
}
