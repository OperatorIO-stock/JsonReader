using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBoT.Commands.Prefix
{
    public class Music : BaseCommandModule
    {
        [Command("play")]
        public async Task PlayMusic(CommandContext ctx, [RemainingText] string query)
        {
            var userVC = ctx.Member.VoiceState.Channel;
            var lavalinkInstance = ctx.Client.GetLavalink();

            if (ctx.Member.VoiceState == null || userVC == null)
            {
                await ctx.Channel.SendMessageAsync("User находится не в голосовом чате");
                return;
            }

            if (!lavalinkInstance.ConnectedNodes.Any())
            {
                await ctx.Channel.SendMessageAsync("Соединения не установлено");
                return;
            }

            if (userVC.Type != ChannelType.Voice)
            {
                await ctx.Channel.SendMessageAsync("Голосовой чат не найден");
                return;
            }

            

            var node = lavalinkInstance.ConnectedNodes.Values.First();
            await node.ConnectAsync(userVC);

            var connection = node.GetGuildConnection(ctx.Member.VoiceState.Guild);
            if (connection == null)
            {
                await ctx.Channel.SendMessageAsync("Не удалось подключиться к серверу LavaLink");
                return;
            }

            var searchQuery = await node.Rest.GetTracksAsync(query);
            if (searchQuery.LoadResultType == LavalinkLoadResultType.NoMatches ||
                searchQuery.LoadResultType == LavalinkLoadResultType.LoadFailed)
            {
                await ctx.Channel.SendMessageAsync($"Не удалось найти запрос: {query}");
                return;
            }

            var musicTrack = searchQuery.Tracks.First();

            await connection.PlayAsync(musicTrack);

            string musicDescription = $"Сейчас играет: {musicTrack.Title}\n" +
                                      $"Автор: {musicTrack.Author} \n" +
                                      $"URL: {musicTrack.Uri}";

            var nowPlaingEmbed = new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Purple,
                Title = $"Успешно подключен к каналу {userVC.Name}",
                Description = musicDescription
            };

            await ctx.Channel.SendMessageAsync(embed: nowPlaingEmbed);
        }

        [Command("pause")]
        public async Task PauseMusic(CommandContext ctx)
        {
            var userVC = ctx.Member.VoiceState.Channel;
            var lavalinkInstance = ctx.Client.GetLavalink();

            if (ctx.Member.VoiceState == null || userVC == null)
            {
                await ctx.Channel.SendMessageAsync("User находится не в голосовом чате");
                return;
            }

            if (!lavalinkInstance.ConnectedNodes.Any())
            {
                await ctx.Channel.SendMessageAsync("Соединения не установлено");
                return;
            }

            if (userVC.Type != ChannelType.Voice)
            {
                await ctx.Channel.SendMessageAsync("Голосовой чат не найден");
                return;
            }

            var node = lavalinkInstance.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(ctx.Member.VoiceState.Guild);

            if (connection == null)
            {
                await ctx.Channel.SendMessageAsync("Не удалось подключиться к серверу LavaLink");
                return;
            }
            
            if (connection.CurrentState.CurrentTrack == null)
            {
                await ctx.Channel.SendMessageAsync("В данный момент активных треков нет");
                return;
            }

            await connection.PauseAsync();

            var pausedEmbed = new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Yellow,
                Title = "Трек на паузе"
            };

            await ctx.Channel.SendMessageAsync(embed: pausedEmbed);
        }

        [Command("resume")]
        public async Task ResumeMusic(CommandContext ctx)
        {
            var userVC = ctx.Member.VoiceState.Channel;
            var lavalinkInstance = ctx.Client.GetLavalink();

            if (ctx.Member.VoiceState == null || userVC == null)
            {
                await ctx.Channel.SendMessageAsync("User находится не в голосовом чате");
                return;
            }

            if (!lavalinkInstance.ConnectedNodes.Any())
            {
                await ctx.Channel.SendMessageAsync("Соединения не установлено");
                return;
            }

            if (userVC.Type != ChannelType.Voice)
            {
                await ctx.Channel.SendMessageAsync("Голосовой чат не найден");
                return;
            }

            var node = lavalinkInstance.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(ctx.Member.VoiceState.Guild);

            if (connection == null)
            {
                await ctx.Channel.SendMessageAsync("Не удалось подключиться к серверу LavaLink");
                return;
            }

            if (connection.CurrentState.CurrentTrack == null)
            {
                await ctx.Channel.SendMessageAsync("В данный момент активных треков нет");
                return;
            }

            await connection.ResumeAsync();

            var resumeEmbed = new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Green,
                Title = "Трек возобновлен"
            };

            await ctx.Channel.SendMessageAsync(embed: resumeEmbed);
        }

        [Command("stop")]
        public async Task StopMusic(CommandContext ctx)
        {
            var userVC = ctx.Member.VoiceState.Channel;
            var lavalinkInstance = ctx.Client.GetLavalink();

            if (ctx.Member.VoiceState == null || userVC == null)
            {
                await ctx.Channel.SendMessageAsync("User находится не в голосовом чате");
                return;
            }

            if (!lavalinkInstance.ConnectedNodes.Any())
            {
                await ctx.Channel.SendMessageAsync("Соединения не установлено");
                return;
            }

            if (userVC.Type != ChannelType.Voice)
            {
                await ctx.Channel.SendMessageAsync("Голосовой чат не найден");
                return;
            }

            var node = lavalinkInstance.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(ctx.Member.VoiceState.Guild);

            if (connection == null)
            {
                await ctx.Channel.SendMessageAsync("Не удалось подключиться к серверу LavaLink");
                return;
            }

            if (connection.CurrentState.CurrentTrack == null)
            {
                await ctx.Channel.SendMessageAsync("В данный момент активных треков нет");
                return;
            }

            await connection.StopAsync();
            await connection.DisconnectAsync();

            var stopEmbed = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Red,
                Title = "Трек остановлен",
                Description = "Успешный выход из голосового канала"
            };

            await ctx.Channel.SendMessageAsync(embed: stopEmbed);

        }
    }
}
