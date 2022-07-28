using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Dialogue.Steps
{
    class TextStep : DialogueStepBase
    {
        private IDialogueStep _nextStep;
        private readonly int? _minLenght;
        private readonly int? _maxLenght;

        public TextStep(string content, IDialogueStep nextStep, int? minLenght = null, int? maxLenght = null) : base(content)
        {
            _nextStep = nextStep;
            _minLenght = minLenght;
            _maxLenght = maxLenght;
        }

        public Action<string> OnValidResult { get; set; } = delegate { };

        public override IDialogueStep NextStep => _nextStep;

        public void SetNextStep(IDialogueStep nextStep)
        {
            _nextStep = nextStep;
        }
        public override async Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user)
        {
            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = "Nadpis",
                Description = _contetnt,
            };
            embedBuilder.AddField("Popisek textu","hodnota");

            var interactivity = client.GetInteractivityModule();

            while (true)
            {
                var embed = await channel.SendMessageAsync(embed: embedBuilder).ConfigureAwait(false);

                OnMessageAdded(embed);

                var messeageResult = await interactivity.WaitForMessageAsync(
                    x => x.Channel.Id == channel.Id && x.Author.Id == user.Id).ConfigureAwait(false);

                OnMessageAdded(messeageResult.Message);

                if(messeageResult.Message.Content.Equals("?cancel", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                OnValidResult(messeageResult.Message.Content);

                return false;

            }
        }
    }
}
