using Discord.Interactions;

namespace brokenHand.Discord.Modules.Basic
{
    public class BasicModule : InteractionModuleBase<SocketInteractionContext>
    {
        public BasicService _basicService;
        public BasicModule(HttpClient httpClient)
        {
            _basicService = new BasicService(httpClient);
        }

        [SlashCommand("roll", "Roll (or calculate) something")]
        public async Task Roll(string roll, bool secret = false)
        {
            await RespondAsync(embed: (await _basicService.RollAsync(roll)).Build(), ephemeral: secret);
        }

        [SlashCommand("d20", "Roll a standard d20, adding f.e. \"+5\" as additional operations")]
        public async Task D20(string roll = "", bool secret = false)
        {
            await RespondAsync(embed: (await _basicService.RollAsync("1d20" + roll)).Build(), ephemeral: secret);
        }
    }
}
