using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;

namespace DiscordBot.Models
{
    class Emoji
    {   
        public static DiscordEmoji GetEmoji(CommandContext ctx, string name)
        {
            DiscordEmoji emoji = DiscordEmoji.FromName(ctx.Client, name);
            return emoji;
        }
        /// <summary>
        /// Vrací emoji na základě typu bodu.
        /// </summary>
        /// <param name="ctx">CommandContext</param>
        /// <param name="type">Typ bodu</param>
        /// <returns>Emoji</returns>
        public static DiscordEmoji GetEmoji(CommandContext ctx, Enums.PointType type)
        {
            DiscordEmoji emoji;
            switch (type)
            {
                case Enums.PointType.Pokestop:
                    emoji = DiscordEmoji.FromName(ctx.Client, Pokestop);
                    return emoji;
                case Enums.PointType.Gym:
                    emoji = DiscordEmoji.FromName(ctx.Client, Gym);
                    return emoji;
                case Enums.PointType.ExGym:
                    emoji = DiscordEmoji.FromName(ctx.Client, ExGym);
                    return emoji;
                case Enums.PointType.Portal:
                    emoji = DiscordEmoji.FromName(ctx.Client, Ingress);
                    return emoji;
                default:
                    emoji = DiscordEmoji.FromName(ctx.Client, Question);
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
        public static string Ingress { get { return ":ingress:"; } }
        public static string Location { get { return ":round_pushpin:"; } }
        public static string Map { get { return ":map:"; } }
        public static string New { get { return ":new:"; } }
        public static string Point { get { return ":triangular_flag_on_post:"; } }
        public static string Pokestop { get { return ":pokestop:"; } }
        public static string Question { get { return ":grey_question:"; } }
        public static string RedCross { get { return ":x:"; } }           
        public static string Warning { get { return ":warning:"; } }
        
        
        


    }
}
