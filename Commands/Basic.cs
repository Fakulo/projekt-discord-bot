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
        }

        [Command("add")]
        public async Task Add(CommandContext ctx, string name, double lat, double lng)
        {            
            Item item = new Item(lat, lng, name);
            List<Item> points = new List<Item>();
            points.Add(item);

            await ctx.Channel.SendMessageAsync("LVL 30: " + item.GetCell().Id.ToString()).ConfigureAwait(false);
            await ctx.Channel.SendMessageAsync("LVL 17: " + item.GetParent(17)).ConfigureAwait(false);
            
            //await AddToDatabase.AddToDB(points);

        }
        [Command("addData")]
        public async Task AddData(CommandContext ctx)
        {
            List<Item> list = CsvFile.ReadFile();
            Algorithm.DatabaseComm db = new Algorithm.DatabaseComm(ctx);

            db.ProcessInputPoints(list);            
            
        }

        // TODO: Přidat české aliasy
        // TODO: Možnost zadání více stringů najednou
        [Command("changeType")]
        public async Task ChangeType(CommandContext ctx, string name)
        {
            DatabaseComm db = new DatabaseComm(ctx);

            await Task.Run(() => db.ChangePointType(name));
        }

        [Command("changeName")]
        public async Task ChangeName(CommandContext ctx, string name)
        {
            DatabaseComm db = new DatabaseComm(ctx);

            await Task.Run(() => db.ChangePointName(name));
        }

        [Command("check")]
        public async Task Check(CommandContext ctx)
        {
            DatabaseComm db = new DatabaseComm(ctx);

            await Task.Run(() => db.CheckPoints());
        }

        [Command("list")]
        public async Task List(CommandContext ctx, string input)
        {
            DatabaseComm db = new DatabaseComm(ctx);

            //await Task.Run(() => db.CheckPoints());
            List<Point> points = db.GetPoints(input);
            var interactivity = ctx.Client.GetInteractivityModule();      
            
            var pages = this.GeneratePagesInEmbeds(ctx, points);
            //DiscordMessage m = await ctx.Client.SendMessageAsync(ctx.Channel, l[0]).ConfigureAwait(false);
            //PaginatedMessage pm = new PaginatedMessage();
            var pokestopEmoji = DiscordEmoji.FromName(ctx.Client, Emoji.Pokestop);
            var ct = new CancellationTokenSource(30);
            await interactivity.SendPaginatedMessage(ctx.Channel, ctx.User, pages,TimeSpan.FromMinutes(5),TimeoutBehaviour.Delete);
        }

        [Command("pokemons")]
        public async Task Pokemons(CommandContext ctx)
        {
            DatabaseComm db = new(ctx);            
            List<Pokemon> pokemons = db.GetPokemons();
            
            await ctx.Channel.SendMessageAsync(pokemons[0].Name.ToString()).ConfigureAwait(false);           
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
                sb.AppendLine(Emoji.GetEmoji(ctx, Enum.Parse<PointType>(points[i].Type)) + " " + points[i].Type);
                sb.AppendLine(Emoji.GetEmoji(ctx, Emoji.Point) + " " + points[i].Latitude.ToString().Replace(",", ".") + ", " + points[i].Longitude.ToString().Replace(",", "."));
                sb.AppendLine(Emoji.GetEmoji(ctx, Emoji.Edit) + " " + points[i].LastUpdate.ToString());
                
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
