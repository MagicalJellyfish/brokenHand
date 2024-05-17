using Discord.Interactions;

namespace brokenHand.Discord.Modules.DefaultsModule
{
    [Group("defaults", "Set defaults to use when not specified")]
    public class DefaultsModule : InteractionModuleBase<SocketInteractionContext>
    {
        private DefaultsService _defaultsService;

        public DefaultsModule(HttpClient httpClient)
        {
            _defaultsService = new DefaultsService(httpClient);
        }

        [SlashCommand("char", "Set your current default character")]
        public async Task DefaultChar(int id)
        {
            await RespondAsync(
                embed: (await _defaultsService.DefaultChar(id, Context.User.Id)).Build()
            );
        }

        [SlashCommand("ability", "Set your current default ability")]
        public async Task DefaultAbility(string shortcut)
        {
            await RespondAsync(
                embed: (await _defaultsService.DefaultAbility(shortcut, Context.User.Id)).Build()
            );
        }

        [SlashCommand("targets", "Set your current default target(s)")]
        public async Task DefaultTargets(string targets)
        {
            await RespondAsync(
                embed: (await _defaultsService.DefaultTargets(targets, Context.User.Id)).Build()
            );
        }
    }
}
