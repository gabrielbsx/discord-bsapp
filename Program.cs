using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using DiscordBSApp.Logger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DiscordBSApp
{
    public class Program
    {
        private DiscordSocketClient _client;

        public static Task Main(string[] args) => new Program().MainAsync();

        public async Task MainAsync()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddYamlFile("config.yml")
                .Build();

            using IHost host = Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                    services
                        .AddSingleton(config)
                        .AddSingleton(_ => new DiscordSocketClient(new DiscordSocketConfig
                        {
                            GatewayIntents = Discord.GatewayIntents.AllUnprivileged,
                            AlwaysDownloadUsers = true,
                        }))
                        .AddTransient<LoggerImpl>()
                        .AddSingleton(serviceProvider => new InteractionService(serviceProvider.GetRequiredService<DiscordSocketClient>()))
                        .AddSingleton<InteractionHandler>())
                        .Build();

            await RunAsync(host);
        }

        public async Task RunAsync(IHost host)
        {
            using IServiceScope serviceScope = host.Services.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;

            var commands = provider.GetRequiredService<InteractionService>();
            _client = provider.GetRequiredService<DiscordSocketClient>();
            var config = provider.GetRequiredService<IConfigurationRoot>();

            await provider.GetRequiredService<InteractionHandler>().InitializeAsync();
            _client.Log += _ => provider.GetRequiredService<LoggerImpl>().Log(_);
            commands.Log += _ => provider.GetRequiredService<LoggerImpl>().Log(_);
            _client.Ready += async () =>
            {
                if (false)
                {
                    await commands.RegisterCommandsToGuildAsync(UInt64.Parse(config["testGuild"]), true);
                }
                else
                {
                    await commands.RegisterCommandsGloballyAsync(true);
                }
            };
            await _client.LoginAsync(TokenType.Bot, config["tokens:discord"]);
            await _client.StartAsync();
            await Task.Delay(-1);
        }

        static bool IsDebug()
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
    }
}