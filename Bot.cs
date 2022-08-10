using DiscordBot.Algorithm;
using DiscordBot.Commands;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class Bot
    {
        public DiscordClient Client { get; private set; }
        public CommandsNextModule Commands { get; private set; }

        public async Task RunAsync()
        {
            var json = string.Empty;

            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync().ConfigureAwait(false);

            var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

            var config = new DiscordConfiguration
            {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                LogLevel = LogLevel.Debug,
                UseInternalLogHandler = true
            };

            Client = new DiscordClient(config);

            Client.Ready += OnClientReady;

            Client.UseInteractivity(new InteractivityConfiguration
            { 
                Timeout = TimeSpan.FromSeconds(30)
            });

            Client.MessageCreated += OnTypingReady;

            

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefix = "!",
                EnableDms = false,
                EnableMentionPrefix = true                
            };

            Commands = Client.UseCommandsNext(commandsConfig);

            Commands.RegisterCommands<Basic>();
        

            await Client.ConnectAsync();

            await Task.Delay(-1);

        }

        private async Task<Task> OnTypingReady(MessageCreateEventArgs e)
        {
            if (e.Author.Id != e.Client.CurrentUser.Id)
            {
                var member = (DiscordMember)e.Author;

                Console.WriteLine("píše...");
                Console.WriteLine(e.Author.Username + ": " + e.Message.Content);
                await e.Channel.SendMessageAsync("Napsal si: " + e.Message.Content).ConfigureAwait(false);
                DiscordChannel channel = await member.CreateDmChannelAsync().ConfigureAwait(false);
                await channel.SendMessageAsync("Napsal si: " + e.Message.Content).ConfigureAwait(false);
            }            
            return Task.CompletedTask;
        }        

        private Task OnClientReady(ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
