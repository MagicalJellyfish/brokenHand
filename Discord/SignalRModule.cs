using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using brokenHand.Discord.Modules.Basic;
using Discord;
using Discord.WebSocket;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;

namespace brokenHand.Discord
{
    public class SignalRModule
    {
        private readonly DiscordSocketClient _discordClient;
        private readonly HubConnection _signalr;
        private readonly IConfiguration _config;

        public SignalRModule(
            DiscordSocketClient discordClient,
            HubConnection signalr,
            IConfiguration config
        )
        {
            _discordClient = discordClient;
            _signalr = signalr;
            _config = config;

            _signalr.On(
                "web/Roll",
                (string roll, RollResult result, ulong discordUserId) =>
                    webRoll(roll, result, discordUserId)
            );
        }

        private async Task webRoll(string roll, RollResult result, ulong discordUserId)
        {
            var channel =
                _discordClient.GetChannel(ulong.Parse(_config.GetSection("Guild")["ChannelId"]!))
                as ITextChannel;
            var user = await channel.GetUserAsync(discordUserId);
            await channel.SendMessageAsync(
                $"{user.Mention} rolled on Website:",
                embed: Constants.RollResultEmbed(roll, result).Build()
            );
        }
    }
}
