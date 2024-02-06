using Discord;
using Discord.Commands;
using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace brokenHand.Discord.Modules.Basic
{
    public class BasicTextModule : ModuleBase<SocketCommandContext>
    {
        public BasicService _basicService;
        public BasicTextModule(BasicService basicService)
        {
            _basicService = basicService;
        }

        [Command("roll")]
        public async Task Ping(string roll)
        {
            RollResult? result = await _basicService.RollAsync(roll);

            if (result == null)
            {
                await ReplyAsync("Something went wrong!");
                return;
            }

            var embed = await _basicService.RollResultEmbed(roll, result);
            await ReplyAsync(embed: embed.Build());
        }

        [Command("d20")]
        public async Task D20(string roll = "")
        {
            RollResult? result = await _basicService.RollAsync("1d20" + roll);

            if (result == null)
            {
                await ReplyAsync("Something went wrong!");
                return;
            }

            var embed = await _basicService.RollResultEmbed(roll, result);
            await ReplyAsync(embed: embed.Build());
        }
    }
}
