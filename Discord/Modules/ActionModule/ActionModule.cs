using brokenHand.Discord.Modules.CombatModule;
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
            await RespondAsync(embed: (await _actionService.ActivateChar(id, Context.User.Id)).Build());
        }
    }
}
