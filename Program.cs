using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Lavalink;
using DSharpPlus.Net;
using DSharpPlus.SlashCommands;
using System.Threading.Tasks;
using DiscordBoT.Config;
using DiscordBoT.Commands.Prefix;
using System;

namespace DiscordBoT
{
    public sealed class Program
    {
        public static DiscordClient Client { get; set; }
        public static CommandsNextExtension Commands { get; set; }
        static async Task Main()
        {
            //Считыватель настроек префикса и токена бота
            var configJson = new JSONReader();
            await configJson.ReadJSON();

            //Установка базовых настроек бота
            var config = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = configJson.token,
                TokenType = TokenType.Bot,
                AutoReconnect = true
            };
            Client = new DiscordClient(config);

            Client.UseInteractivity(new InteractivityConfiguration()
            {
                Timeout = TimeSpan.FromMinutes(2)
            });

            Client.Ready += OnClientReady; 
            Client.ComponentInteractionCreated += InteractionEventHandler;
            Client.MessageCreated += MessageSendHandler;
            Client.ModalSubmitted += ModalEventHandler;
            Client.VoiceStateUpdated += VoiceChannelHandler;
            Client.GuildMemberAdded += UserJoinHandler;

            //Конфигурация префикса для вызова комманд
            var commandsConfig = new CommandsNextConfiguration()
            {
                StringPrefixes = new string[] {configJson.prefix},
                EnableMentionPrefix = true,
                EnableDms = true,
                EnableDefaultHelp = false
            };
            Commands = Client.UseCommandsNext(commandsConfig);

            // Команды префикс
            Commands.RegisterCommands<Music>();

            //Конфигурация LavaLink для музыки
            var endpoint = new ConnectionEndpoint
            {
                Hostname = "v3.lavalink.rocks",
                Port = 443,
                Secured = true
            };
            var lavalinkConfig = new LavalinkConfiguration
            {
                Password = "horizxon.tech",
                RestEndpoint = endpoint,
                SocketEndpoint = endpoint
            };
            var lavalink = Client.UseLavalink();

            //Запуск бота и дополнение LavaLink
            await Client.ConnectAsync();
            await lavalink.ConnectAsync(lavalinkConfig);
            await Task.Delay(-1);
        }
        private static Task OnClientReady(DiscordClient sender, ReadyEventArgs args)
        {
            return Task.CompletedTask;
        }
        private static async Task UserJoinHandler(DiscordClient sender, GuildMemberAddEventArgs args)
        {
            throw new NotImplementedException();
        }

        private static async Task VoiceChannelHandler(DiscordClient sender, VoiceStateUpdateEventArgs args)
        {
            throw new NotImplementedException();
        }

        private static async Task ModalEventHandler(DiscordClient sender, ModalSubmitEventArgs args)
        {
            throw new NotImplementedException();
        }

        private static async Task MessageSendHandler(DiscordClient sender, MessageCreateEventArgs args)
        {
            throw new NotImplementedException();
        }

        private static async Task InteractionEventHandler(DiscordClient sender, ComponentInteractionCreateEventArgs args)
        {
            throw new NotImplementedException();
        }

    }
}
