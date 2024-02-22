using Discord;
using Discord.Interactions;

namespace brokenHand.Discord.Modules.CombatModule
{
    [Group("combat", "Manipulate combat")]
    public class CombatModule : InteractionModuleBase<SocketInteractionContext>
    {
        private CombatService _combatService;
        public CombatModule(HttpClient httpClient)
        {
            _combatService = new CombatService(httpClient);
        }

        [SlashCommand("start", "Start a new combat")]
        public async Task StartCombat()
        {
            await RespondAsync(embed: (await _combatService.StartCombat()).Build());
        }

        [SlashCommand("end", "End combat")]
        public async Task EndCombat()
        {
            await RespondAsync(embed: (await _combatService.EndCombat()).Build());
        }

        [SlashCommand("activate", "Replace currently active combat")]
        public async Task ActivateCombat(int id)
        {
            await RespondAsync(embed: (await _combatService.ActivateCombat(id)).Build());
        }

        [SlashCommand("add-char", "Add a character to the currently active combat")]
        public async Task AddChar(int id, int? initRoll = null, string? shortcut = null)
        {
            await RespondAsync(embed: (await _combatService.AddParticipant(id, initRoll, shortcut)).Build());
        }

        [SlashCommand("remove-char", "Remove a character from combat")]
        public async Task RemoveChar(string shortcut)
        {
            await RespondAsync(embed: (await _combatService.RemoveParticipant(shortcut)).Build());
        }

        [SlashCommand("add-event", "Add an event to the currently active combat")]
        public async Task AddEvent(string name, int round, int init = 0, bool secret = false)
        {
            await RespondAsync(embed: (await _combatService.AddEvent(name, round, init, secret)).Build(), ephemeral: secret);
        }

        [SlashCommand("nextturn", "End the current participant's turn and start the next")]
        public async Task NextTurn()
        {
            List<EmbedBuilder> embeds = await _combatService.NextTurn();
            await RespondAsync(embed: embeds.First().Build());
            embeds.RemoveAt(0);

            foreach (EmbedBuilder embed in embeds)
            {
                await ReplyAsync(embed: embed.Build());
            }
        }
    }

    public class CombatModuleShorts : InteractionModuleBase<SocketInteractionContext>
    {
        private CombatService _combatService;
        public CombatModuleShorts(HttpClient httpClient)
        {
            _combatService = new CombatService(httpClient);
        }

        [SlashCommand("next", "End the current participant's turn and start the next")]
        public async Task NextTurnShort()
        {
            List<EmbedBuilder> embeds = await _combatService.NextTurn();
            EmbedBuilder response = embeds.Last();
            embeds.RemoveAt(embeds.Count - 1);
            await RespondAsync(embed: response.Build());

            foreach (EmbedBuilder embed in embeds)
            {
                await ReplyAsync(embed: embed.Build());
            }

        }
    }
}
