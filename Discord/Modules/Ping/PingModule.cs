using Discord.Interactions;

namespace brokenHand.Discord.Modules.Ping
{
    public class PingModule : InteractionModuleBase<SocketInteractionContext>
    {
        public PingModule() { }

        [SlashCommand("ping", "Perform a ping")]
        public async Task Ping()
        {
            await RespondAsync("Pong!");
        }
    }
}
