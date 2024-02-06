using Discord.Commands;

namespace brokenHand.Discord.Modules.Basic
{
    public class BasicTextModule : ModuleBase<SocketCommandContext>
    {
        public BasicService _basicService;
        public BasicTextModule(HttpClient httpClient)
        {
            _basicService = new BasicService(httpClient);
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
