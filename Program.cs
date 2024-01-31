using brokenHand;
using brokenHand.Discord.Handlers;
using brokenHand.Discord.Modules.Basic;
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
    private static IConfiguration _config;
    private readonly IServiceProvider _serviceProvider = CreateServices();

    static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

    public async Task MainAsync()
    {
        try
        {
            DiscordSocketClient client = _serviceProvider.GetRequiredService<DiscordSocketClient>();

            client.Ready += _client_Ready;

            client.Log += DiscordLog;

            await client.LoginAsync(TokenType.Bot, _config["discordToken"]);
            await client.StartAsync();

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
        _config = new ConfigurationBuilder()
           .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
           .AddEnvironmentVariables()
           .Build();

        var discordConfig = new DiscordSocketConfig()
        {
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
        };

        HttpClient httpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://localhost:7029/api/")
        };

        var collection = new ServiceCollection()
            .AddSingleton(discordConfig)
            .AddSingleton<DiscordSocketClient>()

            .AddSingleton(httpClient)

            .AddSingleton<InteractionService>()
            .AddSingleton<InteractionHandler>()

            .AddSingleton<CommandService>()
            .AddSingleton<CommandHandler>()

            .AddSingleton<BasicService>();

        return collection.BuildServiceProvider();
    }

    private async Task _client_Ready()
    {
        InteractionService interactionService = _serviceProvider.GetRequiredService<InteractionService>();
        await _serviceProvider.GetRequiredService<InteractionHandler>().InitializeAsync();
        await interactionService.RegisterCommandsToGuildAsync(ulong.Parse(_config["guildIds:Bot-Test"]));

        CommandService commandService = _serviceProvider.GetRequiredService<CommandService>();
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