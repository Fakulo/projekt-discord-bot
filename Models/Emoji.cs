using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using static DiscordBot.Models.Enums;

namespace DiscordBot.Models
{
    class Emoji
    {
        public static DiscordEmoji GetEmoji(CommandContext ctx, string name)
        {
            DiscordEmoji emoji = DiscordEmoji.FromName(ctx.Client, name);
            return emoji;
        }

        public static DiscordEmoji GetEmoji(DiscordClient client, string name)
        {
            DiscordEmoji emoji = DiscordEmoji.FromName(client, name);
            return emoji;
        }
        /// <summary>
        /// Vrací emoji na základě typu bodu.
        /// </summary>
        /// <param name="ctx">CommandContext</param>
        /// <param name="type">Typ bodu</param>
        /// <returns>Emoji</returns>
        public static DiscordEmoji GetEmoji(CommandContext ctx, PointType type)
        {
            return GetEmojiFromType(ctx.Client, type);
        }
        public static DiscordEmoji GetEmoji(DiscordClient client, PointType type)
        {
            return GetEmojiFromType(client, type);
        }

        private static DiscordEmoji GetEmojiFromType(DiscordClient client, PointType type)
        {
            DiscordEmoji emoji;
            switch (type)
            {
                case PointType.Pokestop:
                    emoji = DiscordEmoji.FromName(client, Pokestop);
                    return emoji;
                case PointType.Gym:
                    emoji = DiscordEmoji.FromName(client, Gym);
                    return emoji;
                case PointType.ExGym:
                    emoji = DiscordEmoji.FromName(client, ExGym);
                    return emoji;
                case PointType.Portal:
                    emoji = DiscordEmoji.FromName(client, Ingress);
                    return emoji;
                default:
                    emoji = DiscordEmoji.FromName(client, Question);
                    return emoji;
            }
        }

        public static string ArrowBack { get { return ":arrow_backward:"; } }
        public static string ArrowChange { get { return ":arrows_counterclockwise:"; } }
        public static string ArrowNext { get { return ":arrow_forward:"; } }
        public static string ArrowRight { get { return ":arrow_right:"; } }
        public static string Edit { get { return ":pencil2:"; } }
        public static string ExGym { get { return ":exgym:"; } }
        public static string GreenCheck { get { return ":white_check_mark:"; } }
        public static string Gym { get { return ":gym:"; } }
        public static string Id { get { return ":id:"; } }
        public static string Invite { get { return ":envelope_with_arrow:"; } }
        public static string Ingress { get { return ":ingress:"; } }
        public static string Location { get { return ":round_pushpin:"; } }
        public static string Map { get { return ":map:"; } }
        public static string New { get { return ":new:"; } }
        public static string Point { get { return ":triangular_flag_on_post:"; } }
        public static string Pokestop { get { return ":pokestop:"; } }
        public static string Question { get { return ":grey_question:"; } }
        public static string RedCross { get { return ":x:"; } }
        public static string ThumbsUp { get { return ":thumbsup:"; } }
        public static string Warning { get { return ":warning:"; } }

        public static string One { get { return ":one:"; } }
        public static string Two { get { return ":two:"; } }
        public static string Three { get { return ":three:"; } }
        public static string Four { get { return ":four:"; } }
        public static string Five { get { return ":five:"; } }
        


    }
}
