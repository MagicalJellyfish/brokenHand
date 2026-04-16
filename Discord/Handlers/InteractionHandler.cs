using System.Reflection;
using brokenHand.Requests.Http;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace brokenHand.Discord.Handlers
{
    public class InteractionHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly InteractionService _interactionService;
        private readonly IServiceProvider _services;

        public InteractionHandler(
            DiscordSocketClient client,
            InteractionService commands,
            IServiceProvider services
        )
        {
            _client = client;
            _interactionService = commands;
            _services = services;
        }

        public async Task InitializeAsync()
        {
            await _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

            _client.InteractionCreated += HandleInteraction;
            _interactionService.SlashCommandExecuted += HandleCommandError;
        }

        private async Task HandleInteraction(SocketInteraction interaction)
        {
            try
            {
                LogInteraction((SocketSlashCommand)interaction);
                var ctx = new SocketInteractionContext(_client, interaction);
                await _interactionService.ExecuteCommandAsync(ctx, _services);
            }
            catch (Exception ex)
            {
                Program.Log($"Command execution failed, error {ex.Message}.");
            }
        }

        private async Task HandleCommandError(
            SlashCommandInfo commandInfo,
            IInteractionContext interactionContext,
            IResult result
        )
        {
            if (result.GetType() != typeof(ExecuteResult))
                return;
            ExecuteResult executeResult = (ExecuteResult)result;

            if (!executeResult.IsSuccess)
            {
                if (executeResult.Exception.InnerException == null)
                {
                    await interactionContext.Interaction.RespondAsync(executeResult.ErrorReason);
                    return;
                }

                if (
                    executeResult.Exception.InnerException.GetType()
                    == typeof(HttpRequestNoSuccessException)
                )
                {
                    HttpRequestNoSuccessException exception = (HttpRequestNoSuccessException)
                        executeResult.Exception.InnerException;

                    await interactionContext.Interaction.RespondAsync(
                        embed: (await ErrorEmbedFromHttpResponse(exception.Response)).Build()
                    );
                    return;
                }

                await interactionContext.Interaction.RespondAsync(
                    executeResult.Exception.InnerException.Message
                );
            }
        }

        private async Task<EmbedBuilder> ErrorEmbedFromHttpResponse(HttpResponseMessage response)
        {
            string message = await response.Content.ReadAsStringAsync();
            if (!message.StartsWith('{'))
            {
                if (message.Length > 1000)
                {
                    message = Format.Sanitize(message.Split("  at ")[0]);
                }

                return new EmbedBuilder
                {
                    Title = "Error!",
                    Description = message,
                    Color = Color.Red,
                };
            }
            else
            {
                return new EmbedBuilder
                {
                    Title = "Error!",
                    Description = $"{response.StatusCode}: {response.ReasonPhrase}",
                    Color = Color.Red,
                };
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
