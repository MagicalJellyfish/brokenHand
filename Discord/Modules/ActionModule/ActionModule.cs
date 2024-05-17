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

        [SlashCommand("ability", "Execute an ability")]
        public async Task Ability(
            string? shortcut = null,
            [Summary("targets", "Separate multiple targets with space")] string? targets = null,
            string? charId = null
        )
        {
            List<EmbedBuilder> embeds = await _actionService.Ability(
                Context.User.Id,
                charId,
                shortcut,
                targets
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
