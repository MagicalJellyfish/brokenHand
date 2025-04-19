using Discord;
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

        [SlashCommand(
            "roll",
            "Roll (or calculate) something, including references to a character's values"
        )]
        public async Task Roll(string roll, int? charId = null, int repeat = 1, bool secret = false)
        {
            await RollAndRespond(roll, charId, repeat, secret);
        }

        [SlashCommand("d20", "Roll a standard d20, adding f.e. \"+5\" as additional operations")]
        public async Task D20(
            string roll = "",
            int? charId = null,
            int repeat = 1,
            bool secret = false
        )
        {
            await RollAndRespond("1d20" + roll, charId, repeat, secret);
        }

        [SlashCommand("str", "Roll for strength")]
        public async Task STR(int? charId = null, int repeat = 1, bool secret = false)
        {
            await RollAndRespond("1d20+STR", charId, repeat, secret);
        }

        [SlashCommand("dex", "Roll for dexterity")]
        public async Task DEX(int? charId = null, int repeat = 1, bool secret = false)
        {
            await RollAndRespond("1d20+DEX", charId, repeat, secret);
        }

        [SlashCommand("con", "Roll for constitution")]
        public async Task CON(int? charId = null, int repeat = 1, bool secret = false)
        {
            await RollAndRespond("1d20+CON", charId, repeat, secret);
        }

        [SlashCommand("int", "Roll for intelligence")]
        public async Task INT(int? charId = null, int repeat = 1, bool secret = false)
        {
            await RollAndRespond("1d20+INT", charId, repeat, secret);
        }

        [SlashCommand("ins", "Roll for instincts")]
        public async Task INS(int? charId = null, int repeat = 1, bool secret = false)
        {
            await RollAndRespond("1d20+INS", charId, repeat, secret);
        }

        [SlashCommand("cha", "Roll for charisma")]
        public async Task CHA(int? charId = null, int repeat = 1, bool secret = false)
        {
            await RollAndRespond("1d20+CHA", charId, repeat, secret);
        }

        private async Task RollAndRespond(string roll, int? charId, int repeat, bool secret)
        {
            List<EmbedBuilder> embeds = await _basicService.Roll(
                roll,
                Context.User.Id,
                charId,
                repeat
            );

            await RespondAsync(embed: embeds.First().Build(), ephemeral: secret);
            embeds.RemoveAt(0);

            foreach (EmbedBuilder embed in embeds)
            {
                await ReplyAsync(embed: embed.Build());
            }
        }
    }
}
