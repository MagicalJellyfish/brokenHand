using brokenHand.Discord.Modules.CombatModule;
using Discord;
using Discord.Interactions;

namespace brokenHand.Discord.Modules.ActionModule
{
    public class ActionModule : InteractionModuleBase<SocketInteractionContext>
    {
        private ActionService _actionService;

        public ActionModule(HttpClient httpClient)
        {
            _actionService = new ActionService(httpClient);
        }

        [SlashCommand("activate-char", "Replace your currently active character")]
        public async Task ActivateChar(int id)
        {
            await RespondAsync(
                embed: (await _actionService.ActivateChar(id, Context.User.Id)).Build()
            );
        }

        [SlashCommand("char-ability", "Execute an ability")]
        public async Task CharAbility(
            string charId,
            string abilityShortcut,
            string? targets = null,
            bool targetShortcuts = false
        )
        {
            List<EmbedBuilder> embeds = await _actionService.CharAbility(
                charId,
                abilityShortcut,
                targets,
                targetShortcuts
            );
            await RespondAsync(embed: embeds.First().Build());
            embeds.RemoveAt(0);

            foreach (EmbedBuilder embed in embeds)
            {
                await ReplyAsync(embed: embed.Build());
            }
        }

        [SlashCommand("ability", "Execute an ability")]
        public async Task Ability(
            string shortcut,
            string? targets = null,
            bool targetShortcuts = false
        )
        {
            List<EmbedBuilder> embeds = await _actionService.Ability(
                Context.User.Id,
                shortcut,
                targets,
                targetShortcuts
            );
            await RespondAsync(embed: embeds.First().Build());
            embeds.RemoveAt(0);

            foreach (EmbedBuilder embed in embeds)
            {
                await ReplyAsync(embed: embed.Build());
            }
        }
    }
}
