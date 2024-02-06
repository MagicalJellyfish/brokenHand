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
            await ReplyAsync(embed: (await _basicService.RollAsync(roll)).Build());
        }

        [Command("d20")]
        public async Task D20(string roll = "")
        {
            await ReplyAsync(embed: (await _basicService.RollAsync("1d20" + roll)).Build());
        }
    }
}
