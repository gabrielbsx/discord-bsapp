using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System.Reflection;

namespace DiscordBSApp
{
    public class InteractionHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly InteractionService _commands;
        private readonly IServiceProvider _services;

        public InteractionHandler(DiscordSocketClient client, InteractionService commands, IServiceProvider services)
        {
            _client = client;
            _commands = commands;
            _services = services;
        }

        public async Task InitializeAsync()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            _client.InteractionCreated += HandleInteraction;
            _commands.SlashCommandExecuted += SlashCommandExecuted;
            _commands.ContextCommandExecuted += ContextCommandExecuted;
            _commands.ComponentCommandExecuted += ComponentCommandExecuted;
        }
        private Task ComponentCommandExecuted(ComponentCommandInfo commandInfo, IInteractionContext interactionContext, IResult result)
        {
            return Task.CompletedTask;
        }
        private Task ContextCommandExecuted(ContextCommandInfo commandInfo, IInteractionContext interactionContext, IResult result)
        {
            return Task.CompletedTask;
        }
        private Task SlashCommandExecuted(SlashCommandInfo commandInfo, IInteractionContext interactionContext, IResult result)
        {
            return Task.CompletedTask;
        }

        private async Task HandleInteraction(SocketInteraction interaction)
        {
            try
            {
                var context = new SocketInteractionContext(_client, interaction);
                await _commands.ExecuteCommandAsync(context, _services);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (interaction.Type == InteractionType.ApplicationCommand)
                {
                    await interaction.GetOriginalResponseAsync().ContinueWith(async (message) => await message.Result.DeleteAsync());
                }
            }
        }
    }
}
