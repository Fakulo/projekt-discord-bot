using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Dialogue.Steps
{
    public abstract class DialogueStepBase : IDialogueStep
    {
        protected readonly string _contetnt;

        public DialogueStepBase(string content)
        {
            _contetnt = content;
        }

        public Action<DiscordMessage> OnMessageAdded { get; set; } = delegate { };

        public abstract IDialogueStep NextStep { get; }

        public abstract Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user);

        protected async Task TryAgain(DiscordChannel channel, string problem)
        {
            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = "Prosím, zkuste to znovu.",
                Color = DiscordColor.Black,
            };

            embedBuilder.AddField("Nastal problém při zadávání.", problem);

            var embed = await channel.SendMessageAsync(embed: embedBuilder).ConfigureAwait(false);

            OnMessageAdded(embed);
        }
    }
}
