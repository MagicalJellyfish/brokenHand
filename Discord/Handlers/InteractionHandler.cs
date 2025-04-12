using System.Reflection;
using Discord.Interactions;
using Discord.WebSocket;

namespace brokenHand.Discord.Handlers
{
    public class InteractionHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly InteractionService _commands;
        private readonly IServiceProvider _services;

        public InteractionHandler(
            DiscordSocketClient client,
            InteractionService commands,
            IServiceProvider services
        )
        {
            _client = client;
            _commands = commands;
            _services = services;
        }

        public async Task InitializeAsync()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

            _client.InteractionCreated += HandleInteraction;
        }

        private async Task HandleInteraction(SocketInteraction interaction)
        {
            try
            {
                LogInteraction((SocketSlashCommand)interaction);
                var ctx = new SocketInteractionContext(_client, interaction);
                await _commands.ExecuteCommandAsync(ctx, _services);
            }
            catch (Exception ex)
            {
                Program.Log($"Command execution failed, error {ex.Message}.");
            }
        }

        private void LogInteraction(SocketSlashCommand command)
        {
            List<string> arguments = new List<string>();
            foreach (SocketSlashCommandDataOption option in command.Data.Options)
            {
                arguments.Add($"\r\n    {option.Name} = {option.Value}");
            }
            Program.Log(
                $"Received command {command.Data.Name} with the following arguments: {string.Join("", arguments)}."
            );
        }
    }
}
