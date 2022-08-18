using DiscordBot.Database;
using DiscordBot.Dialogue;
using DiscordBot.Dialogue.Steps;
using DiscordBot.Models;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DiscordBot.Models.Enums;

namespace DiscordBot.Algorithm
{
    class CheckData
    {
        /// <summary>
        /// Vytáhne z databáze list bodů v databází, které potřebují zkontrolovat.
        /// </summary>
        /// <param name="ctx">CommandContext</param>
        /// <returns>List bodů na kontrolu.</returns>
        public static List<Point> GetPointsToCheck(CommandContext ctx)
        {

            using var context = new PogoContext();
            var list = (from p in context.Points
                        where p.NeedCheck.Equals(NeedCheck.Yes)
                        select p).ToList();
            return list;
        }
        /// <summary>
        /// MessageBox s reakcemi, kterým se potvrzuje vytvoření nebo zamítnutí bodu.
        /// </summary>
        /// <param name="ctx">DB context</param>
        /// <param name="chnl">Kanál, kde se MessageBox zobrazí</param>
        /// <param name="p">Bod</param>
        /// <param name="title">Nadpis MessageBoxu</param>
        /// <param name="description">Text v MessageBoxu</param>
        /// <param name="color">Barva MessageBoxu</param>
        /// <returns>(true,typ bodu) v případě potvrzení nebo (false,typ bodu) v případě zamítnutí</returns>
        public static async Task<(bool, PointType)> AddCheckPoint(CommandContext ctx, Task<DiscordChannel> chnl, Item p, string title, string description, DiscordColor color)
        {
            var checkEmbed = new DiscordEmbedBuilder
            {
                Title = title,
                Description = description,
                Color = color,
                Url = "https://www.google.cz/maps/place/" + p.Latitude.ToString().Replace(",", ".") + "," + p.Longitude.ToString().Replace(",", "."),
                Timestamp = DateTime.Now,
            };
            // Poslání zprávy
            var message = await chnl.Result.SendMessageAsync(embed: checkEmbed);

            var pokestopEmoji = DiscordEmoji.FromName(ctx.Client, Emoji.Pokestop);
            var ingressEmoji = DiscordEmoji.FromName(ctx.Client, Emoji.Ingress);
            var cancelEmoji = DiscordEmoji.FromName(ctx.Client, Emoji.RedCross);

            // Vytvoření reakcí
            await message.CreateReactionAsync(pokestopEmoji);
            await message.CreateReactionAsync(ingressEmoji);
            await message.CreateReactionAsync(cancelEmoji);

            // Čekání na stisknutí na reakci
            var interactivity = ctx.Client.GetInteractivityModule();
            var reactionResult = await interactivity.WaitForReactionAsync(
                x => x == pokestopEmoji || x == ingressEmoji || x == cancelEmoji,
                ctx.User,
                TimeSpan.FromSeconds(30)
            ).ConfigureAwait(false);

            // Uživatel nezareaguje do 30 vteřin - vrací se false a typ portál + zpráva, že čas vypršel
            // TODO: místo PointType.Portal vracet PointType.None - pokud se vrací false
            if (reactionResult == null)
            {
                await message.DeleteAsync();
                Methods.SendBoxMessage(chnl, "Časový limit vypršel - BOD NEPŘIDÁN.", "Na rozhodnutí máte 30 vteřin, potom je návrh zamítnut.", DiscordColor.Red, p.Latitude, p.Longitude);
                return (false, PointType.Portal);
            }
            // Uživatel zareaguje na pokestop - vrací se true a typ pokestop
            if (reactionResult.Emoji == pokestopEmoji)
            {
                await message.DeleteAsync();
                return (true, PointType.Pokestop);
            }
            // Uživatel zareaguje na portal - vrací se true a typ portal
            else if (reactionResult.Emoji == ingressEmoji)
            {
                await message.DeleteAsync();
                return (true, PointType.Portal);
            }
            // Uživatel zareaguje na křížek - vrací se false a typ portal
            else if (reactionResult.Emoji == cancelEmoji)
            {
                await message.DeleteAsync();
                return (false, PointType.Portal);
            }
            return (false, PointType.Portal);
        }

        /// <summary>
        /// MessageBox s reakcemi, kterým se potvrzuje nebo zamítne změna souřadnic bodu.
        /// </summary>
        /// <param name="ctx">DB context</param>
        /// <param name="chnl">Kanál, kde se MessageBox zobrazí</param>
        /// <param name="p">Bod</param>
        /// <param name="title">Nadpis MessageBoxu</param>
        /// <param name="description"></param>
        /// <param name="color">Barva MessageBoxu</param>
        /// <returns>(true) v případě potvrzení nebo (false) v případě zamítnutí</returns>
        public static async Task<bool> CheckPoint(CommandContext ctx, Task<DiscordChannel> chnl, Point p, string title, string description, DiscordColor color)
        {
            var checkEmbed = new DiscordEmbedBuilder
            {
                Title = title,
                Description = description,
                Color = color,
                Url = "https://www.google.cz/maps/place/" + p.Latitude.ToString().Replace(",", ".") + "," + p.Longitude.ToString().Replace(",", "."),
                Timestamp = DateTime.Now,
            };
            // Poslání zprávy
            var message = await chnl.Result.SendMessageAsync(embed: checkEmbed);

            var cancelEmoji = DiscordEmoji.FromName(ctx.Client, Emoji.RedCross);
            var greenCheckEmoji = DiscordEmoji.FromName(ctx.Client, Emoji.GreenCheck);

            // Vytvoření reakcí
            await message.CreateReactionAsync(greenCheckEmoji);
            await message.CreateReactionAsync(cancelEmoji);

            // TODO: Čas 30 sekund použít jako konstantu pro všechny 
            // Čekání na stisknutí na reakci
            var interactivity = ctx.Client.GetInteractivityModule();
            var reactionResult = await interactivity.WaitForReactionAsync(
                x => x == cancelEmoji || x == greenCheckEmoji,
                ctx.User,
                TimeSpan.FromSeconds(30)
            ).ConfigureAwait(false);

            // Uživatel nezareaguje do 30 vteřin - vrací se false + zpráva, že čas vypršel
            if (reactionResult == null)
            {
                await message.DeleteAsync();
                Methods.SendBoxMessage(chnl, "Časový limit vypršel - ZMĚNA ZAMÍTNUTA.", "Na rozhodnutí máte 30 vteřin, potom je návrh zamítnut.", DiscordColor.Red, p.Latitude, p.Longitude);
                return false;
            }
            // Uživatel zareaguje na potvrzení - vrací se true
            if (reactionResult.Emoji == greenCheckEmoji)
            {
                await message.DeleteAsync();
                return true;
            }
            // Uživatel zareaguje na zamítnutí - vrací se false
            else if (reactionResult.Emoji == cancelEmoji)
            {
                await message.DeleteAsync();
                return false;
            }
            return false;
        }

        /// <summary>
        /// MessageBox s reakcemi, kterýma se změní typ bodu.
        /// </summary>
        /// <param name="ctx">DB context</param>
        /// <param name="chnl">Kanál, kde se MessageBox zobrazí</param>
        /// <param name="p">Bod</param>
        /// <param name="title">Nadpis MessageBoxu</param>
        /// <param name="description"></param>
        /// <param name="color">Barva MessageBoxu</param>
        /// <returns>(true,typ bodu) v případě změny nebo (false,typ bodu) v případě odmítnutí změny</returns>
        public static async Task<(bool, PointType)> ChangeType(CommandContext ctx, Task<DiscordChannel> chnl, Point p, string title, string description, DiscordColor color)
        {
            var checkEmbed = new DiscordEmbedBuilder
            {
                Title = title,
                Description = description,
                Color = color,
                Url = "https://www.google.cz/maps/place/" + p.Latitude.ToString().Replace(",", ".") + "," + p.Longitude.ToString().Replace(",", "."),
                Timestamp = DateTime.Now,
            };

            // Poslání zprávy
            var message = await chnl.Result.SendMessageAsync(embed: checkEmbed);

            var pokestopEmoji = DiscordEmoji.FromName(ctx.Client, Emoji.Pokestop);
            var gymEmoji = DiscordEmoji.FromName(ctx.Client, Emoji.Gym);
            var exGymEmoji = DiscordEmoji.FromName(ctx.Client, Emoji.ExGym);
            var portalEmoji = DiscordEmoji.FromName(ctx.Client, Emoji.Ingress);
            var skipEmoji = DiscordEmoji.FromName(ctx.Client, Emoji.ArrowRight);

            // Vytvoření reakcí
            if (p.Type != PointType.Pokestop) { await message.CreateReactionAsync(pokestopEmoji); }
            if (p.Type != PointType.Gym) { await message.CreateReactionAsync(gymEmoji); }
            if (p.Type != PointType.ExGym) { await message.CreateReactionAsync(exGymEmoji); }
            if (p.Type != PointType.Portal) { await message.CreateReactionAsync(portalEmoji); }            

            await message.CreateReactionAsync(skipEmoji);

            // Čekání na stisknutí na reakci
            var interactivity = ctx.Client.GetInteractivityModule();
            var reactionResult = await interactivity.WaitForReactionAsync(
                x => x == pokestopEmoji || x == gymEmoji || x == exGymEmoji || x == portalEmoji || x == skipEmoji,
                ctx.User,
                TimeSpan.FromSeconds(30)
            ).ConfigureAwait(false);

            // Uživatel nezareaguje do 30 vteřin - vrací se false a typ portál + zpráva, že čas vypršel
            if (reactionResult == null)
            {
                await message.DeleteAsync();
                Methods.SendBoxMessage(chnl, "Časový limit vypršel - TYP NEZMĚNĚN.", "Na rozhodnutí máte 30 vteřin, potom je návrh zamítnut.", DiscordColor.Red, p.Latitude, p.Longitude);
                return (false, PointType.Portal);
            }
            // Uživatel zareaguje na Pokestop - vrací se true + typ Pokestop
            if (reactionResult.Emoji == pokestopEmoji)
            {
                await message.DeleteAsync();
                return (true, PointType.Pokestop);
            }
            // Uživatel zareaguje na Gym - vrací se true + typ Gym
            else if (reactionResult.Emoji == gymEmoji)
            {
                await message.DeleteAsync();
                return (true, PointType.Gym);
            }
            // Uživatel zareaguje na ExGym - vrací se true + typ ExGym
            else if (reactionResult.Emoji == exGymEmoji)
            {
                await message.DeleteAsync();
                return (true, PointType.ExGym);
            }
            // Uživatel zareaguje na Portál - vrací se true + typ Portál
            else if (reactionResult.Emoji == portalEmoji)
            {
                await message.DeleteAsync();
                return (true, PointType.Portal);
            }
            // Uživatel zareaguje na přeskočení - vrací se false + typ Portál
            else if (reactionResult.Emoji == skipEmoji)
            {
                await message.DeleteAsync();
                return (false, PointType.Portal);
            }
            return (false, PointType.Portal);
        }

        /// <summary>
        /// MessageBox s reakcemi, kterýma se změní název bodu.
        /// </summary>
        /// <param name="ctx">DB context</param>
        /// <param name="chnl">Kanál, kde se MessageBox zobrazí</param>
        /// <param name="p">Bod</param>
        /// <param name="title">Nadpis MessageBoxu</param>
        /// <param name="description"></param>
        /// <param name="color">Barva MessageBoxu</param>
        /// <returns>(true,název bodu) v případě změny nebo (false,"") v případě odmítnutí změny</returns>
        public static async Task<(bool, string)> ChangeName(CommandContext ctx, Task<DiscordChannel> chnl, Point p, string title, string description, DiscordColor color)
        {
            var checkEmbed = new DiscordEmbedBuilder
            {
                Title = title,
                Description = description,
                Color = color,
                Url = "https://www.google.cz/maps/place/" + p.Latitude.ToString().Replace(",", ".") + "," + p.Longitude.ToString().Replace(",", "."),
                Timestamp = DateTime.Now,
            };

            // Poslání zprávy
            var message = await chnl.Result.SendMessageAsync(embed: checkEmbed);

            var skipEmoji = DiscordEmoji.FromName(ctx.Client, Emoji.ArrowRight);

            // Vytvoření reakcí
            await message.CreateReactionAsync(skipEmoji);

            // Čekání na stisknutí na reakci
            var interactivity = ctx.Client.GetInteractivityModule();            
            
            Task<MessageContext> messageResult = interactivity.WaitForMessageAsync(x => x.Channel.Id == 842805476973608961 && x.Author.Id == ctx.User.Id, TimeSpan.FromSeconds(30));
            Task<ReactionContext> reactionResult = interactivity.WaitForReactionAsync(x => x == skipEmoji, ctx.User, TimeSpan.FromSeconds(30));

            List<Task> tasks = new() { messageResult, reactionResult };

            var result = await Task.WhenAny(tasks);
            StringBuilder sb = new();
            sb.AppendLine(Emoji.GetEmoji(ctx, p.Type) + " " + p.Name);
            sb.AppendLine();

            if (result == messageResult)
            {
                MessageContext messageContext = await messageResult;
                // Uživatel nezareaguje do 30 vteřin - vrací se false a "" + zpráva, že čas vypršel
                if (messageContext == null)
                {
                    await messageContext.Message.DeleteAsync();
                    await message.DeleteAsync();
                    sb.AppendLine("Na rozhodnutí máte 30 vteřin, potom je návrh zamítnut.");
                    Methods.SendBoxMessage(chnl, "Časový limit vypršel - NÁZEV NEZMĚNĚN.", sb.ToString(), DiscordColor.Red, p.Latitude, p.Longitude);
                    return (false, "");
                }
                // Uživatel napíše stejný název - vrací se false + ""
                else if (messageContext.Message.Content.Equals(p.Name))
                {
                    await messageContext.Message.DeleteAsync();
                    await message.DeleteAsync();
                    sb.AppendLine("Nový název je stejný jako původní!");
                    Methods.SendBoxMessage(chnl, "Změna neproběhla.", sb.ToString(), DiscordColor.Red, p.Latitude, p.Longitude);
                    
                    return (false, "");
                }
                // Uživatel napíše nový název - vrací se true + nový název
                else if (messageContext.Message.Content.Length > 0)
                {
                    // TODO: Odchytit UnauthorizedException: Unauthorized: 403
                    string mess = messageContext.Message.Content;
                    await messageContext.Message.DeleteAsync();
                    await message.DeleteAsync();
                    return (true, mess);
                }
            }

            if (result == reactionResult)
            {
                ReactionContext reactionContext = await reactionResult;
                // Uživatel nezareaguje do 30 vteřin - vrací se false a "" + zpráva, že čas vypršel
                if (reactionContext == null)
                {
                    await message.DeleteAsync();
                    sb.AppendLine("Na rozhodnutí máte 30 vteřin, potom je návrh zamítnut.");
                    Methods.SendBoxMessage(chnl, "Časový limit vypršel - NÁZEV NEZMĚNĚN.", sb.ToString(), DiscordColor.Red, p.Latitude, p.Longitude);
                    return (false, "");
                }
                // Uživatel zareaguje na přeskočení - vrací se false + ""
                else if (reactionContext.Emoji == skipEmoji)
                {
                    await message.DeleteAsync();
                    return (false, "");
                }
            }

            return (false, "");
        }

        /// <summary>
        /// MessageBox s reakcemi, kterýma se provede změna bodu.
        /// </summary>
        /// <param name="ctx">DB context</param>
        /// <param name="chnl">Kanál, kde se MessageBox zobrazí</param>
        /// <param name="p">Bod</param>
        /// <param name="title">Nadpis MessageBoxu</param>
        /// <param name="description">Popis</param>
        /// <param name="color">Barva MessageBoxu</param>
        /// <returns>(true) v případě změny nebo (false) v případě odmítnutí změny</returns>
        public static async Task<(ManualChange,string)> ManualCheck(CommandContext ctx, Task<DiscordChannel> chnl, Point p, string title, string description, DiscordColor color)
        {
            var checkEmbed = new DiscordEmbedBuilder
            {
                Title = title,
                Description = description,
                Color = color,
                Url = "https://www.google.cz/maps/place/" + p.Latitude.ToString().Replace(",", ".") + "," + p.Longitude.ToString().Replace(",", "."),
                Timestamp = DateTime.Now,
            };

            // Poslání zprávy
            var message = await chnl.Result.SendMessageAsync(embed: checkEmbed);

            var editEmoji = DiscordEmoji.FromName(ctx.Client, Emoji.Edit);
            var mapEmoji = DiscordEmoji.FromName(ctx.Client, Emoji.Map);
            var pokestopEmoji = DiscordEmoji.FromName(ctx.Client, Emoji.Pokestop);
            var gymEmoji = DiscordEmoji.FromName(ctx.Client, Emoji.Gym);
            var exGymEmoji = DiscordEmoji.FromName(ctx.Client, Emoji.ExGym);
            var portalEmoji = DiscordEmoji.FromName(ctx.Client, Emoji.Ingress);
            var skipEmoji = DiscordEmoji.FromName(ctx.Client, Emoji.ArrowRight);
            var crossEmoji = DiscordEmoji.FromName(ctx.Client, Emoji.RedCross);

            // Vytvoření reakcí
            await message.CreateReactionAsync(editEmoji);
            await message.CreateReactionAsync(mapEmoji);
            await message.CreateReactionAsync(pokestopEmoji);
            await message.CreateReactionAsync(gymEmoji);
            await message.CreateReactionAsync(exGymEmoji);
            await message.CreateReactionAsync(portalEmoji);
            await message.CreateReactionAsync(skipEmoji);
            await message.CreateReactionAsync(crossEmoji);

            // Čekání na stisknutí na reakci
            var interactivity = ctx.Client.GetInteractivityModule();
            var reactionResult = await interactivity.WaitForReactionAsync(
                x => x == editEmoji || x == mapEmoji || x == pokestopEmoji || x == gymEmoji || x == exGymEmoji || x == portalEmoji || x == skipEmoji || x == crossEmoji,
                ctx.User,
                TimeSpan.FromSeconds(30)
            ).ConfigureAwait(false);


            // Uživatel nezareaguje do 30 vteřin - vrací se false a typ portál + zpráva, že čas vypršel
            if (reactionResult == null)
            {
                await message.DeleteAsync();
                Methods.SendBoxMessage(chnl, "Časový limit vypršel - ZMĚNA NEPROVEDENA.", "Na rozhodnutí máte 30 vteřin, potom je návrh zamítnut.", DiscordColor.Red, p.Latitude, p.Longitude);
                return (ManualChange.TimeOut,"");
            }
            // Uživatel zareaguje na Pokestop - vrací se true + typ Pokestop
            if (reactionResult.Emoji == editEmoji)
            {
                await message.DeleteAsync();

                StringBuilder sb = new();
                sb.AppendLine(Emoji.GetEmoji(ctx, p.Type) + " " + p.Name);

                StringBuilder sbr = new();
                sbr.AppendLine("ZMĚNA NÁZVU:");
                sbr.Append("Napiš název nebo zruš volbu pomocí ");
                sbr.Append(Emoji.GetEmoji(ctx, Emoji.RedCross));

                var checkEmbed2 = new DiscordEmbedBuilder
                {
                    Title = sb.ToString(),
                    Description = sbr.ToString(),
                    Color = color,
                    Url = "https://www.google.cz/maps/place/" + p.Latitude.ToString().Replace(",", ".") + "," + p.Longitude.ToString().Replace(",", "."),
                    Timestamp = DateTime.Now,
                };

                // Poslání zprávy
                var message2 = await chnl.Result.SendMessageAsync(embed: checkEmbed2);

                // Vytvoření reakcí
                await message2.CreateReactionAsync(crossEmoji);

                // Čekání na stisknutí na reakci
                Task<MessageContext> messageResult2 = interactivity.WaitForMessageAsync(x => x.Channel.Id == 842805476973608961 && x.Author.Id == ctx.User.Id, TimeSpan.FromSeconds(30));
                Task<ReactionContext> reactionResult2 = interactivity.WaitForReactionAsync(x => x == crossEmoji, ctx.User, TimeSpan.FromSeconds(30));
                
                List<Task> tasks = new() { messageResult2, reactionResult2 };

                var result = await Task.WhenAny(tasks);

                // TODO: Umožnit změnit všechny parametry najednou/postupně (nové emoji přidat)
                // Uživatel nezareaguje do 30 vteřin - vrací se false a "" + zpráva, že čas vypršel
                if (result == messageResult2)
                {
                    MessageContext messageContext2 = await messageResult2;
                    // Uživatel nezareaguje do 30 vteřin - vrací se false a "" + zpráva, že čas vypršel
                    if (messageContext2 == null)
                    {
                        await messageContext2.Message.DeleteAsync();
                        sb.AppendLine("Na rozhodnutí máte 30 vteřin, potom je návrh zamítnut.");
                        Methods.SendBoxMessage(chnl, "Časový limit vypršel - NÁZEV NEZMĚNĚN.", sb.ToString(), DiscordColor.Red, p.Latitude, p.Longitude);
                        return (ManualChange.TimeOut, "");
                    }
                    // Uživatel napíše stejný název - vrací se false + ""
                    else if (messageContext2.Message.Content.Equals(p.Name))
                    {
                        await messageContext2.Message.DeleteAsync();
                        sb.AppendLine("Nový název je stejný jako původní!");
                        Methods.SendBoxMessage(chnl, "Změna neproběhla.", sb.ToString(), DiscordColor.Red, p.Latitude, p.Longitude);

                        return (ManualChange.Duplicate, "");
                    }
                    // Uživatel napíše nový název - vrací se true + nový název
                    else if (messageContext2.Message.Content.Length > 0)
                    {
                        // TODO: Odchytit UnauthorizedException: Unauthorized: 403
                        string mess = messageContext2.Message.Content;
                        await messageContext2.Message.DeleteAsync();
                        return (ManualChange.Name, mess);
                    }
                }
                if (result == reactionResult2)
                {
                    ReactionContext reactionContext2 = await reactionResult2;
                    // Uživatel nezareaguje do 30 vteřin - vrací se false a "" + zpráva, že čas vypršel
                    if (reactionContext2 == null)
                    {
                        await reactionContext2.Message.DeleteAsync();
                        sb.AppendLine("Na rozhodnutí máte 30 vteřin, potom je návrh zamítnut.");
                        Methods.SendBoxMessage(chnl, "Časový limit vypršel - NÁZEV NEZMĚNĚN.", sb.ToString(), DiscordColor.Red, p.Latitude, p.Longitude);
                        return (ManualChange.TimeOut, "");
                    }
                    // Uživatel zareaguje na přeskočení - vrací se false + ""
                    else if (reactionContext2.Emoji == crossEmoji)
                    {
                        await reactionContext2.Message.DeleteAsync();
                        return (ManualChange.Skip, "");
                    }
                }
                return (ManualChange.Skip, "");
            }
            // TODO: Změna na Switch-Case
            // Uživatel zareaguje na pokestop - vrací se 
            else if (reactionResult.Emoji == pokestopEmoji)
            {
                await message.DeleteAsync();
                return (ManualChange.Type, "Pokestop");
            }

            // Uživatel zareaguje na gym - vrací se 
            else if (reactionResult.Emoji == gymEmoji)
            {
                await message.DeleteAsync();
                return (ManualChange.Type, "Gym");
            }

            // Uživatel zareaguje na exgym - vrací se 
            else if (reactionResult.Emoji == exGymEmoji)
            {
                await message.DeleteAsync();
                return (ManualChange.Type, "ExGym");
            }

            // Uživatel zareaguje na portal - vrací se 
            else if (reactionResult.Emoji == portalEmoji)
            {
                await message.DeleteAsync();
                return (ManualChange.Type, "Portal");
            }                     

            // Uživatel zareaguje na přeskočení - vrací se false
            else if (reactionResult.Emoji == skipEmoji)
            {
                await message.DeleteAsync();
                return (ManualChange.Skip,"");
            }

            // Uživatel zareaguje na křížek - vrací se 
            else if (reactionResult.Emoji == crossEmoji)
            {
                await message.DeleteAsync();
                return (ManualChange.Cancel, "");
            }

            return (ManualChange.Skip, "");
        }

        public enum ManualChange
        {
            Type,
            Name,
            Duplicate,
            Coordinates,
            TimeOut,
            Skip,
            Cancel
        }
    }
}
