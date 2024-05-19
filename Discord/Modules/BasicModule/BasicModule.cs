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
            await RespondAsync(embed: (await _basicService.Roll(roll)).Build(), ephemeral: secret);
        }

        [SlashCommand("d20", "Roll a standard d20, adding f.e. \"+5\" as additional operations")]
        public async Task D20(string roll = "", bool secret = false)
        {
            await RespondAsync(
                embed: (await _basicService.Roll("1d20" + roll)).Build(),
                ephemeral: secret
            );
        }

        [SlashCommand(
            "char-roll",
            "Roll (or calculate) something, including references to a character's values"
        )]
        public async Task CharRoll(string roll, int? charId = null, bool secret = false)
        {
            await RespondAsync(
                embed: (await _basicService.CharRoll(roll, Context.User.Id, charId)).Build(),
                ephemeral: secret
            );
        }
    }
}
