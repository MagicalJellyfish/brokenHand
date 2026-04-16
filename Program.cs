using System.Reflection;
using brokenHand;
using brokenHand.Discord;
using brokenHand.Discord.Handlers;
using brokenHand.Requests.Http;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    private static IConfiguration _config;

    private static HubConnection _hubConnection;
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

            ConnectSignalR();
            _serviceProvider.GetRequiredService<SignalRModule>();

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
            .AddJsonFile("appsettings.json", false, true)
            .Build();

        var discordConfig = new DiscordSocketConfig()
        {
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent,
        };

        SetupSignalr();

        IServiceCollection collection = new ServiceCollection()
            .AddSingleton(_config)
            .AddSingleton(discordConfig)
            .AddSingleton<DiscordSocketClient>()
            .AddSingleton(_hubConnection)
            .AddSingleton<SignalRModule>()
            .AddSingleton(p => new InteractionService(p.GetRequiredService<DiscordSocketClient>()))
            .AddSingleton<InteractionHandler>()
            .AddSingleton<CommandService>()
            .AddSingleton<CommandHandler>();

        collection
            .AddTransient<SuccessOnlyHttpMessageHandler>()
            .AddHttpClient(
                Constants.BrokenHeartClient,
                client =>
                {
                    client.BaseAddress = new Uri($"{_config["brokenHeart:url"]}/api/");
                }
            )
            .AddHttpMessageHandler<SuccessOnlyHttpMessageHandler>();

        List<Type> services = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.Name.EndsWith("Service"))
            .ToList();

        foreach (Type service in services)
        {
            collection.AddSingleton(service);
        }

        return collection.BuildServiceProvider();
    }

    private async Task _client_Ready()
    {
        InteractionService interactionService =
            _serviceProvider.GetRequiredService<InteractionService>();
        await _serviceProvider.GetRequiredService<InteractionHandler>().InitializeAsync();

        await interactionService.RegisterCommandsToGuildAsync(
            ulong.Parse(_config.GetSection("Guild")["GuildId"]!)
        );

        CommandService commandService = _serviceProvider.GetRequiredService<CommandService>();
        await _serviceProvider.GetRequiredService<CommandHandler>().InitializeAsync();
    }

    private static void SetupSignalr()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl($"{_config["brokenHeart:url"]}/signalr")
            .Build();

        _hubConnection.Closed += async (error) => ConnectSignalR();
    }

    private static async void ConnectSignalR()
    {
        try
        {
            await _hubConnection.StartAsync();
            Log("SignalR connection established.");
        }
        catch (Exception ex)
        {
            Log("SignalR connection failed. Waiting 1 minute before reconnecting...");
            await Task.Delay(TimeSpan.FromMinutes(1));
            ConnectSignalR();
        }
    }

    private Task DiscordLog(LogMessage msg)
    {
        string currentTime = DateTime.Now.ToString("HH:mm:ss");
        Console.WriteLine($"[Discord] {currentTime} - {msg.ToString().Substring(9)}");

        return Task.CompletedTask;
    }

    public static void Log(string msg)
    {
        string currentTime = DateTime.Now.ToString("HH:mm:ss");
        Console.WriteLine($"[Custom] {currentTime} - {msg.ToString()}");
    }
}
