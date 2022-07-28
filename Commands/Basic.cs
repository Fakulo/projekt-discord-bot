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
        public async Task List(CommandContext ctx)
        {
            // DatabaseComm db = new DatabaseComm(ctx);

            //await Task.Run(() => db.CheckPoints());
            string reallyLongString = "Lorem ipsum dolor sit amet, consectetur adipiscing ...";
            var interactivity = ctx.Client.GetInteractivityModule();
            List<String> l = new List<String>();
            for (int i = 0; i < 10; i++)
            {                
                l.Add("Test textu " + i);
            }
            var pages = this.GeneratePagesInEmbeds(l);
            //DiscordMessage m = await ctx.Client.SendMessageAsync(ctx.Channel, l[0]).ConfigureAwait(false);
            PaginatedMessage pm = new PaginatedMessage();
            var pokestopEmoji = DiscordEmoji.FromName(ctx.Client, Emoji.Pokestop);
            var ct = new CancellationTokenSource(30);
            await interactivity.SendPaginatedMessage(ctx.Channel, ctx.User, pages,TimeSpan.FromMinutes(1),TimeoutBehaviour.Delete);
        }

        public IEnumerable<Page> GeneratePagesInEmbeds(List<String> input)
        {
            if (input.Count == 0)
            {
                throw new InvalidOperationException("You must provide a string that is not null or empty!");
            }

            List<Page> list = new List<Page>();
            int num = 1;
            foreach (string item in input)
            {
                list.Add(new Page
                {
                    Embed = new DiscordEmbedBuilder
                    {
                        Title = $"Page {num}",
                        Description = item
                    }
                });
                num++;
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
