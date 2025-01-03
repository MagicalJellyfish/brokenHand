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

        [SlashCommand("str", "Roll for strength")]
        public async Task STR(int? charId = null, bool secret = false)
        {
            await RespondAsync(
                embed: (await _basicService.CharRoll("1d20+STR", Context.User.Id, charId)).Build(),
                ephemeral: secret
            );
        }

        [SlashCommand("dex", "Roll for dexterity")]
        public async Task DEX(int? charId = null, bool secret = false)
        {
            await RespondAsync(
                embed: (await _basicService.CharRoll("1d20+DEX", Context.User.Id, charId)).Build(),
                ephemeral: secret
            );
        }

        [SlashCommand("con", "Roll for constitution")]
        public async Task CON(int? charId = null, bool secret = false)
        {
            await RespondAsync(
                embed: (await _basicService.CharRoll("1d20+CON", Context.User.Id, charId)).Build(),
                ephemeral: secret
            );
        }

        [SlashCommand("int", "Roll for intelligence")]
        public async Task INT(int? charId = null, bool secret = false)
        {
            await RespondAsync(
                embed: (await _basicService.CharRoll("1d20+INT", Context.User.Id, charId)).Build(),
                ephemeral: secret
            );
        }

        [SlashCommand("ins", "Roll for instincts")]
        public async Task INS(int? charId = null, bool secret = false)
        {
            await RespondAsync(
                embed: (await _basicService.CharRoll("1d20+INS", Context.User.Id, charId)).Build(),
                ephemeral: secret
            );
        }

        [SlashCommand("cha", "Roll for charisma")]
        public async Task CHA(int? charId = null, bool secret = false)
        {
            await RespondAsync(
                embed: (await _basicService.CharRoll("1d20+CHA", Context.User.Id, charId)).Build(),
                ephemeral: secret
            );
        }
    }
}
