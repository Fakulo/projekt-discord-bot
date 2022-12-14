using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DiscordBot.Algorithm;
using DiscordBot.Database;
using DiscordBot.Models;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Google.Common.Geometry;
using Newtonsoft.Json;
using DSharpPlus.Interactivity;
using DiscordBot.Dialogue.Steps;
using DiscordBot.Dialogue;
using System.Threading;
using System.Linq;
using DSharpPlus.EventArgs;

namespace DiscordBot.Commands
{
    enum State
    {
        AddToDB,
        AddToDBCheck,
        ChangeName,
        ChangeNameCheck,
        ChangePosition,
        ChangePositionCheck,
        ChangePositionOrAdd,
        Duplicate,
        Unreachable
    }
    public class Basic
    {
        [Command("Ping")]
        public async Task Ping(CommandContext ctx)
        {
            var emoji = DiscordEmoji.FromName(ctx.Client, ":no_entry:");
            var embed = new DiscordEmbedBuilder
            {
                Title = "Access denied",
                Description = $"{emoji} You do not have the permissions required to execute this command.",
                Color = new DiscordColor(0xFF0000),               
                
            };
            await ctx.Channel.SendMessageAsync("pong",false,embed).ConfigureAwait(false);
        }

        [Command("getid")]
        public async Task Get(CommandContext ctx, double lat, double lng)
        {
            Item item = new Item(lat, lng);
            await ctx.Channel.SendMessageAsync("LVL 30: " + item.GetCell().Id.ToString()).ConfigureAwait(false);
            await ctx.Channel.SendMessageAsync("LVL 17: " + item.GetParent(17)).ConfigureAwait(false);
            await ctx.Channel.SendMessageAsync("LVL 14: " + item.GetParent(14)).ConfigureAwait(false);
        }

        [Command("add")]
        public async Task Add(CommandContext ctx, string name, double lat, double lng)
        {
            if (ctx.Channel.Id == 843455748401790996)
            {
                Item item = new(lat, lng, name);
                List<Item> points = new List<Item>
            {
                item
            };

                await ctx.Channel.SendMessageAsync("LVL 30: " + item.GetCell().Id.ToString()).ConfigureAwait(false);
                await ctx.Channel.SendMessageAsync("LVL 17: " + item.GetParent(17)).ConfigureAwait(false);

                //await AddToDatabase.AddToDB(points);
            }
        }
        [Command("addData")]
        public async Task AddData(CommandContext ctx)
        {
            if (ctx.Channel.Id == 843455748401790996)
            {
                List<Item> list = CsvHandler.ReadFile();
                Algorithm.DatabaseHandler db = new Algorithm.DatabaseHandler(ctx);

                await Task.Run(() => db.ProcessInputPoints(list));
            }
            
        }

        // TODO: Přidat české aliasy
        // TODO: Možnost zadání více stringů najednou
        [Command("changeType")]
        public async Task ChangeType(CommandContext ctx, string name)
        {
            if(ctx.Channel.Id == 843455748401790996 || ctx.Channel.Id == 842805476973608961)
            {
                DatabaseHandler db = new DatabaseHandler(ctx);

                await Task.Run(() => db.ChangePointType(name));
            }
            
        }

        [Command("changeName")]
        public async Task ChangeName(CommandContext ctx, string name)
        {
            if (ctx.Channel.Id == 843455748401790996)
            {
                DatabaseHandler db = new DatabaseHandler(ctx);

                await Task.Run(() => db.ChangePointName(name));
            }
        }

        [Command("check")]
        public async Task Check(CommandContext ctx)
        {
            if (ctx.Channel.Id == 843455748401790996)
            {
                DatabaseHandler db = new DatabaseHandler(ctx);

                await Task.Run(() => db.CheckPoints());
            }
        }

        [Command("checkCells")]
        public async Task CheckCells(CommandContext ctx)
        {
            if (ctx.Channel.Id == 843455748401790996 || ctx.Channel.Id == 842805476973608961)
            {
                DatabaseHandler db = new DatabaseHandler(ctx);
                PogoContext context = new PogoContext();

                await Task.Run(() => db.ChangeAllCells(context));
            }
        }

        [Command("list")]
        public async Task List(CommandContext ctx, string input)
        {
            DatabaseHandler db = new DatabaseHandler(ctx);

            //await Task.Run(() => db.CheckPoints());
            List<Point> points = db.GetPoints(input);
            if(points.Count > 0)
            {
                var interactivity = ctx.Client.GetInteractivityModule();

                var pages = this.GeneratePagesInEmbeds(ctx, points);
                //DiscordMessage m = await ctx.Client.SendMessageAsync(ctx.Channel, l[0]).ConfigureAwait(false);
                //PaginatedMessage pm = new PaginatedMessage();
                var pokestopEmoji = DiscordEmoji.FromName(ctx.Client, Emoji.Pokestop);
                var ct = new CancellationTokenSource(30);
                await interactivity.SendPaginatedMessage(ctx.Channel, ctx.User, pages, TimeSpan.FromMinutes(5), TimeoutBehaviour.Delete);
            }
            else
            {
                string reason = "Klíčové slovo " + input + " nebylo nalezeno v databázi.";
                SendBoxHandler.SendBoxMessage(ctx.Channel, "Bod nenalezen.", reason, DiscordColor.Yellow);
            }
            
        }

        [Command("pokemons")]
        public async Task Pokemons(CommandContext ctx)
        {
            DatabaseHandler db = new(ctx);
            //List<PokemonStat> pokemons = db.GetPokemons();
            using var context = new PogoContext();
            var gymCell = context.GymLocationCells.FirstOrDefault(i => i.IdCell14 == "5118682267492810752");
            //var pointCell = context.Points.FirstOrDefault(i => i.Name == "Liberec historická budova 1912");
            List<Point> points = gymCell.Points.ToList();
            foreach (var point in points)
            {
                await ctx.Channel.SendMessageAsync(point.Name).ConfigureAwait(false);
            }                  
       }

        public IEnumerable<Page> GeneratePagesInEmbeds(CommandContext ctx, List<Point> points)
        {
            if (points.Count == 0)
            {
                throw new InvalidOperationException("You must provide a string that is not null or empty!");
            }

            List<Page> list = new List<Page>();
            StringBuilder sb;         
            for (int i = 0; i < points.Count; i++)
            {
                sb = new StringBuilder();
                sb.AppendLine(Emoji.GetEmoji(ctx, points[i].Type) + " " + points[i].Type);
                sb.AppendLine(Emoji.GetEmoji(ctx, Emoji.Point) + " " + points[i].Latitude.ToString().Replace(",", ".") + ", " + points[i].Longitude.ToString().Replace(",", "."));
                sb.AppendLine(Emoji.GetEmoji(ctx, Emoji.Edit) + " " + points[i].UpdatedAt.ToString());
                
                var embed = new DiscordEmbedBuilder
                {
                    Title = points[i].Name,
                    Description = sb.ToString(),
                    Color = DiscordColor.Blue,
                    Url = "https://www.google.cz/maps/place/" + points[i].Latitude.ToString().Replace(",", ".") + "," + points[i].Longitude.ToString().Replace(",", ".")
                };
                string before = (i == 0) ? ("- " + Emoji.GetEmoji(ctx, Emoji.ArrowBack)) : (points[i - 1].Name + " " + Emoji.GetEmoji(ctx, Emoji.ArrowBack));
                string after = (i == points.Count-1) ? (Emoji.GetEmoji(ctx, Emoji.ArrowNext) + " -") : (Emoji.GetEmoji(ctx, Emoji.ArrowNext) + " " + points[i + 1].Name);
                embed.WithFooter($" {before} {i+1}/{points.Count} {after} ");
                list.Add( new Page {
                    Embed = embed                    
                });
            }

            return list;
        }


        [Command("checkold")]
        public async Task CheckOld(CommandContext ctx)
        {
            const string ConfirmRegex = "\\b[Yy][Ee]?[Ss]?\\b|\\b[Nn][Oo]?\\b";
            const string YesRegex = "[Yy][Ee]?[Ss]?";
            const string NoRegex = "[Nn][Oo]?";
            List<Point> list = new List<Point>();

            var interactivity = ctx.Client.GetInteractivityModule();

            await ctx.RespondAsync("Are you sure?");
            var m = await interactivity.WaitForMessageAsync(
                x => x.Channel.Id == ctx.Channel.Id
                && x.Author.Id == ctx.Member.Id
                && Regex.IsMatch(x.Content, ConfirmRegex));

            if (Regex.IsMatch(m.Message.Content, YesRegex)) {
                list = CheckData.GetPointsToCheck(ctx);
                await ctx.Channel.SendMessageAsync("Vyžaduje ruční kontrolu.");
                foreach (var point in list)
                {
                    await ctx.Channel.SendMessageAsync(point.Name);
                }
            }
            else 
            {
                await ctx.Channel.SendMessageAsync(m.Message.Content);
            }

            //await ctx.Channel.SendMessageAsync("done").ConfigureAwait(false);
        }



        [Command("dialogue")]
        public async Task Dialogue(CommandContext ctx)
        {
            var yesStep = new TextStep("Vybral si ANO", null);
            var noStep = new TextStep("Vybral si NE", null);

            var emojiStep = new ReactionStep("Začátek - první krok", new Dictionary<DiscordEmoji, ReactionStepData> {
                { DiscordEmoji.FromName(ctx.Client, Emoji.GreenCheck), new ReactionStepData { Content = "YES", NextStep = yesStep } },
                { DiscordEmoji.FromName(ctx.Client, Emoji.ArrowChange), new ReactionStepData { Content = "CHANGE", NextStep = noStep } },
            });

            //inputStep.OnValidResult += (result) => input = result;

            var inputDialogueHandler = new DialogueHandler(
                ctx.Client,
                ctx.Channel,
                ctx.User,
                emojiStep
            );

            bool succeeded = await inputDialogueHandler.ProcessDialogue().ConfigureAwait(false);

            if (!succeeded) { return; }

            await ctx.Channel.SendMessageAsync("Konec").ConfigureAwait(false);

        }
    }
}
