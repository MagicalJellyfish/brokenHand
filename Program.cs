using brokenHand;
using brokenHand.Discord.Handlers;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Reflection;

public class Program
{
    public static IConfiguration config;
    private static DiscordSocketClient _client;
    private InteractionService _interactionService;
    private CommandService _commandService;
    private readonly IServiceProvider _serviceProvider = CreateServices();

    static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

    public async Task MainAsync()
    {
        config = new ConfigurationBuilder()
            .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
            .AddEnvironmentVariables()
            .Build();

        try
        {
            _client = _serviceProvider.GetRequiredService<DiscordSocketClient>();

            _client.Ready += _client_Ready;

            _client.Log += DiscordLog;

            await _client.LoginAsync(TokenType.Bot, config["discordToken"]);
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }
        catch (Exception ex)
        {
            Log(ex.Message);
            Console.Read();
        }
    }

    static IServiceProvider CreateServices()
    {
        var config = new DiscordSocketConfig()
        {
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
        };

        var collection = new ServiceCollection()
            .AddSingleton(config)
            .AddSingleton<DiscordSocketClient>()

            .AddSingleton<InteractionService>()
            .AddSingleton<InteractionHandler>()

            .AddSingleton<CommandService>()
            .AddSingleton<CommandHandler>();

        return collection.BuildServiceProvider();
    }

    private async Task _client_Ready()
    {
        _interactionService = _serviceProvider.GetRequiredService<InteractionService>();
        await _serviceProvider.GetRequiredService<InteractionHandler>().InitializeAsync();
        await _interactionService.RegisterCommandsToGuildAsync(ulong.Parse(config["guildIds:Bot-Test"]));

        _commandService = _serviceProvider.GetRequiredService<CommandService>();
        await _serviceProvider.GetRequiredService<CommandHandler>().InitializeAsync();
    }

    private Task DiscordLog(LogMessage msg)
    {
        string currentTime = DateTime.Now.ToString("HH:mm:ss");
        Console.WriteLine("[Discord] " + currentTime + " - " + msg.ToString().Substring(9));

        return Task.CompletedTask;
    }

    public static void Log(string msg)
    {
        string currentTime = DateTime.Now.ToString("HH:mm:ss");
        Console.WriteLine("[Custom] " + currentTime + " - " + msg.ToString());
    }
}