using Discord;
using Discord.Interactions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace brokenHand.Discord.Modules.Basic
{
    public class BasicModule : InteractionModuleBase<SocketInteractionContext>
    {
        public BasicService _basicService;
        public BasicModule(BasicService basicService)
        {
            _basicService = basicService;
        }

        [SlashCommand("roll", "Roll (or calculate) something")]
        public async Task Roll(string roll, bool secret = false)
        {
            RollResult? result = await _basicService.RollAsync(roll);

            if (result == null)
            {
                await RespondAsync("Something went wrong!", ephemeral: secret);
            }

            var embed = await _basicService.RollResultEmbed(roll, result);
            await RespondAsync(embed: embed.Build(), ephemeral: secret);
        }

        [SlashCommand("d20", "Roll a standard d20, adding f.e. \"+5\" as additional operations")]
        public async Task D20(string roll = "", bool secret = false)
        {
            RollResult? result = await _basicService.RollAsync("1d20" + roll);

            if (result == null)
            {
                await RespondAsync("Something went wrong!", ephemeral: secret);
            }

            var embed = await _basicService.RollResultEmbed(roll, result);
            await RespondAsync(embed: embed.Build(), ephemeral: secret);
        }
    }
}
